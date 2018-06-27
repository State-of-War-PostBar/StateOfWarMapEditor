using UnityEngine;
using RadiacUI;
using StateOfWarUtility;


namespace MapEditor
{
    public sealed class SelectionController : SignalReceiver
    {
        public string signalSelect;
        
        public string emitSelectBuilding;
        public string emitSelectUnit;
        
        struct SelectionInfo
        {
            public int id;
            public bool isUnit;
        }
        
        void Start()
        {
            AddCallback(new Signal(signalSelect), () =>
            {
                if(Global.inst.edt == null) return; // Do nothing...
                
                var pos = Global.inst.cursorPointingGrid;
                var sel = Global.inst.selection;
                var edt = Global.inst.edt;
                
                int su = -1;
                int sb = -1;
                
                if(sel.selected)
                {
                    var b = new SelectionInfo() { id = sel.id, isUnit = sel.isUnit };
                    
                    if(b.isUnit)
                    {
                        su = TrySelectUnit(edt, pos, b.id + 1, edt.units.count - 1);
                        if(su == -1)
                            su = TrySelectUnit(edt, pos, 0, b.id - 1);
                        if(su == -1)
                            sb = TrySelectBuilding(edt, pos, 0, edt.buildings.count - 1);
                    }
                    else
                    {
                        sb = TrySelectBuilding(edt, pos, b.id + 1, edt.buildings.count - 1);
                        if(sb == -1)
                            su = TrySelectUnit(edt, pos, 0, edt.units.count - 1);
                        if(sb == -1 && su == -1)
                            sb = TrySelectBuilding(edt, pos, 0, b.id - 1);
                    }
                }
                else
                {
                    sb = TrySelectBuilding(edt, pos, 0, edt.buildings.count - 1);
                    if(sb == -1)
                        su = TrySelectUnit(edt, pos, 0, edt.units.count - 1);
                }
                
                if(sb != -1)
                {
                    // +Debug.LogFormat("Select building {0} {1} {2}", edt.buildings[sb].type, edt.buildings[sb].x, edt.buildings[sb].y);
                    sel.id = sb;
                    sel.isBuilding = true;
                    sel.selected = true;
                    SignalManager.EmitSignal(emitSelectBuilding);
                }
                else if(su != -1)
                {
                    // Debug.LogFormat("Select unit {0} {1} {2}", edt.units[su].type, edt.units[su].x, edt.units[su].y);
                    sel.id = su;
                    sel.isUnit = true;
                    sel.selected = true;
                    SignalManager.EmitSignal(emitSelectUnit);
                }
                else
                {
                    // Debug.LogFormat("Nothing selected.");
                    sel.id = -1;
                    sel.isUnit = true;
                    sel.selected = false;
                }    
            });
        }
        
        void Update()
        {
            
        }
        
        int TrySelectUnit(Edt edt, Vector2Int pos, int from, int to)
        {
            for(int i = from; i <= to; i++)
            {
                if(CanBeSelect(pos, edt.units[i]))
                    return i;
            }
            return -1;
        }
        
        int TrySelectBuilding(Edt edt, Vector2Int pos, int from, int to)
        {
            for(int i = from; i <= to; i++)
            {
                if(CanBeSelect(pos, edt.buildings[i]))
                    return i;
            }
            return -1;
        }
        
        bool CanBeSelect(Vector2Int pos, Building b)
        {
            var sz = Global.inst.buildingSize[b.type];
            return b.x <= pos.x && pos.x < b.x + sz.x
                && b.y <= pos.y && pos.y < b.y + sz.y;
        }
        
        bool CanBeSelect(Vector2Int position, Unit u)
        {
            var pos = (position + new Vector2(0.5f, 0.5f)) * Global.gridSize;
            var top = pos + new Vector2(Global.gridSize, Global.gridSize);
            var bottom = pos + new Vector2(-Global.gridSize, -Global.gridSize);
            return bottom.x < u.x && u.x < top.x
                && bottom.y < u.y && u.y < top.y;
        }
        
    }
    
}
