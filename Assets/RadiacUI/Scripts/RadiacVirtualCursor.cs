using UnityEngine;

namespace RadiacUI
{
    public static class VirtualCursor
    {
        // In a Time.deltaTime, which Update() takes.
        public static Vector2 deltaPosition { get; private set; }
        
        static Vector2 curPosition;
        
        // In pixels.
        public static Vector2 position
        {
            get
            {
                return curPosition;
            }
        }
        
        // In [-1, 1].
        public static Vector2 viewportPosition
        {
            get
            {
                return new Vector2(position.x * 2f / VirtualCamera.size.x - 1f, position.y * 2f / VirtualCamera.size.y - 1f);
            }
        }
        
        public static Ray ray
        {
            get
            {
                return VirtualCamera.bindedCamera.ScreenPointToRay(new Vector2(position.x, position.y));
            }
        }
        
        public static bool scrolling
        {
            get
            {
                return Input.mouseScrollDelta.y != 0.0f;
            }
        }
        
        public static float scrollValue
        {
            get
            {
                return Input.mouseScrollDelta.y;
            }
        }
        
        public static void Init()
        {
            if(Component.FindObjectOfType<RadiacCursor>() == null) return;
            
            curPosition = Input.mousePosition;
            
            RadiacEnvironment.RadiacUpdates += () =>
            {
                curPosition += RadiacCursor.inst.speedMult * new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                curPosition = curPosition.Clamp(Vector2.zero, new Vector2(Screen.width - 1, Screen.height - 1));
            };
        }
    }
    
}
