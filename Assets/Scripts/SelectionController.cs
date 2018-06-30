using UnityEngine;
using RadiacUI;
using StateOfWarUtility;


namespace MapEditor
{
    public sealed class SelectionController : SignalReceiver
    {
        public string signalSelect;
        
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
                    if(sel.battleUnit != null)
                    {
                        su = TrySelectUnit(edt, pos, sel.id + 1, edt.units.count - 1);
                        if(su == -1)
                            su = TrySelectUnit(edt, pos, 0, sel.id - 1);
                        if(su == -1)
                            sb = TrySelectBuilding(edt, pos, 0, edt.buildings.count - 1);
                    }
                    else
                    {
                        sb = TrySelectBuilding(edt, pos, sel.id + 1, edt.buildings.count - 1);
                        if(sb == -1)
                            su = TrySelectUnit(edt, pos, 0, edt.units.count - 1);
                        if(sb == -1 && su == -1)
                            sb = TrySelectBuilding(edt, pos, 0, sel.id - 1);
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
                    sel.SetBuilding(sb);
                }
                else if(su != -1)
                {
                    sel.SetUnit(su);
                }
                else
                {
                    sel.Reset();
                }
            });
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
        
        bool CanBeSelect(Vector2Int position, BattleUnit u)
        {
            var pos = (position + new Vector2(0.5f, 0.5f)) * Global.gridSize;
            var top = pos + new Vector2(Global.gridSize, Global.gridSize);
            var bottom = pos + new Vector2(-Global.gridSize, -Global.gridSize);
            return bottom.x < u.x && u.x < top.x
                && bottom.y < u.y && u.y < top.y;
        }
        
    }
    
}
