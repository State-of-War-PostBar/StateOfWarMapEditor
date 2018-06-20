using UnityEngine;
using System;
using System.Text;


namespace RadiacUI
{
    public enum InputOperator
    {
        Backspace = 1,
        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,
        LineBreak,
    }
    
    public static class RadiacInputController
    {
        public static RadiacInputReceiver focusing = null;
        
        /// <summary>
        /// For supporting the long-press repeating.
        /// </summary>
        public const float longPressThreshold = 0.5f; // press a key 0.8s to toggle long press.
        public const float longPressRepeatDelay = 0.03f; // 30ms per input.
        static float longPressCount; // the exceedede time for long press.
        static KeyCode lastPressKey;
        static float lastPressTime;
        
        public static void Init()
        {
            RadiacEnvironment.RadiacUpdates += KeyboardInputDispatch;
            lastPressKey = KeyCode.None;
            // TODO!
            // Load global shortcut configuration.
        }
        
        
        
        public static void KeyboardInputDispatch()
        {
            foreach(KeyCode i in Enum.GetValues(typeof(KeyCode)))
            {
                if(Input.GetKey(i))
                {
                    if(i != lastPressKey)
                    {
                        lastPressKey = i;
                        longPressCount = 0;
                    }
                    break;
                }
            }
            
            // Composite with the input string,
            // it can be easily used for text input and also the other inputs.
            
            string keys = Input.inputString.ToLower();
            
            if(focusing == null)
            {
                // TODO!
                // do nothing recently...
            }
            else
            {
                bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
                bool alt = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
                bool ctrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
                
                foreach(var i in keys)
                {
                    if(!char.IsControl(i))
                    {
                        focusing.ReceiveChar(ctrl, shift, alt, i);
                    }
                }
                
                DealWithOperator(KeyCode.Backspace, InputOperator.Backspace);
                DealWithOperator(KeyCode.LeftArrow, InputOperator.MoveLeft);
                DealWithOperator(KeyCode.RightArrow, InputOperator.MoveRight);
                DealWithOperator(KeyCode.UpArrow, InputOperator.MoveUp);
                DealWithOperator(KeyCode.DownArrow, InputOperator.MoveDown);
                DealWithOperator(KeyCode.Return, InputOperator.LineBreak);
            }
        }
        
        static void DealWithOperator(KeyCode key, InputOperator op)
        {
            if(Input.GetKeyDown(key))
            {
                focusing.ReceiveOperator(op);
            }
            
            // Repeat.
            if(lastPressKey == key && Input.GetKey(key))
            {
                longPressCount += Time.deltaTime;
                if(longPressCount >= longPressThreshold)
                {
                    for(int i=1; i<(longPressCount - longPressThreshold) / longPressRepeatDelay; i++)
                        focusing.ReceiveOperator(op);
                    longPressCount = longPressThreshold + (longPressCount - longPressThreshold) % longPressRepeatDelay;
                }
            }
        }
    }
}
