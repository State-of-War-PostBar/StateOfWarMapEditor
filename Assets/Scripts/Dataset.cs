using UnityEngine;
using System;
using System.Collections.Generic;

namespace MapEditor
{
    // Use strings to communnicate.
    public class Dataset : MonoBehaviour
    {
        public Dictionary<string, Sprite> data;
        
        // No checking recently.
        public Sprite this[string name]
        {
            get
            {
                return data[name];
            }
        }
        
    }
    
    
}
