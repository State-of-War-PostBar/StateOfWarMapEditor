using System;
using UnityEngine;

namespace RadiacUI
{
    
    [RequireComponent(typeof(RectTransform))]
    public class RadiacMaskRect : RadiacMaskArea
    {
        RectTransform tr { get { return this.gameObject.GetComponent<RectTransform>(); } }
        public virtual Rect rect { get { return _rect.Transform(tr.position); } }
        
        [SerializeField] Rect _rect;
        
        public override bool IsPointInside(Vector2 point)
            => rect.Contains(point);
        
        public override void CustomDrawGizmos()
        {
            RadiacUtility.DrawRectangleGizmos(rect, tr.position.z + float.Epsilon, Color.blue);
        }
    }
    
    
}
