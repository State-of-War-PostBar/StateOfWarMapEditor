using UnityEngine;
using RadiacUI;
using StateOfWarUtility;
using System;

namespace MapEditor
{
    public sealed class PositionSynchronizer : SignalReceiver
    {
        const int updateDelay = 1;
        
        public string signalAfterSelect;
        
        Building curSelection => Global.inst.edt?.buildings[Global.inst.selection.id];
        
        [SerializeField] bool isMoving = false;
        [SerializeField] Vector2Int targetOriginalPos;
        [SerializeField] Vector2 cursorOriginalPos;
        [SerializeField] Vector2Int cursorOriginalGrid;
        
        Selection sel => Global.inst.selection;
        
        void Start()
        {
            AddCallback(new Signal(signalAfterSelect), () =>
            {
                if(!isMoving && Input.GetKeyDown(KeyCode.Mouse0))
                {
                    isMoving = true;
                    SetupMoving();
                }
            });
        }
        
        int t = 0;
        void Update()
        {
            if(!Input.GetKey(KeyCode.Mouse0))
            {
                isMoving = false;
            }
            
            t++;
            if(t >= updateDelay)
            {
                t -= updateDelay;
                UpdateSelectionPosition();
            }
        }
        
        void SetupMoving()
        {
            if(!sel.selected) return;
            Unit unit = (Unit)sel.building ?? sel.battleUnit;
            if(unit == null) return;
            targetOriginalPos = new Vector2Int(unit.x, unit.y);
            cursorOriginalPos = Global.inst.cursorPointing;
            cursorOriginalGrid = Global.inst.cursorPointingGrid;
        }
        
        void UpdateSelectionPosition()
        {
            if(!sel.selected) return;
            if(!isMoving) return;
            if(sel.building != null)
            {
                UpdateBuilding(sel.building);
            }
            else if(sel.battleUnit != null)
            {
                UpdateBattleUnit(sel.battleUnit);
            }
        }
        
        void UpdateBuilding(Unit unit)
        {
            var delta = Global.inst.cursorPointingGrid - cursorOriginalGrid;
            unit.x = delta.x + targetOriginalPos.x;
            unit.y = delta.y + targetOriginalPos.y;
        }
        
        void UpdateBattleUnit(Unit unit)
        {
            var delta = Global.inst.cursorPointing - cursorOriginalPos;
            var cldelta = new Vector2Int((int)delta.x, (int)delta.y); // integerize forwards 0.
            unit.x = cldelta.x + targetOriginalPos.x;
            unit.y = cldelta.y + targetOriginalPos.y;
            // Pressing shift to align with grids.
            if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                var sz = Global.gridSize;
                unit.x = (unit.x - sz / 2) / sz * sz + sz / 2;
                unit.y = (unit.y - sz / 2) / sz * sz + sz / 2;
            }
        }
        
    }
}
