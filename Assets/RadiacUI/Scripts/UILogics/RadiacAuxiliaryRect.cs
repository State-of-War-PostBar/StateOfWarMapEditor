using UnityEngine;

namespace RadiacUI
{
    /// <summary>
    /// Auxiliary rect make responsers possible to react with non-rectangle area.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(RadiacUIComponent))]
    public class RadiacAuxiliaryRect : RadiacAuxiliaryArea
    {
        RectTransform tr { get { return this.gameObject.GetComponent<RectTransform>(); } }
        public virtual Rect rect { get { return _rect.Transform(tr.position); } }
        
        [SerializeField] Rect _rect;
        
        public override bool IsPointInside(Vector2 point)
            => rect.Contains(point);
        
        public void OnDrawGizmosSelected()
            => RadiacUtility.DrawRectangleGizmos(rect, tr.position.z + float.Epsilon, Color.red);
        
    }
    
}
