using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace RadiacUI
{
    /// <summary>
    /// Signals are mainly used to do;
    /// (1) Define interaction of UI elements.
    /// (2) Define interaction of UI and non-UI systems, mainly used to make some command
    ///     so that player can afffect the world.
    /// However the plenty use of signals will make the signal table extremely large
    /// and making a complex information delivering network.
    /// RadiacUI uses these limits to prevent compelx signal networking:
    /// (1) Signal receivers (functions response to signals) are not allowed to emit any signal.
    /// (2) Signals are tagged with the responsed objects if this signal is for UI interactive purpose,
    ///     e.g. "FireControl.Weapons.Display" instead of "DisplayWeapons".
    /// </summary>
    
    // Anyway, this string style is not able to change to enum type without causing corruption...
    public struct Signal : IEquatable<Signal>, IComparable<Signal>
    {
        public string value;
        public Signal(string name) { value = name; }
        public int CompareTo(Signal x) => value.CompareTo(x.value);
        public static bool operator<(Signal a, Signal b) => a.CompareTo(b) < 0;
        public static bool operator==(Signal a, Signal b) => a.CompareTo(b) == 0;
        public static bool operator>(Signal a, Signal b) => a.CompareTo(b) > 0;
        public static bool operator!=(Signal a, Signal b) => a.CompareTo(b) != 0;
        public static bool operator<=(Signal a, Signal b) => a.CompareTo(b) <= 0;
        public static bool operator>=(Signal a, Signal b) => a.CompareTo(b) >= 0;
        public bool Equals(Signal x) => this.CompareTo(x) == 0;
        public override bool Equals(object obj) { throw new NotSupportedException(); }
        public override int GetHashCode() => value.GetHashCode();
        public override string ToString() => value;
    }
    
    public static class SignalManager
    {
        public static bool suspended = false;
        
        static Dictionary<Signal, List<Action>> listeners = new Dictionary<Signal, List<Action>>();
        
        public static void EmitSignal(params string[] x) { if(x != null) foreach(var i in x) EmitSignal(i); }
        public static void EmitSignal(params Signal[] x) { if(x != null) foreach(var i in x) EmitSignal(i); }
        public static void EmitSignal(string x) { EmitSignal(new Signal(x)); }
        public static void EmitSignal(Signal x)
        {
            if(suspended) return;
            if(x.value == null || x.value == "") return;
            Log.AddLogFormat("Signal [{0}] is emitted!", x);
            
            if(listeners.ContainsKey(x))
                foreach(var action in listeners[x]) action(); // Instant callback.
            else
                Log.AddWarningFormat("Can not find receiver for signal: {0}, check typo and so on.", x);
        }
        
        public static void AddSignalCallback(Signal x, Action action)
        {
            Log.AddLogFormat("Add signal [{0}] -> [{1}]", x, action);
            if(!listeners.ContainsKey(x)) listeners.Add(x, new List<Action>());
            listeners[x].Add(action);
        }
        
        public static void RemoveSignalCallback(Signal x, Action action)
        {
            Log.AddLogFormat("Remove signal [{0}] -> [{1}]", x, action);
            listeners[x].Remove(action);
        }
    }
}
