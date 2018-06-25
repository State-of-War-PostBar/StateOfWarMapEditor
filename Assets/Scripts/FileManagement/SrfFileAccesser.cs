using UnityEngine;
using UnityEngine.UI;
using RadiacUI;
using StateOfWarUtility;
using System.IO;

namespace MapEditor
{
    public sealed class SrfFileAccesser : FileAccesser
    {
        
        protected override string textRequest { get { return "SrfFileName"; } }
        protected override string notFound { get { return "$SrfNotFound$"; } }
        protected override string readHint { get { return "$SrfReadHint$"; } }
        
        protected override bool LoadNewFile()
        {
            Texture2D tex = new Texture2D(0, 0, TextureFormat.RGBA32, false);
            
            if(Srf.ValidateSrf(text.text))
                tex.LoadImage(Srf.ToJpg(File.ReadAllBytes(text.text)));
            else if(Srf.ValidateJpg(text.text))
                tex.LoadImage(File.ReadAllBytes(text.text));
            else
                return false;
        
            tex.filterMode = FilterMode.Point;
            tex.name = Path.GetFileName(text.text);
            Global.inst.srfName = text.text;
            Global.inst.srf = tex;
            Global.inst.textAgent.Update(textRequest, Path.GetFileName(text.text));
            
            return true;
        }
        
    }
}
