using UnityEngine;
using RadiacUI;

namespace MapEditor
{
    [DisallowMultipleComponent]
    public sealed class Shortcut : MonoBehaviour
    {
        public string fileSaveSignal;
        
        void Start()
        {
            // Shift + B: Switch the grid pointer.
            RadiacInputController.KeyboardBypass += (Event e) =>
            {
                if(e.shift && e.keyCode == KeyCode.B)
                {
                    Global.inst.showGridPointer = !Global.inst.showGridPointer;
                }
            };
            
            // Shift + S: Save.
            // Notice this key is not affected by RadiacUI.
            RadiacInputController.KeyboardBypass += (Event e) =>
            {
                if(Input.GetKeyDown(KeyCode.S)
                    && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)
                    || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
                {
                    bool saved = false;
                    
                    if(Global.inst.edt != null)
                    {
                        Debug.Log("Edt file saved!");
                        Global.inst.edt.Save(Global.inst.edtName);
                        saved = true;
                    }
                    
                    if(Global.inst.map != null)
                    {
                        Debug.Log("Map file saved!");
                        Global.inst.map.Save(Global.inst.mapName);
                        saved = true;
                    }
                    
                    if(saved)
                    {
                        SignalManager.EmitSignal(new Signal(fileSaveSignal));
                    }
                }
            };
            
        }
        
        
    }
    
    
}
