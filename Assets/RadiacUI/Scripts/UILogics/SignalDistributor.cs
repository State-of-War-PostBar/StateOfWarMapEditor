using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace RadiacUI
{
    public sealed class SignalDistributor : SignalReceiver
    {
        public string fromSignal;
        public string[] toSignals;
        
        void Start()
        {
            foreach(var i in toSignals) if(fromSignal == i)
            {
                throw new InvalidOperationException("Signal " + i + "Cannot be emitted when receiving signal " + fromSignal);
            }
            
            AddCallback(new Signal(fromSignal), () =>
            {
                if(toSignals == null) return;
                
                // Execute one by one, from top to bottom...
                for(int i=0; i<toSignals.Length; i++)
                {
                    SignalManager.EmitSignal(toSignals[i]);
                }
            });
        }
    }
}
