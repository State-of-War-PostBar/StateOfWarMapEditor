using UnityEngine;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RadiacUI
{
    /// <summary>
    /// the Panel is the basic components that interacts with inputs,
    /// including mouse input and keyboard input.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class RadiacPanel : RadiacUIComponent
    {
        internal static readonly HashSet<RadiacPanel> all = new HashSet<RadiacPanel>();
        
        public string[] signalMouseScroll;
        public string[] signalMouseEnter;
        public string[] signalMouseLeave;
        public string[] signalMouseMove;
        
        Vector2 lastCursorPos;
        
        protected RadiacAuxiliaryArea[] aux { get { return this.gameObject.GetComponents<RadiacAuxiliaryRect>(); } }
        protected RectTransform tr { get { return this.gameObject.GetComponent<RectTransform>(); } }
        public bool useBaseRect = true;
        
        /// <summary>
        /// True IFF cursor is pointing to this object or at least one of this object's children.
        /// </summary>
        internal bool cursorHovering { get; private set; }
        
        protected override void Start()
        {
            base.Start();
            
            lastCursorPos = VirtualCursor.position;
            all.Add(this);
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            all.Remove(this);
        }
        
        /// <summary>
        /// This stored value provides ability to measure the "Enter" event without moving cursor through the boundary.
        /// e.g. set a UIComponent to visiable while the cursor is in the territory of this UIComponent.
        /// We're not recording and using "mouse position in last frame" for this calculation.
        /// </summary>
        bool trigLast;
        
        protected override void Update()
        {
            base.Update();
            
            if(cursorHovering && VirtualCursor.scrolling)
            {
                SignalManager.EmitSignal(signalMouseScroll);
            }
            
            if(cursorHovering && !trigLast)
            {
                SignalManager.EmitSignal(signalMouseEnter);
            }
            
            if(!cursorHovering && trigLast)
            {
                SignalManager.EmitSignal(signalMouseLeave);
            }
            
            if(cursorHovering || trigLast)
            {
                if(VirtualCursor.position != lastCursorPos)
                {
                    SignalManager.EmitSignal(signalMouseMove);
                }
            }
            
            trigLast = cursorHovering;
            lastCursorPos = VirtualCursor.position;
        }
        
        /// <summary>
        /// Test if a point is inside the panel's area.
        /// Notice this does not consider neither activity nor depth layout.
        /// </summary>
        internal bool IsPointInsidePanel(Vector2 pos)
        {
            bool x = false;
            
            // Union behaviour:
            // Available if at least one xxxArea contains the point.
            if(useBaseRect) x |= tr.rect.Transform(tr.position).Contains(pos);
            if(aux != null) foreach(var i in aux) x |= i.IsPointInside(pos);
            
            // Intersect behaviour:
            // Available if and only if all masks contains the point.
            if(x)
            {
                foreach(var i in this.transform.FindComponentsInParents<RadiacMaskArea>())
                {
                    bool takeAccount = false;
                    takeAccount |= i.maskType == RadiacMaskArea.MaskType.AllChildren;
                    takeAccount |= i.maskType == RadiacMaskArea.MaskType.DirectChildrenOnly && i.transform == this.transform.parent;
                    if(takeAccount && !i.IsPointInside(pos))
                    {
                        x = false;
                        break;
                    }
                }
            }
            
            return x;
        }
        
        // ============================================================================================================
        // Global static functions...
        // ============================================================================================================
        
        static bool listenerAssigned = false;
        public static void GlobalInit()
        {
            if(listenerAssigned)
            {
                Log.AddWarning("Radiac Panel's updator re-inited!");
            }
            
            RadiacEnvironment.RadiacUpdates += UpdateCursorHovering;
            listenerAssigned = true;
        }
        
        /// <summary>
        /// To build up a listener monitoring cursorHovering property, a static method will be assigned in Start().
        /// </summary>
        static void UpdateCursorHovering()
        {
            // Find what the cursor hits.
            RadiacPanel res = null;
            foreach(var i in all)
            {
                if(i.active && i.IsPointInsidePanel(VirtualCursor.position))
                {
                    if(res == null || i.gameObject.transform.position.z < res.gameObject.transform.position.z)
                    {
                        res = i;
                    }
                }
            }
            
            // Set the hit and its parent objects "cursorHovering".
            foreach(var i in all) i.cursorHovering = false;
            
            while(res != null)
            {
                res.cursorHovering = true;
                res = res.gameObject.transform.parent.GetComponent<RadiacPanel>();
            }
        }
        
        // ============================================================================================================
        // Editor Auxiliary...
        // ============================================================================================================
        
        #if UNITY_EDITOR
        
        public void OnDrawGizmosSelected()
        {
            if(useBaseRect)
            {
                RadiacUtility.DrawRectangleGizmos(tr.rect.Transform(tr.position), tr.position.z + float.Epsilon, Color.red);
            }
            
            if(Selection.activeGameObject != transform.gameObject) return;
            
            // Draw masks which limit this panel's reaction.
            foreach(var i in this.transform.FindComponentsInParents<RadiacMaskArea>())
            {
                bool takeAccount = false;
                takeAccount |= i.maskType == RadiacMaskArea.MaskType.AllChildren;
                takeAccount |= i.maskType == RadiacMaskArea.MaskType.DirectChildrenOnly && i.transform == this.transform.parent;
                if(takeAccount)
                {
                    i.CustomDrawGizmos();
                }
            }
        }
        
        #endif
        
    }
}
