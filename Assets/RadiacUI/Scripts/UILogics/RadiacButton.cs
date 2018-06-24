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
        public string emitRightClick;
        public string emitRightRelease;
        public string emitMiddleClick;
        public string emitMiddleRelease;
        
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
            
            if(cursorHovering && Input.GetMouseButtonDown(1))
            {
                SignalManager.EmitSignal(emitRightClick);
            }
            
            if(cursorHovering && Input.GetMouseButtonUp(1))
            {
                SignalManager.EmitSignal(emitRightRelease);
            }
            
            if(cursorHovering && Input.GetMouseButtonDown(2))
            {
                SignalManager.EmitSignal(emitMiddleClick);
            }
            
            if(cursorHovering && Input.GetMouseButtonUp(2))
            {
                SignalManager.EmitSignal(emitMiddleRelease);
            }
        }
    }
}
