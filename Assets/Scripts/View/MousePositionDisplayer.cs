using UnityEngine;
using UnityEngine.UI;

namespace MapEditor
{
    [RequireComponent(typeof(Text))]
    public class MousePositionDisplayer : MonoBehaviour
    {
        Text text { get { return this.GetComponent<Text>(); } }
        void Update()
        {
            if(Global.inst.showMousePosition)
                text.text = "[ " + Global.inst.cursorPointingGrid.x + " " + Global.inst.cursorPointingGrid.y + " ]";
            else
                text.text = "";
        }
        
    }
    
}
