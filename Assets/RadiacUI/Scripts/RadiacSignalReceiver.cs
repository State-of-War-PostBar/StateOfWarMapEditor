using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace RadiacUI
{
    public abstract class SignalReceiver : MonoBehaviour
    {
        readonly List<Signal> signalList = new List<Signal>();
        readonly List<Action> actionList = new List<Action>();
        
        protected void AddCallback(Signal x, Action action)
        {
            if(x.value == "") return; // do nothing for empty string.
            
            signalList.Add(x);
            actionList.Add(action);
            SignalManager.AddSignalCallback(x, action);
        }
        
        // Remove all callbacks that originally added by this object.
        // Use protected virtual modifier
        // to make sure there won't be an unconscious override.
        protected virtual void OnDestroy()
        {
            for(int i=0; i<signalList.Count; i++)
            {
                SignalManager.RemoveSignalCallback(signalList[i], actionList[i]);
            }
        }
    }
}
