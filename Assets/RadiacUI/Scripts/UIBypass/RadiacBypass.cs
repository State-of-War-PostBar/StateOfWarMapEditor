using System;
using UnityEngine;

namespace RadiacUI
{
    /// <summary>
    /// The basic bypass class.
    /// A bypass is the module that delivers external information when signal emitted.
    /// This module can be attached to wherever it can be attached,
    /// and grab information when something happened.
    /// </summary>
    public abstract class RadiacBypass : SignalReceiver
    {
         public string signal;
         
         protected virtual void Start()
         {
             if(signal == "")
             {
                 Log.AddWarning(signalEmpty);
             }
             
             AddCallback(new Signal(signal), SignalBypass);
         }
         
         protected virtual void SignalBypass()
         {
             // do nothing...
             // as a place holder.
         }
         
         const string signalEmpty =
            "Bypass doesn't receive any signal!";
    }
}
