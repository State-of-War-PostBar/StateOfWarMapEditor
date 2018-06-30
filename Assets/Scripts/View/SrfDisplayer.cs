using UnityEngine;
using System;

namespace MapEditor
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class SrfDisplayer : MonoBehaviour
    {
        SpriteRenderer rd => this.GetComponent<SpriteRenderer>();
        
        void Update()
        {
            if(Global.inst.srf != null)
                Refresh();
        }
        
        void Refresh()
        {
            if(rd.sprite.texture != Global.inst.srf)
            {
                rd.sprite = Sprite.Create(
                    Global.inst.srf,
                    new Rect(0 ,-Global.inst.srf.height, Global.inst.srf.width, Global.inst.srf.height),
                    new Vector2(0.0f, 1.0f),
                    1.0f);
                rd.sprite.name = Global.inst.srf.name;
            }
        }
        
    }
    
    
}
