using UnityEngine;
using UnityEngine.UI;
using RadiacUI;
using StateOfWarUtility;
using System.IO;

namespace MapEditor
{
    public sealed class MapFileAccesser : FileAccesser
    {
        protected override string textRequest { get { return "MapFileName"; } }
        protected override string notFound { get { return "$MapNotFound$"; } }
        protected override string readHint { get { return "$MapReadHint$"; } }
        
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
