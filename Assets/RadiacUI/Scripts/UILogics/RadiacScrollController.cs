using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RadiacUI
{
    /// <summary>
    /// A scroll bar is a complex UI element,
    /// that should contains scrollable multiple panels.
    /// This script is used for dynamically adjusting the reaction area of all sub-panels in scroll bar.
    /// </summary>
    
    // Even if you don't need one, this is necessary to have all logical things done.
    [RequireComponent(typeof(RectMask2D))]
    public class RadiacScrollController : RadiacPanel
    {
        RadiacPanel[] subs
        {
            get
            {
                return RadiacFunctional.Filter<RadiacPanel, List<RadiacPanel>>(
                    GetComponentsInChildren<RadiacPanel>(),
                    (a) => this != a
                    ).ToArray();
            }
        }
        
        public float scrollSpeed;
        
        protected override void Start()
        {
            base.Start();
        }
        
        /// <summary>
        /// Use Bypass to invoke this function.
        /// </summary>
        public void Scroll(float delta)
        {
            foreach(var i in subs)
            {
                i.GetComponent<RectTransform>().anchoredPosition3D += new Vector3(0f, VirtualCursor.scrollValue * scrollSpeed);
            }
            
            // Limit sub-panel to center.
            float topMost = -float.MaxValue;
            float bottomMost = float.MaxValue;
            foreach(var i in subs)
            {
                topMost = Mathf.Max(topMost, i.GetComponent<RectTransform>().anchoredPosition.y);
                bottomMost = Mathf.Min(topMost, i.GetComponent<RectTransform>().anchoredPosition.y);
            }
            
            float dt = 0.0f;
            if(topMost < 0.0f)
            {
                dt = -topMost;
            }
            else if(bottomMost > 0.0f)
            {
                dt = -bottomMost;
            }
            
            if(dt != 0.0f)
            {
                foreach(var i in subs)
                {
                    i.GetComponent<RectTransform>().anchoredPosition3D += new Vector3(0f, dt);
                }
            }
        }
        
    }
    
}
