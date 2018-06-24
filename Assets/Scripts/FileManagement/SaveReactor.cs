using UnityEngine;
using UnityEngine.UI;
using System;

using RadiacUI;
using StateOfWarUtility;

namespace MapEditor
{
    [RequireComponent(typeof(Graphic))]
    public sealed class SaveReactor : MonoBehaviour
    {
        // public string emitFileSave;
        public float fadePerSecond;
        
        [SerializeField] Text edtNameText;
        [SerializeField] Text mapNameText;
        [SerializeField] Color baseColor;
        
        Graphic rd { get { return this.GetComponent<Graphic>(); } }
        
        void Start()
        {
            baseColor = rd.color;
            rd.color = new Color(rd.color.a, rd.color.g, rd.color.b, 0f);
            
            RadiacInputController.KeyboardBypass += (Event e) =>
            {
                if(Input.GetKeyDown(KeyCode.S) && RadiacInputController.ctrl)
                {
                    if(Global.inst.edt != null)
                        Global.inst.edt.Save(edtNameText.text);
                    if(Global.inst.map != null)
                        Global.inst.map.Save(mapNameText.text);
                
                    // SignalManager.EmitSignal(new Signal(emitFileSave));
                    
                    rd.color = baseColor;
                }
            };
        }
        
        void Update()
        {
            rd.color = new Color(
                rd.color.r,
                rd.color.g,
                rd.color.b,
                Mathf.Max(0f, rd.color.a - fadePerSecond * Time.deltaTime));
        }
    }
    
    
}
