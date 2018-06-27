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
                if(sel.isBuilding)
                {
                    var s = Global.inst.edt.buildings[sel.id];
                    var lpos = new Vector2(s.x, s.y);
                    var rpos = lpos + Global.inst.buildingSize[s.type];
                    pos = (lpos + rpos) * 0.5f * Global.gridSize;
                }
                else
                {
                    var s = Global.inst.edt.units[sel.id];
                    pos = new Vector2(s.x, s.y);
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
