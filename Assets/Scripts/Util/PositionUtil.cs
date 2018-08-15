using UnityEngine;
using System;

namespace MapEditor
{
    public static class PositionUtil
    {
        public static void Set1x1SpritePosition(Transform trans, Rect rect)
        {
            trans.position = rect.center;
            trans.localScale = new Vector2(rect.width, rect.height);
        }
    }
    
    
}
