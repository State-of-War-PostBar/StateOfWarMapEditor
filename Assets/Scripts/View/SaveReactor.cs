using UnityEngine;
using UnityEngine.UI;
using System;

using RadiacUI;
using StateOfWarUtility;

namespace MapEditor
{
    [RequireComponent(typeof(Graphic))]
    public sealed class SaveReactor : SignalReceiver
    {
        public string fileSaveSignal;
        
        // public string emitFileSave;
        public float fadePerSecond;
        
        [SerializeField] Color baseColor;
        
        Graphic rd { get { return this.GetComponent<Graphic>(); } }
        
        [SerializeField] Text edtNameText;
        [SerializeField] Text mapNameText;
        
        void Start()
        {
            baseColor = rd.color;
            rd.color = new Color(rd.color.a, rd.color.g, rd.color.b, 0f);
            
            AddCallback(new Signal(fileSaveSignal), () =>
            {
                rd.color = baseColor;
            });
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
