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
        
        
        protected override bool LoadNewFile()
        {
            if(!Map.Validate(text.text)) return false;
            
            Global.inst.map = new Map(text.text);
            Global.inst.mapName = text.text;
            Global.inst.textAgent.Update(textRequest, Path.GetFileName(text.text));
                
            return true;
        }
    }
}
