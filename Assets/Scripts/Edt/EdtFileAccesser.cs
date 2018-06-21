using UnityEngine;
using UnityEngine.UI;
using RadiacUI;
using StateOfWarUtility;
using System.IO;

namespace MapEditor
{
    public sealed class EdtFileAccesser : RadiacBypass
    {
        const string textRequest = "EdtFileName";
        const string notFound = "$EdtNotFound$";
        const string readHint = "$EdtReadHint$";
        
        public Text text = null;
        
        protected override void Start()
        {
            base.Start();
            Global.inst.textAgent.Register(textRequest);
            Global.inst.textAgent.Update(textRequest, LocalizationSupport.GetLocalizedString(readHint));
        }
        
        protected override void SignalBypass()
        {
            if(Edt.Validate(text.text))
            {
                Global.inst.edt = new Edt(text.text);
                Global.inst.textAgent.Update(textRequest, Path.GetFileName(text.text));
            }
            else
            {
                Global.inst.textAgent.Update(textRequest, LocalizationSupport.GetLocalizedString(notFound));
            }
        }
    }
}
