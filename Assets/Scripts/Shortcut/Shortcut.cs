using UnityEngine;
using RadiacUI;
using System.IO;
using System;

namespace MapEditor
{
    [DisallowMultipleComponent]
    public sealed class Shortcut : MonoBehaviour
    {
        public string screenShotPath;
        
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
            
            // Print Screen : Capture screenshoot.
            RadiacInputController.KeyboardBypass += (Event e) =>
            {
                if(Input.GetKeyDown(KeyCode.P) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
                {
                    Directory.CreateDirectory(screenShotPath);
                    ScreenCapture.CaptureScreenshot(screenShotPath + DateTime.Now.ToString("yyyy-mm-dd-hh-mm-ss-ffff") + ".png");
                }
            };
            
            // Turn on/off the map displaying.
            // The map displaying will be turned on when enabling map editing without shortkey.
            RadiacInputController.KeyboardBypass += (Event e) =>
            {
                if(Input.GetKeyDown(KeyCode.M))
                {
                    Global.inst.showTiles = !Global.inst.showTiles;
                }
            };
            
            // Show mouse position.
            RadiacInputController.KeyboardBypass += (Event e) =>
            {
                if(Input.GetKey(KeyCode.Semicolon))
                {
                    Global.inst.showMousePosition = !Global.inst.showMousePosition;
                }
            };
            
        }
        
        
    }
    
    
}
