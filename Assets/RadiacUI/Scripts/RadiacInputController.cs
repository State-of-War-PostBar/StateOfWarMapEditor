using UnityEngine;
using System;
using System.Text;


namespace RadiacUI
{
    public static class RadiacInputController
    {
        public static RadiacInputReceiver focusing = null;
        
        /// <summary>
        /// For supporting the long-press repeating.
        /// </summary>
        public const float longPressThreshold = 0.5f; // press a key 0.8s to toggle long press.
        public const float longPressRepeatDelay = 0.03f; // 30ms per input.
        static float longPressCount; // the exceeded time for long press.
        static float lastPressTime;
        
        public static Action<Event> KeyboardBypass;
        
        public static bool ctrl { get; private set; }
        public static bool shift { get; private set; }
        public static bool alt { get; private set; }
        
        static bool inputStrAssigned = false;
        
        public static void Init()
        {
            RadiacEnvironment.RadiacUpdates += () => inputStrAssigned = false;
            RadiacEnvironment.RadiacGUICallback += GUIUpdate;
            KeyboardBypass = (e) => { };
            
            // TODO!
            // Load global shortcut configuration.
            
        }
        
        public static void GUIUpdate()
        {
            var e = Event.current;
            if(!e.isKey) return;
            if(e.type != EventType.KeyDown && e.type != EventType.KeyUp) return;
            if(e.keyCode == KeyCode.None) return;
            
            ctrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            alt = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
            
            if(focusing == null)
            {
                KeyboardBypass(e);
            }
            else
            {
                if(e.type != EventType.KeyDown) return;
                
                if(e.functionKey || char.IsControl((char)e.keyCode))
                {
                    focusing.ReceiveOperator(e.keyCode);
                }
                else
                {
                    if(!inputStrAssigned)
                    {
                        foreach(var c in Input.inputString) if(!char.IsControl(c))
                        {
                            focusing.ReceiveChar(c);
                        }
                        
                        inputStrAssigned = true;
                    }
                }
            }
        }
    }
}
