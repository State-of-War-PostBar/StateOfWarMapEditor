using UnityEngine;
using UnityEngine.UI;

using System;

namespace RadiacUI
{
    [RequireComponent(typeof(Graphic))]
    [RequireComponent(typeof(RadiacUIComponent))]
    [DisallowMultipleComponent]
    public sealed class RadiacDisplayController : MonoBehaviour
    {
        /// <summary>
        /// The base color property provides a way to modify color outside.
        /// </summary>
        public Color baseColor = Color.white;
        public float fadeSpeed = 1.0f;
        
        Graphic image { get { return this.gameObject.GetComponent<Graphic>(); } }
        RadiacUIComponent uiBase { get { return this.gameObject.GetComponent<RadiacUIComponent>(); } }
        RadiacDisplayController parent { get { return this.gameObject.transform.parent.gameObject.GetComponent<RadiacDisplayController>(); } }
        
        public float selfTransparency;
        public float transparency { get { return (parent == null ? 1.0f : parent.transparency) * selfTransparency; } }
        
        void Start()
        {
            if(fadeSpeed <= 0f) throw new ArgumentOutOfRangeException();
        }
        
        void Update()
        {
            // TODO: the transparency looks weird.
            // Maybe it depends on how pictures are served.
            
            float step = fadeSpeed * Time.deltaTime;
            selfTransparency = Mathf.Clamp(selfTransparency + (uiBase.active ? 1 : -1) * step, 0f, 1.0f);
            image.color = baseColor * new Color(image.color.r, image.color.g, image.color.b, transparency);
        }
    }
}
