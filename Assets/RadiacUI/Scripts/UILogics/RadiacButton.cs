using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace RadiacUI
{
    /// <summary>
    /// Button supports click, pressing and release events.
    /// 
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class RadiacButton : RadiacPanel
    {
        public string[] signalMouseClick;
        public string[] signalMouseRelease;
        
        protected override void Update()
        {
            base.Update();
            
            if(cursorHovering && Input.GetMouseButtonDown(0))
            {
                SignalManager.EmitSignal(signalMouseClick);
            }
            
            if(cursorHovering && Input.GetMouseButtonUp(0))
            {
                SignalManager.EmitSignal(signalMouseRelease);
            }
            
        }
    }
}
