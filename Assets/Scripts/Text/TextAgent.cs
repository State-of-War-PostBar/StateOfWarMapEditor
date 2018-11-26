using UnityEngine;
using System;
using System.Collections.Generic;

namespace MapEditor
{
    /// <summary>
    /// An agent of listener.
    /// Store traslated values so that text boxes can display them.
    /// Used in Global class.
    /// </summary>
    public sealed class TextAgent
    {
        readonly Dictionary<string, string> map = new Dictionary<string, string>();
        
        /// <summary>
        /// Register a requirement string.
        /// strings can not be re-registered for preventing bugs.
        /// </summary>
        public void Register(string s, string t)
        {
            Register(s);
            Update(s, t);
        }
        
        public void Register(string s)
        {
            if(map.ContainsKey(s))
                throw new InvalidOperationException("re-register a string!");
            map.Add(s, "");
        }
        
        /// <summary>
        /// Update a registered string.
        /// </summary>
        public void Update(string s, string t)
        {
            if(!map.ContainsKey(s))
                throw new InvalidOperationException("string " + s + " not registered.");
            map[s] = t;
        }
        
        public string Get(string s) => (!map.ContainsKey(s)) ? "NotFound:" + s : map[s];
        
        public string this[string s]
        {
            get => Get(s);
            set => Update(s, value);
        }
    }
}
