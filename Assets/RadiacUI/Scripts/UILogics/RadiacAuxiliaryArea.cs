using UnityEngine;

namespace RadiacUI
{
    /// <summary>
    /// Auxiliary rect make responsers possible to react with non-rectangle area.
    /// </summary>
    [RequireComponent(typeof(RadiacUIComponent))]
    public abstract class RadiacAuxiliaryArea : MonoBehaviour
    {
        public abstract bool IsPointInside(Vector2 point);
        
        /// <summary>
        /// A place holder fuinction that allows other components draw its gizmos.
        /// </summary>
        public virtual void CustomDrawGizmos() { }
    }
    
}
