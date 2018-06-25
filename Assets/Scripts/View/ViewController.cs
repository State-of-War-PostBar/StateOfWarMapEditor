using System;
using UnityEngine;

using RadiacUI;
using UnityEngine.UI;

namespace MapEditor
{
    [RequireComponent(typeof(Camera))]
    public sealed class ViewController : SignalReceiver
    {
        public string signalEnterMove;
        public string signalLeaveMove;
        
        public float areaLimit;
        
        public float moveMultiply = 1.0f;
        
        public Vector2 backPosition;
        
        Camera cam { get { return this.GetComponent<Camera>(); } }
        
        [SerializeField] bool moving = false;
        
        void Start()
        {
            VirtualCursor.lockCursor = false;
            
            AddCallback(new Signal(signalEnterMove), () =>
            {
                moving = true;
                VirtualCursor.lockCursor = true;
                EditorCursor.inst.moving = true;
            });
            
            AddCallback(new Signal(signalLeaveMove), () =>
            {
                moving = false;
                VirtualCursor.lockCursor = false;
                EditorCursor.inst.moving = false;
            });
            
            RadiacInputController.KeyboardBypass += (e) =>
            {
                if(e.keyCode == KeyCode.Space)
                {
                    var pos = this.gameObject.transform.position;
                    pos.x = backPosition.x;
                    pos.y = backPosition.y;
                    this.gameObject.transform.position = pos;
                }
            };
        }
        
        void Update()
        {
            if(moving)
            {
                cam.transform.Translate(moveMultiply * VirtualCursor.deltaPosition);
                cam.transform.position = cam.transform.position
                    .Clamp(new Vector3(-areaLimit, -areaLimit, -areaLimit), new Vector3(areaLimit, areaLimit, areaLimit));
            }
        }
        
    }
    
    
    
}
