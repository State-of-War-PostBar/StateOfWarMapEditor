using UnityEngine;
using UnityEngine.UI;
using RadiacUI;
using StateOfWarUtility;
using System.IO;

namespace MapEditor
{
    public sealed class SrfFileAccesser : RadiacBypass
    {
        const string textRequest = "SrfFileName";
        const string notFound = "$SrfNotFound$";
        const string readHint = "$SrfReadHint$";
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
            if(recentFilePath == null || recentFilePath != text.text)
            {
                Texture2D tex = new Texture2D(0, 0, TextureFormat.RGBA32, false);
                
                if(Srf.ValidateSrf(text.text))
                {
                    tex.LoadImage(Srf.ToJpg(File.ReadAllBytes(text.text)));
                }
                else if(Srf.ValidateJpg(text.text))
                {
                    tex.LoadImage(File.ReadAllBytes(text.text));
                }
                else
                {
                    Global.inst.textAgent.Update(textRequest, LocalizationSupport.GetLocalizedString(notFound));
                    return;
                }
            
                tex.filterMode = FilterMode.Point;
                
                tex.name = Path.GetFileName(text.text);
                Global.inst.srf = tex;
                
                Global.inst.textAgent.Update(textRequest, Path.GetFileName(text.text));
                
                recentFilePath = text.text;
            }
        }
        
    }
}
