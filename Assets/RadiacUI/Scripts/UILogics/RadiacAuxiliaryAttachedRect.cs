using UnityEngine;
using System;

namespace RadiacUI
{
    /// <summary>
    /// Auxiliary rect make responsers possible to react with non-rectangle area.
    /// This class provides a rectangle defined by a teal RectTransform.
    /// </summary>
    [RequireComponent(typeof(RadiacUIComponent))]
    public class RadiacAuxiliaryAttachedRect : RadiacAuxiliaryRect
    {
        public RectTransform attachment;
        public override Rect rect
        {
            get
            {
                return attachment.rect.Transform(attachment.position);
            }
        }
    }
    
}
