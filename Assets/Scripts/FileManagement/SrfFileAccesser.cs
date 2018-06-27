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
        
        protected override bool LoadNewFile(string path)
        {
            Texture2D tex = new Texture2D(0, 0, TextureFormat.RGBA32, false);
            
            if(Srf.ValidateSrf(path))
                tex.LoadImage(Srf.ToJpg(File.ReadAllBytes(path)));
            else if(Srf.ValidateJpg(path))
                tex.LoadImage(File.ReadAllBytes(path));
            else
                return false;
            
            tex.filterMode = FilterMode.Point;
            tex.name = Path.GetFileName(path);
            Global.inst.srfName = path;
            Global.inst.srf = tex;
            Global.inst.textAgent.Update(textRequest, Path.GetFileName(path));
               
            return true;
        }
        
    }
}
