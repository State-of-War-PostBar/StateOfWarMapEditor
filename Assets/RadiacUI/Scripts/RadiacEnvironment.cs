using UnityEngine;
using System;

namespace RadiacUI
{
    /// <summary>
    /// This class provides a global Update and FixedUpdate used by Radaic UI system.
    /// In theory this class can be mounted on any GameObject that will never be destroyed.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class RadiacEnvironment : MonoBehaviour
    {
        public static RadiacEnvironment instance;
        public static Action RadiacUpdates;
        public static Action RadiacFixedUpdates;
        public static Action RadiacGUICallback;
        
        void Awake()
        {
            if(instance != null) Log.AddWarning("Radiac Environment replaced.");
            
            instance = this;
            
            RadiacUpdates = () => { };
            RadiacFixedUpdates = () => { };
            RadiacGUICallback = () => { };
            
            // Static initialization are placed here.
            
            LocalizationSupport.LoadLocalizationFile();
            
            RadiacPanel.GlobalInit();
            VirtualCamera.Init();
            VirtualCursor.Init();
            RadiacInputController.Init();
        }
        
        void Update()
        {
            RadiacUpdates();
        }
        
        void FixedUpdate()
        {
            RadiacFixedUpdates();
        }
        
        void OnGUI()
        {
            RadiacGUICallback();
        }
    }
}
