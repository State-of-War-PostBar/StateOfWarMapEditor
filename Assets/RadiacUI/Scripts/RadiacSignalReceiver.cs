using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace RadiacUI
{
    public abstract class SignalReceiver : MonoBehaviour
    {
        readonly Dictionary<Signal, Action> actionList = new Dictionary<Signal, Action>();
        
        protected void AddCallback(Signal x, Action action)
        {
            if(x.value == "") return; // do nothing for empty string.
            
            actionList.Add(x, action);
            SignalManager.AddSignalCallback(x, action);
        }
        
        // Remove all callbacks that originally added by this object.
        // Use protected virtual modifier
        // to make sure there won't be an unconscious override.
        protected virtual void OnDestroy()
        {
            foreach(var i in actionList)
            {
                SignalManager.RemoveSignalCallback(i.Key, i.Value);
            }
        }
    }
}
