using UnityEngine;
using RadiacUI;

namespace MapEditor
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class SelectionPointer : MonoBehaviour
    {
        MeshRenderer rd { get { return this.GetComponent<MeshRenderer>(); } }
        
        Color baseColor;
        
        void Start()
        {
            rd.sortingLayerName = "PointingInfo";
            rd.sortingOrder = 0;
            baseColor = rd.material.GetColor("_Color");
        }
        
        void Update()
        {
            var sel = Global.inst.selection;
            if(sel.selected)
            {
                Vector2 pos;
                if(sel.building != null)
                {
                    var lpos = new Vector2(sel.building.x, sel.building.y);
                    var rpos = lpos + Global.inst.buildingSize[sel.building.type];
                    pos = (lpos + rpos) * 0.5f * Global.gridSize;
                }
                else // if(sel.unit != null)
                {
                    pos = new Vector2(sel.unit.x, sel.unit.y);
                }
                
                pos.y = -pos.y;
                rd.material.SetVector("_CursorPosition", pos);
                rd.material.SetColor("_Color", baseColor);
            }
            else
            {
                rd.material.SetColor("_Color", baseColor.SetA(0f));
            }
        }
    }
}
