// #define RADIAC_DEBUG


using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RadiacUI
{
    [RequireComponent(typeof(Text))]
    [RequireComponent(typeof(RadiacDisplayController))]
    public class RadiacTextInput : RadiacInputReceiver
    {
        Text text { get { return this.gameObject.GetComponent<Text>(); } }
        RectTransform rectTransform { get { return this.gameObject.GetComponent<RectTransform>(); } }
        TextGenerator gen { get { return text.cachedTextGenerator; } }
        
        [SerializeField] Material caretMaterial;
        [SerializeField] Image caret;
        [SerializeField] float caretWidth;
        [SerializeField] AnimationCurve caretAlphaCurve;
        [SerializeField] float caretBlinkSpeedMult = 1.0f;
        
        /// <summary>
        /// Caret position of the whole string.
        /// </summary>
        [SerializeField] int caretPos;
        
        int caretLine
        {
            get
            {
                for(int i=0; i < gen.lineCount - 1; i++)
                {
                    if(gen.lines[i].startCharIdx <= caretPos && caretPos < gen.lines[i+1].startCharIdx) return i;
                }
                return gen.lineCount - 1;
            }
        }
        
        int caretOffset { get { return caretPos - gen.lines[caretLine].startCharIdx; } }
        
        protected override void Start()
        {
            base.Start();
        }
        
        public override void ReceiveOperator(InputOperator op)
        {
            switch(op)
            {
                case InputOperator.Backspace:
                {
                    if(text.text.Length != 0)
                    {
                        if(caretPos != 0)
                        {
                            caretPos--;
                        }
                        text.text = text.text.Remove(caretPos, 1);
                    }
                    break;
                }
                
                case InputOperator.MoveLeft:
                {
                    caretPos = Mathf.Max(0, caretPos - 1);
                    break;
                }
                
                case InputOperator.MoveRight:
                {
                    caretPos = Mathf.Min(text.text.Length, caretPos + 1);
                    break;
                }
                
                case InputOperator.MoveUp:
                {
                    if(caretLine == 0)
                    {
                        caretPos = 0;
                    }
                    else
                    {
                        caretPos -= gen.lines[caretLine].startCharIdx - gen.lines[caretLine-1].startCharIdx;
                    }
                    break;
                }
                
                case InputOperator.MoveDown:
                {
                    if(caretLine == gen.lines.Count - 1)
                    {
                        caretPos = text.text.Length;
                    }
                    else
                    {
                        caretPos += gen.lines[caretLine+1].startCharIdx - gen.lines[caretLine].startCharIdx;
                        caretPos = Mathf.Min(caretPos, text.text.Length);
                    }
                    break;
                }
                
                case InputOperator.LineBreak:
                {
                    text.text = text.text.Insert(caretPos, "\n");
                    caretPos++;
                    break;
                }
                
                default: break;
            }
        }
        
        public override void ReceiveChar(bool ctrl, bool shift, bool alt, char c)
        {
            if(shift) c = char.ToUpper(c);
            text.text = text.text.Insert(caretPos, c.ToString());
            caretPos++;
        }
        
        protected override void Update()
        {
            base.Update();
            DrawCaret();
        }
        
        public void DrawCaret()
        {
            gen.Populate(text.text, text.GetGenerationSettings(rectTransform.rect.size));
            var rect = GetCaretRect(caretPos, caretLine);
            caret.rectTransform.sizeDelta = rect.size;
            caret.rectTransform.position = new Vector3(rect.center.x, rect.center.y, caret.rectTransform.position.y);
            caret.GetComponent<RadiacDisplayController>().baseColor =
                caret.GetComponent<RadiacDisplayController>().baseColor.SetA(
                    (focused ? 1f : 0f) * caretAlphaCurve.Evaluate(Time.time * caretBlinkSpeedMult));
        }
        
        Rect GetCaretRect(int caretPos, int caretLine)
        {
            // Magic! about how to find the caret position...
            // See source code of UGUI InputField
            // https://bitbucket.org/Unity-Technologies/ui/src/a3f89d5f7d145e4b6fa11cf9f2de768fea2c500f/UnityEngine.UI/UI/Core/InputField.cs?at=2017.3
            // Line *1960* Function *GenerateCaret* for more details.
            
            var curOffset = new Vector2(
                gen.characters[caretPos].cursorPos.x,
                gen.lines[caretLine].topY) / text.pixelsPerUnit;
            float height = gen.lines[caretLine].height / text.pixelsPerUnit;
            
            var bottomLeft = rectTransform.position + new Vector3(curOffset.x, curOffset.y - height, -1f);
            var topRight = rectTransform.position + new Vector3(curOffset.x + caretWidth, curOffset.y, -1f);
            
            #if RADIAC_DEBUG
            
            var bottomRight = rectTransform.position + new Vector3(curOffset.x + caretWidth, curOffset.y - height, -1f);
            var topLeft = rectTransform.position + new Vector3(curOffset.x, curOffset.y, -1f);
            
            Debug.DrawLine(bottomLeft, bottomRight, Color.red);
            Debug.DrawLine(bottomRight, topRight, Color.red);
            Debug.DrawLine(topRight, topLeft, Color.red);
            Debug.DrawLine(topLeft, bottomLeft, Color.red);
            
            #endif
            
            return new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
        }
    }
}
