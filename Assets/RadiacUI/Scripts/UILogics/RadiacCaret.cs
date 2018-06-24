using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RadiacUI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Graphic))]
    public sealed class RadiacCaret : MonoBehaviour
    {
        readonly HashSet<RadiacTextInput> reqs = new HashSet<RadiacTextInput>();
        Graphic rd;
        
        public RectTransform rectTransform { get { return rd.rectTransform; } }
        
        public float width;
        [SerializeField] AnimationCurve alphaCurve;
        [SerializeField] float blinkSpeedMult = 1.0f;
        
        void Start()
        {
            rd = this.GetComponent<Graphic>();
            rd.color = rd.color.SetA(0f);
        }
        
        public void RequireDisplay(RadiacTextInput identity)
        {
            if(!reqs.Contains(identity))
                reqs.Add(identity);
        }
        
        public void RevokeDisplay(RadiacTextInput identity)
        {
            if(reqs.Contains(identity))
                reqs.Remove(identity);
        }
        
        
        public void Update()
        {
            if(reqs.Count != 0)
            {
                // set blink.
                rd.color = rd.color.SetA(alphaCurve.Evaluate(Time.time * blinkSpeedMult));
            }
            else
            {
                rd.color = rd.color.SetA(0f);
            }
        }
        
        
    }
    
    
}
