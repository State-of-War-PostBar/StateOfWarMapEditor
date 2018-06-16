using UnityEngine;
using System;

namespace RadiacUI
{
    [RequireComponent(typeof(RadiacScrollController))]
    public class ScrollbarBypass : RadiacBypass
    {
        protected override void SignalBypass()
        {
            this.gameObject.GetComponent<RadiacScrollController>().Scroll(VirtualCursor.scrollValue);
        }
    }
}
