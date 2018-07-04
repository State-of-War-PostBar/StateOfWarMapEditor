using UnityEngine;
using RadiacUI;
using System.IO;
using System;
using System.Runtime.InteropServices;
using StateOfWarUtility;

namespace MapEditor
{
    // Shortcuts:
    // [Shift + Esc | Ctrl + Esc] quit. 
    // [Shift + B] switch the grid pointer.
    // [Shift + S | Ctrl + S] save.
    // [Shift + P] screenshot.
    // [Shift + M] switch the map display.
    // [Shift + ;] switch mouse position display.
    // [Shift + T] switch building and units taken place display.
    
    // [Shift + D] synchonrize selected object's position with mouse.
    
    // [Ctrl + C | Shift + C] copy.
    // [Ctrl + V | Shift + V] paste.
    // [Ctrl + X | Shift + X] cut.
    
    // [Delete | Shift + Tab] delete.
    // [Shift + W] create building.
    // [Shift + E] create unit.
    
    [DisallowMultipleComponent]
    public sealed class Shortcut : MonoBehaviour
    {
        public string screenShotPath;
        public string fileSaveSignal;
        
        void Start()
        {
            // [Shift + Esc | Ctrl + Esc] quit.
            RadiacInputController.KeyboardBypass += (Event e) =>
            {
                if(e.shift || e.control)
                {
                    if(Input.GetKeyDown(KeyCode.Escape))
                    {
                        Application.Quit();
                    }
                }
            };
            
            // [Shift + B] switch the grid pointer.
            RadiacInputController.KeyboardBypass += (Event e) =>
            {
                if(e.shift && e.keyCode == KeyCode.B && e.type == EventType.KeyDown)
                {
                    Global.inst.showGridPointer = !Global.inst.showGridPointer;
                }
            };
            
            // [Shift + S | Ctrl + S] save.
            // Notice this key is not affected by RadiacUI.
            RadiacInputController.KeyboardBypass += (Event e) =>
            {
                if(Input.GetKeyDown(KeyCode.S) && (e.shift || e.control))
                {
                    bool saved = false;
                    
                    if(Global.inst.edt != null)
                    {
                        Global.inst.edt.Save(Global.inst.edtName);
                        saved = true;
                    }
                    
                    if(Global.inst.map != null)
                    {
                        Global.inst.map.Save(Global.inst.mapName);
                        saved = true;
                    }
                    
                    if(saved)
                    {
                        SignalManager.EmitSignal(new Signal(fileSaveSignal));
                    }
                }
            };
            
            // [Shift + P] screenshot.
            RadiacInputController.KeyboardBypass += (Event e) =>
            {
                if(Input.GetKeyDown(KeyCode.P) && e.shift)
                {
                    Directory.CreateDirectory(screenShotPath);
                    ScreenCapture.CaptureScreenshot(screenShotPath + DateTime.Now.ToString("yyyy-mm-dd-hh-mm-ss-ffff") + ".png");
                }
            };
            
            // [Shift + M] switch the map display.
            // The map displaying will be turned on when enabling map editing without shortkey.
            RadiacInputController.KeyboardBypass += (Event e) =>
            {
                if(Input.GetKeyDown(KeyCode.M) && e.shift)
                {
                    Global.inst.showTiles = !Global.inst.showTiles;
                }
            };
            
            // [Shift + ;] switch mous position display.
            RadiacInputController.KeyboardBypass += (Event e) =>
            {
                if(Input.GetKeyDown(KeyCode.Semicolon) && e.shift)
                {
                    Global.inst.showMousePosition = !Global.inst.showMousePosition;
                }
            };
            
            // [Shift + D] synchonrize selected object's position with mouse.
            // RadiacInputController.KeyboardBypass += (Event e) =>
            // {
            //     if(Input.GetKeyDown(KeyCode.D) && e.shift)
            //     {
            //         Global.inst.syncPosition = true;
            //     }
            //     else if(Input.GetKeyUp(KeyCode.D) || !e.shift)
            //     {
            //         Global.inst.syncPosition = false;
            //     }
            // };
            
            // [Ctrl + C | Shift + C] copy.
            RadiacInputController.KeyboardBypass += (Event e) =>
            {
                // This sentence is not necessary since there can't be a selction when edt file is not loaded.
                // Preserved for bug-preventing...
                if(Global.inst.edt == null) return;
                if(!Global.inst.selection.selected) return;
                
                if(Input.GetKeyDown(KeyCode.C) && (e.control || e.shift))
                {
                    if(Global.inst.selection.building != null)
                    {
                        Global.inst.clipBoard = Global.inst.selection.building.Clone();
                    }
                    else if(Global.inst.selection.battleUnit != null)
                    {
                        Global.inst.clipBoard = Global.inst.selection.battleUnit.Clone();
                    }
                }
            };
            
            // [Ctrl + V | Shift + V] paste.
            RadiacInputController.KeyboardBypass += (Event e) =>
            {
                if(Global.inst.edt == null) return;
                
                if(Input.GetKey(KeyCode.V) && (e.control || e.shift))
                {
                    if(Global.inst.clipBoard is Building)
                    {
                        // add to the tail; select the tail.
                        var x = Global.inst.edt.buildings.Add();
                        x.Assign(Global.inst.clipBoard);
                        Global.inst.selection.SetBuilding(Global.inst.edt.buildings.count - 1);
                        x.x = Global.inst.cursorPointingGrid.x;
                        x.y = Global.inst.cursorPointingGrid.y;
                    }
                    else if(Global.inst.clipBoard is Unit)
                    {
                        // add to the tail; select the tail.
                        var x = Global.inst.edt.units.Add();
                        x.Assign(Global.inst.clipBoard);
                        Global.inst.selection.SetUnit(Global.inst.edt.units.count - 1);
                        x.x = Global.inst.cursorPointingGrid.x;
                        x.y = Global.inst.cursorPointingGrid.y;
                    }
                }
            };
            
            // [Ctrl + X | Shift + X] cut.
            // This equals copy and remove.
            RadiacInputController.KeyboardBypass += (Event e) =>
            {
                if(Global.inst.edt == null) return;
                if(!Global.inst.selection.selected) return;
                
                if(Input.GetKey(KeyCode.X) && (e.control || e.shift))
                {
                    if(Global.inst.selection.building != null)
                    {
                        Global.inst.clipBoard = Global.inst.selection.building.Clone();
                        Global.inst.edt.buildings.Remove(Global.inst.selection.id);
                    }
                    else if(Global.inst.selection.battleUnit != null)
                    {
                        Global.inst.clipBoard = Global.inst.selection.battleUnit.Clone();
                        Global.inst.edt.units.Remove(Global.inst.selection.id);
                    }
                    Global.inst.selection.Reset();
                }
            };
            
            // [Delete | Shift + Tab] delete current selecetion.
            RadiacInputController.KeyboardBypass += (Event e) =>
            {
                if(Global.inst.edt == null) return;
                if(!Global.inst.selection.selected) return;
                
                if(Input.GetKey(KeyCode.Delete)
                    || (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift)))
                {
                    if(Global.inst.selection.building != null)
                    {
                        Global.inst.edt.buildings.Remove(Global.inst.selection.id);
                    }
                    else if(Global.inst.selection.battleUnit != null)
                    {
                        Global.inst.edt.units.Remove(Global.inst.selection.id);
                    }
                    Global.inst.selection.Reset();
                }
            };
            
