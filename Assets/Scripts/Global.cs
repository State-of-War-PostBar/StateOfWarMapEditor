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
        }
        
        public TextAgent textAgent = new TextAgent();
        public Edt edt;
        public Map map;
        public Texture2D srf;
    }
    
    
    
    
}
