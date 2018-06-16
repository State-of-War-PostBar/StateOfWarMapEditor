using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace RadiacUI
{
    [DisallowMultipleComponent]
    public class RadiacUIComponent : SignalReceiver
    {
        [SerializeField] bool _selfActive;
        public bool selfActive
        {
            get { return _selfActive; }
            
            /// <summary>
            /// Set this property true->true or false->false should not trigger callbacks
            /// because they are not true state switching.
            /// </summary>
            set
            {
                if(_selfActive && !value) deactiveCallback();
                if(!_selfActive && value) activeCallback();
                _selfActive = value;
            }
        }
        
        public Action activeCallback = () => { };
        public Action deactiveCallback = () => { };
        
        public bool active { get { return selfActive && (parent == null || parent.active); } }
        
        RadiacUIComponent parent = null;
        
        public int depth { get { return parent == null ? 1 : parent.depth + 1; } }
        
        public string[] signalActive;
        public string[] signalDeactive;
        
        [SerializeField] string switchSignal = "";
        [SerializeField] string activeSignal = "";
        [SerializeField] string deactiveSignal = "";
        
        protected virtual void Start()
        {
            AddCallback(new Signal(switchSignal), () => selfActive = !selfActive);
            
            if(signalActive.Contains(activeSignal))
            {
                throw new ArgumentException("Signal When Active should not contains Active Signal.");
            }
            AddCallback(new Signal(activeSignal), () => selfActive = true);
            
            if(signalDeactive.Contains(deactiveSignal))
            {
                throw new ArgumentException("Signal When Deactive should not contains Deactive Signal.");
            }
            AddCallback(new Signal(deactiveSignal), () => selfActive = false);
            
            activeCallback += () => SignalManager.EmitSignal(signalActive);
            deactiveCallback += () => SignalManager.EmitSignal(signalDeactive);
            
            var par = this.gameObject.transform.parent.gameObject;
            if(par.GetComponent<Canvas>() == null)
            {
                parent = par.GetComponent<RadiacUIComponent>();
                if(parent == null)
                {
                    throw new Exception("A UI Component must be attached to Canvas directly or has a parent mounted with UI Component.");
                }
            }
        }
        
        protected virtual void Update()
        {
            // Do nothing but preserved for compatibility.
        }
        
    }
    
    
}
