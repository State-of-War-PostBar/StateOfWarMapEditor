using UnityEngine;
using UnityEngine.UI;
using RadiacUI;
using StateOfWarUtility;
using System.IO;

namespace MapEditor
{
    public sealed class MapFileAccesser : RadiacBypass
    {
        const string textRequest = "MapFileName";
        const string notFound = "$MapNotFound$";
        const string readHint = "$MapReadHint$";
        
        public Text text = null;
        
        [SerializeField] string recentFilePath = null;
        
        protected override void Start()
        {
            base.Start();
            Global.inst.textAgent.Register(textRequest);
            Global.inst.textAgent.Update(textRequest, LocalizationSupport.GetLocalizedString(readHint));
        }
        
        protected override void SignalBypass()
        {
            if((recentFilePath == null || recentFilePath != text.text))
            {
                if(Map.Validate(text.text))
                {
                    Global.inst.map = new Map(text.text);
                    Global.inst.textAgent.Update(textRequest, Path.GetFileName(text.text));
                    
                    recentFilePath = text.text;
                }
                else
                {
                    Global.inst.textAgent.Update(textRequest, LocalizationSupport.GetLocalizedString(notFound));
                }
            }
        }
    }
}
