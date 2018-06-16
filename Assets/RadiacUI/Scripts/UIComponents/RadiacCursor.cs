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
        protected virtual void Start()
        {
            // Cursor.lockState = CursorLockMode.Confined;
            // Cursor.visible = false;
            
            QualitySettings.vSyncCount = 0;
        }
        
        protected virtual void OnDestroy()
        {
            Cursor.lockState = CursorLockMode.None;
            // Cursor.visible = true;
        }
        
        protected virtual void Update()
        {
            // The position sync is direct snyc.
            // Turn off the V-Blank sync will recduce the latency to exact one frame.
            // Or the latency will be obvious...
            this.gameObject.transform.position = VirtualCursor.position;
        }
        
        
        
    }
    
}
