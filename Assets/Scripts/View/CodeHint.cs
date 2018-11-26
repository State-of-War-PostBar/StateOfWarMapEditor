using UnityEngine;
using UnityEngine.UI;
using RadiacUI;

[DisallowMultipleComponent]
public class CodeHint : SignalReceiver
{
    public static CodeHint inst;
    CodeHint() => inst = this;
}
