using UnityEngine;
using UnityEngine.UI;
using RadiacUI;
using StateOfWarUtility;
using System.IO;

namespace MapEditor
{
    public sealed class MapFileAccesser : FileAccesser
    {
        protected override string textRequest => "MapFileName";
        protected override string notFound => "$MapNotFound$";
        protected override string readHint => "$MapReadHint$";
        
        protected override bool LoadNewFile(string path)
        {
            if(!Map.Validate(path)) return false;
            
            Global.inst.map = new Map(path);
            Global.inst.mapName = path;
            Global.inst.textAgent.Update(textRequest, Path.GetFileName(path));
                
            return true;
        }
    }
}
