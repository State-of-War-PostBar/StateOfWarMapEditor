using UnityEngine;
using UnityEngine.UI;

namespace  RadiacUI
{
    [RequireComponent(typeof(Text))]
    public class RadiacLabel : RadiacUIComponent
    {
        public string requestString;
        
        Text text { get { return this.gameObject.GetComponent<Text>(); } }
        
        protected override void Start()
        {
            base.Start();
            
            if(requestString == "")
            {
                requestString = text.text;
            }
        }
        
        void SyncText()
        {
            text.text = LocalizationSupport.GetLocalizedString(requestString);
        }
        
        protected override void Update()
        {
            base.Update();
            
            // TODO...
            // Is it a good idea to get string every frame?
            SyncText();
        }
    }
}
