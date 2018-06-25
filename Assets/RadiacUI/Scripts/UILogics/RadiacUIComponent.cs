using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace RadiacUI
{
    [DisallowMultipleComponent]
    public class RadiacUIComponent : SignalReceiver
    {
        public string elementName;
        
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
        
        RadiacUIComponent parent { get { return this.transform.parent.GetComponent<RadiacUIComponent>(); } }
        
        public int depth { get { return parent == null ? 1 : parent.depth + 1; } }
        
        public string emitActive = "";
        public string emitDeactive = "";
        
        public string switchSignal = "";
        public string activeSignal = "";
        public string deactiveSignal = "";
        
        protected virtual void Start()
        {
            emitActive = ParseRequest(emitActive);
            emitDeactive = ParseRequest(emitDeactive);
            switchSignal = ParseRequest(switchSignal);
            activeSignal = ParseRequest(activeSignal);
            deactiveSignal = ParseRequest(deactiveSignal);
            
            AddCallback(new Signal(switchSignal), () => selfActive = !selfActive);
            
            if(emitActive != "" && emitActive == activeSignal)
            {
                throw new ArgumentException("Signal When Active should not contains Emit Active.");
            }
            AddCallback(new Signal(activeSignal), () => selfActive = true);
            
            if(emitDeactive != "" && emitDeactive == deactiveSignal)
            {
                throw new ArgumentException("Signal When Deactive should not contains Emit Deactive.");
            }
            AddCallback(new Signal(deactiveSignal), () => selfActive = false);
            
            activeCallback += () => SignalManager.EmitSignal(emitActive);
            deactiveCallback += () => SignalManager.EmitSignal(emitDeactive);
            
            if(parent == null && this.transform.parent.GetComponent<Canvas>() == null)
            {
                throw new Exception("A UI Component must be attached to Canvas directly or has a parent mounted with UI Component.");
            }
        }
        
        protected virtual void Update()
        {
            // Do nothing but preserved for compatibility.
        }
        
        protected string ParseRequest(string request)
        {
            if(request == null || request == "") return request;
            if(request[0] != '.') return request;
            // Syntax:
            // a point '.' represents current object (this).
            // totally n points represents the (n-1)th parent.
            // Get its name and convert the string.
            int cnt = 1;
            RadiacUIComponent obj = this;
            while(request[cnt] == '.')
            {
                obj = obj?.parent;
                cnt++;
            }
            if(obj == null)
                throw new InvalidOperationException("Component " + elementName + " doesn't have " + (cnt - 1) + " level parent!");
            return obj.elementName + request.Substring(cnt - 1);
        }
    }
}
