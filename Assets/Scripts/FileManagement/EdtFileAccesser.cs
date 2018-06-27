using UnityEngine;
using UnityEngine.UI;
using RadiacUI;
using StateOfWarUtility;
using System.IO;

namespace MapEditor
{
    public sealed class EdtFileAccesser : FileAccesser
    {
        protected override string textRequest { get { return "EdtFileName"; } }
        protected override string notFound { get { return "$EdtNotFound$"; } }
        protected override string readHint { get { return "$EdtReadHint$"; } }
        
        protected override bool LoadNewFile(string path)
        {
            if(!Edt.Validate(path)) return false;
            
            Global.inst.edt = new Edt(path);
            Global.inst.edtName = path;
            Global.inst.textAgent.Update(textRequest, Path.GetFileName(path));
            
            Global.inst.selection.id = -1;
            Global.inst.selection.isUnit = true;
            Global.inst.selection.selected = false;
            
            return true;
        }
    }
}
