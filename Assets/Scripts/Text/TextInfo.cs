using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MapEditor
{
    [RequireComponent(typeof(Text))]
    public sealed class TextInfo : MonoBehaviour
    {
        const uint textUpdateDelay = 3;
        
        /// <summary>
        /// Take account with the localization translation.
        /// </summary>
        public string requirementName;
        
        string text
        {
            get { return this.GetComponent<Text>().text; }
            set { this.GetComponent<Text>().text = value; }
        }
        
        int v = 0;
        void Update()
        {
            v++;
            if(v == textUpdateDelay)
            {
                v = 0;
                text = Global.inst.textAgent[requirementName];
            }
        }
    }
}
