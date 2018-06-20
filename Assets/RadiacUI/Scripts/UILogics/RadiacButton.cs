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
        public string emitMouseClick;
        public string emitMouseRelease;
        
        protected override void Update()
        {
            emitMouseClick = ParseRequest(emitMouseClick);
            emitMouseRelease = ParseRequest(emitMouseRelease);
            base.Update();
            
            if(cursorHovering && Input.GetMouseButtonDown(0))
            {
                SignalManager.EmitSignal(emitMouseClick);
            }
            
            if(cursorHovering && Input.GetMouseButtonUp(0))
            {
                SignalManager.EmitSignal(emitMouseRelease);
            }
            
        }
    }
}
