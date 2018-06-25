using UnityEngine;
using RadiacUI;

namespace MapEditor
{
    [RequireComponent(typeof(MeshRenderer))]
    public sealed class GridPointer : MonoBehaviour
    {
        [SerializeField] RadiacPanel panel;
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
            Vector2 pos = Global.inst.cursorPointingGrid;
            rd.material.SetVector("_CursorPosition", pos * 32f + new Vector2(16f, 16f));
            
            if(Global.inst.showGridPointer)
            {
                rd.material.SetColor("_Color", baseColor);
            }
            else
            {
                rd.material.SetColor("_Color", baseColor.SetA(0f));
            }
        }
        
        
    }
}
