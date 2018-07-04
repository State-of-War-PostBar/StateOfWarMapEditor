using System;
using UnityEngine;
using UnityEngine.UI;

using System.Runtime.InteropServices;

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
        
        // Used by virtual cursor.
        public float speedMult = 15.0f;
        
        // ============================================================================================================
        // Windows Specific Section
        // ============================================================================================================
        
        #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        
        [DllImport("user32")]
        static extern int SetCursorPos(int x, int y);
        
        [DllImport("user32")]
        static extern IntPtr GetActiveWindow();
        
        [DllImport("user32")]
        static extern bool GetWindowRect(IntPtr window, IntPtr rect);
        
        [StructLayout(LayoutKind.Explicit)]
        struct WinRect
        {
            [FieldOffset(0x00)] public int left;
            [FieldOffset(0x04)] public int top;
            [FieldOffset(0x08)] public int right;
            [FieldOffset(0x0C)] public int bottom;
        }
        
        bool focused = false;
        
        protected virtual void Start()
        {
            focused = true;
        }
        
        protected virtual void OnDestroy()
        {
            Cursor.visible = true;
        }
        
        protected virtual void Update()
        {
            Cursor.visible = false;
            
            if(Input.GetKey(KeyCode.Mouse0))
            {
                focused = true;
            }
            
            if(focused)
            {
                SetCursorPos(0, 0);
                unsafe
                {
                    WinRect rect = new WinRect();
                    var window = GetActiveWindow();
                    GetWindowRect(window, new IntPtr(&rect));
                    SetCursorPos((rect.left + rect.right) / 2, (rect.top + rect.bottom) / 2);
                }
            }
            
            // There *might* be an obvious lag of cursor.
            // Obvious in Editor but not obvious in build if hide the cursor.
            // Keep it stay. Keep it good.
            this.gameObject.transform.position = VirtualCursor.position;
        }
        
        protected virtual void OnApplicationFocus(bool isFocus)
        {
            focused = isFocus;
        }
    
        #endif // UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    }
    
}
