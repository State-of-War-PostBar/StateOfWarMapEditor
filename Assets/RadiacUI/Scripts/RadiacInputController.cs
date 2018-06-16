using UnityEngine;
using System;


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
        
        public static void Init()
        {
            RadiacEnvironment.RadiacUpdates += KeyboardInputDispatch;
            
            // TODO!
            // Load global shortcut configuration.
        }
        
        public static void KeyboardInputDispatch()
        {
            // Composite with the input string,
            // it can be easily used for text input and also the other inputs.
            
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
                string keys = Input.inputString.ToLower();
                foreach(var c in keys)
                {
                    if(!char.IsControl(c))
                    {
                        focusing.ReceiveChar(ctrl, shift, alt, c);
                    }
                }
                
                if(Input.GetKeyDown(KeyCode.Backspace))
                {
                    focusing.ReceiveOperator(InputOperator.Backspace);
                }
                
                if(Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    focusing.ReceiveOperator(InputOperator.MoveLeft);
                }
                
                if(Input.GetKeyDown(KeyCode.RightArrow))
                {
                    focusing.ReceiveOperator(InputOperator.MoveRight);
                }
                
                if(Input.GetKeyDown(KeyCode.UpArrow))
                {
                    focusing.ReceiveOperator(InputOperator.MoveUp);
                }
                
                if(Input.GetKeyDown(KeyCode.DownArrow))
                {
                    focusing.ReceiveOperator(InputOperator.MoveDown);
                }
                
                if(Input.GetKeyDown(KeyCode.Return))
                {
                    focusing.ReceiveOperator(InputOperator.LineBreak);
                }
            }
        }
    }
}
