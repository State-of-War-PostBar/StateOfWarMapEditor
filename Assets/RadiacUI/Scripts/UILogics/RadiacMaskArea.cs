using System;
using UnityEngine;
using UnityEngine.UI;

namespace RadiacUI
{
    /// <summary>
    /// Mask for reaction purpose.
    /// </summary>
    [RequireComponent(typeof(RadiacUIComponent))]
    public abstract class RadiacMaskArea : MonoBehaviour
    {
        [SerializeField]
        public enum MaskType
        {
            DirectChildrenOnly = 1,
            AllChildren = 2
        }
        
        public MaskType maskType = MaskType.DirectChildrenOnly;
        public abstract bool IsPointInside(Vector2 point);
        
        /// <summary>
        /// A place holder fuinction that allows other components draw its gizmos.
        /// </summary>
        public virtual void CustomDrawGizmos() { }
    }
}
