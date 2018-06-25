using UnityEngine;
using System;

using StateOfWarUtility;

namespace MapEditor
{
    /// <summary>
    /// Where global resources are used.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class Global : MonoBehaviour
    {
        public static Global inst;
        
        void Awake()
        {
            if(inst != null)
                throw new InvalidOperationException("Re-init global object!");
            inst = this;
            
            showGridPointer = true;
        }
        
        public TextAgent textAgent = new TextAgent();
        
        public Edt edt;
        public string edtName;
        
        public Map map;
        public string mapName;
        
        public Texture2D srf;
        public string srfName;
        
        public bool showGridPointer;
        
        
        public Vector2 cursorPointing
        {
            get
            {
                return RadiacUI.VirtualCursor.position
                    + (Vector2)Camera.main.transform.position
                    - new Vector2(Screen.width, Screen.height) * 0.5f;
            }
        }
        
        public Vector2Int cursorPointingGrid
        {
            get
            {
                // TODO:
                // Hard code grid size.
                // This should be move to other place...
                // Alongside the same value that appears everywhere.
                var cur = cursorPointing / 32.0f;
                return new Vector2Int(Mathf.FloorToInt(cur.x), Mathf.FloorToInt(cur.y));
            }
        }
    }
    
    
    
    
}