            // [Shift + W] create building.
            RadiacInputController.KeyboardBypass += (Event e) =>
            {
                if(Global.inst.edt == null) return;
                if(Input.GetKey(KeyCode.W) && e.shift)
                {
                    var x = Global.inst.edt.buildings.Add();
                    x.x = Global.inst.cursorPointingGrid.x;
                    x.y = Global.inst.cursorPointingGrid.y;
                    Global.inst.selection.SetBuilding(Global.inst.edt.buildings.count - 1);
                }
            };
            
            // [Shift + E] create unit.
            RadiacInputController.KeyboardBypass += (Event e) =>
            {
                if(Global.inst.edt == null) return;
                if(Input.GetKey(KeyCode.E) && e.shift)
                {
                    var x = Global.inst.edt.units.Add();
                    var pos = (Global.inst.cursorPointingGrid + new Vector2(0.5f, 0.5f)) * Global.gridSize;
                    x.x = (int)pos.x;
                    x.y = (int)pos.y;
                    Global.inst.selection.SetUnit(Global.inst.edt.units.count - 1);
                }
            };
            
            RadiacInputController.KeyboardBypass += (Event e) =>
            {
                if(Input.GetKeyDown(KeyCode.T) && e.shift)
                {
                    Global.inst.showDecoration = !Global.inst.showDecoration;
                }
            };
        }
        
        
    }
    
    
}
