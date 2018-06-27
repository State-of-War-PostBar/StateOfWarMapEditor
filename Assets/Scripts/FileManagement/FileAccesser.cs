using UnityEngine;
using UnityEngine.UI;
using RadiacUI;

namespace MapEditor
{
    public abstract class FileAccesser : RadiacBypass
    {
        public Text text = null;
        
        protected virtual string textRequest { get { return "EdtFileName"; } }
        protected virtual string notFound { get { return "$EdtNotFound$"; } }
        protected virtual string readHint { get { return "$EdtReadHint$"; } }
        
        public string recentFilePath;
        public string emitFileLoad;
        
        protected override void Start()
        {
            base.Start();
            Global.inst.textAgent.Register(textRequest);
            Global.inst.textAgent.Update(textRequest, LocalizationSupport.GetLocalizedString(readHint));
        }
        
        protected override void SignalBypass()
        {
            base.SignalBypass();
            
            if(recentFilePath == null || recentFilePath != text.text)
            {
                if(LoadNewFile(text.text))
                {
                    recentFilePath = text.text;
                    SignalManager.EmitSignal(new Signal(emitFileLoad));
                }
                else
                {
                    Global.inst.textAgent.Update(textRequest, LocalizationSupport.GetLocalizedString(notFound));
                }
            }
        }
        
        /// <returns>Whether the file is valid.</returns>
        protected virtual bool LoadNewFile(string path)
        {
            // do nothing recently.
            
            return true;
        }
    }
    
}
