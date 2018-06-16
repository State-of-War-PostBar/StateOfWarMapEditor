using UnityEngine;
using System;
using System.Collections.Generic;

namespace RadiacUI
{
    public struct KeyShortcut : IComparable<KeyShortcut>, IEquatable<KeyShortcut>
    {
        // Modifier keys are hard-coded.
        public bool ctrl;
        public bool shift;
        public bool alt;
        public KeyCode key;
        
        public int CompareTo(KeyShortcut x)
        {
            if(ctrl != x.ctrl) return ctrl ? 1 : -1;
            if(shift != x.shift) return shift ? 1 : -1;
            if(alt != x.alt) return alt ? 1 : -1;
            return key == x.key ? 0 : key < x.key ? -1 : 1;
        }
        
        public static bool operator<(KeyShortcut a, KeyShortcut b) { return a.CompareTo(b) < 0; }
        public static bool operator==(KeyShortcut a, KeyShortcut b) { return a.CompareTo(b) == 0; }
        public static bool operator>(KeyShortcut a, KeyShortcut b) { return a.CompareTo(b) > 0; }
        public static bool operator!=(KeyShortcut a, KeyShortcut b) { return a.CompareTo(b) != 0; }
        public static bool operator<=(KeyShortcut a, KeyShortcut b) { return a.CompareTo(b) <= 0; }
        public static bool operator>=(KeyShortcut a, KeyShortcut b) { return a.CompareTo(b) >= 0; }
        public bool Equals(KeyShortcut x) { return this.CompareTo(x) == 0; }
        public override int GetHashCode()
        {
            return key.GetHashCode() + ((ctrl ? 1 : 0) << 28) + ((shift ? 1 : 0) << 29) + ((alt ? 1 : 0) << 30);
        }
        public override bool Equals(object x) { throw new NotSupportedException(); }
    }
    
    public static class ShortcutDatabase
    {
        public static Dictionary<KeyShortcut, string> events = new Dictionary<KeyShortcut, string>();
        
        static ShortcutDatabase()
        {
            RadiacEnvironment.RadiacUpdates += () =>
            {
                foreach(var i in events)
                {
                    bool ctrlEnabled = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || !i.Key.ctrl;
                    bool shiftEnabled = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || !i.Key.shift;
                    bool altEnabled = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt) || !i.Key.alt;
                    bool keyEnabled = Input.GetKeyDown(i.Key.key);
                    if(ctrlEnabled && shiftEnabled && altEnabled && keyEnabled)
                    {
                        SendEvent(i.Value);
                    }
                }
            };
        }
        
        static void SendEvent(string eventName)
        {
            // TODO!
        }
    }
    
}
