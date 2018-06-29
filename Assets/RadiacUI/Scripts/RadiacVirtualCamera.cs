using UnityEngine;

namespace RadiacUI
{
    public static class VirtualCamera
    {
        public static Camera bindedCamera
        {
            get
            {
                return Camera.main;
            }
        }
        
        public static Vector3 position
        {
            get
            {
                return bindedCamera.transform.position;
            }
        }
        
        public static Vector3 direction
        {
            get
            {
                return bindedCamera.transform.rotation * Vector3.forward;
            }
        }
        
        // Also the resolution.
        public static Vector2 size { get; private set; }
        
        
        public static void Init()
        {
            RadiacEnvironment.RadiacUpdates += () =>
            {
                // The Screen.width and Screen.height is correct only if in Update().
                // This value will be the window size if access in editor window rendering.
                // This is also why we don't directly access Screen.* in size property.
                size = new Vector2(Screen.width, Screen.height);
            };
        }
        
    }
    
}
