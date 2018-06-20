using System;
using UnityEngine;
using UnityEngine.UI;

namespace RadiacUI
{
    /// <summary>
    /// A cursor replacement for features that is not supported by the original unity input.
    /// </summary>
    public class RadiacCursor : MonoBehaviour
    {
        public static RadiacCursor inst;
        
        protected virtual void Awake()
        {
            inst = this;
        }
        
        public float speedMult = 15.0f;
        
        protected virtual void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            QualitySettings.vSyncCount = 0;
        }
        
        protected virtual void OnDestroy()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        protected virtual void Update()
        {
            // There *might* be an obvious lag of cursor.
            // Obvious in Editor but not obvious in build if hide the cursor.
            // Keep it stay. Keep it good.
            this.gameObject.transform.position = VirtualCursor.position;
        }
        
        
        
    }
    
}
