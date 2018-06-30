using UnityEngine;
using UnityEngine.UI;
using System;
using RadiacUI;

namespace MapEditor
{
    [RequireComponent(typeof(RawImage))]
    public sealed class EditorCursor : RadiacCursor
    {
        public static new EditorCursor inst => (EditorCursor)RadiacCursor.inst;
        
        public Color standardColor;
        public Texture2D standardCursor;
        
        public Color movingColor;
        public Texture2D movingCursor;
        
        public bool moving;
        
        public RawImage rd => this.GetComponent<RawImage>();
        
        protected override void Start()
        {
            base.Start();
            
            rd.color = standardColor;
            rd.texture = standardCursor;
        }
        
        protected override void Update()
        {
            base.Update();
            
            if(moving)
            {
                rd.color = movingColor;
                rd.texture = movingCursor;
            }
            else
            {
                rd.color = standardColor;
                rd.texture = standardCursor;
            }
        }
    }
}
