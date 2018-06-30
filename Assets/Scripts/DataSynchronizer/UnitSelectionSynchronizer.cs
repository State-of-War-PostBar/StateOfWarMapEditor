using UnityEngine;
using RadiacUI;
using StateOfWarUtility;
using System;

namespace MapEditor
{
    public class UnitSelectionSynchronizer : SignalReceiver
    {
        const int updateDelay = 1;
        
        public string emitUnitPanelOn;
        public string emitUnitPanelOff;
        
        public RadiacTextInput x;
        public RadiacTextInput y;
        public RadiacTextInput owner;
        public RadiacTextInput type;
    
        public string reqX;
        public string reqY;
        public string reqOwner;
        public string reqType;
        
        [SerializeField] bool inited = false;
        
        void Start()
        {
            var ta = Global.inst.textAgent;
            ta.Register(reqX);
            ta.Register(reqY);
            ta.Register(reqOwner);
            ta.Register(reqType);
        }
        
        Func<string, string> local = LocalizationSupport.GetLocalizedString;
        BattleUnit curSelection => Global.inst.edt?.units[Global.inst.selection.id];
        
        int t = 0;
        void Update()
        {
            if(!inited)
            {
                InitModifier();
                inited = true;
            }
            
            t++;
            if(t >= updateDelay)
            {
                t -= updateDelay;
                UpdateCurrentSelection();
            }
        }
        
        void UpdateCurrentSelection()
        {
            var sel = Global.inst.selection;
            if(sel.selected && sel.battleUnit != null)
            {
                UpdateView();
                UpdateInput();
                UpdateChangePosition();
                SignalManager.EmitSignal(emitUnitPanelOn);
            }
            else
            {
                SignalManager.EmitSignal(emitUnitPanelOff);
                ClearView();
            }
        }
        
        void UpdateView()
        {
            // TODO:
            // Optimization:
            // Strings to be localized can be cached, not calculated every frame.
            var ta = Global.inst.textAgent;
            var b = curSelection;
            ta.Update(reqX, "" + b.x);
            ta.Update(reqY, "" + b.y);
            ta.Update(reqOwner, local("$" + b.owner + "$"));
            ta.Update(reqType, local("$" + b.type + "$"));
        }
        
        void UpdateInput()
        {
            var b = curSelection;
            x.text = "" + b.x;
            y.text = "" + b.y;
            owner.text = "" + (uint)b.owner;
            type.text = "" + (uint)b.type;
        }
        
        void UpdateChangePosition()
        {
            if(Global.inst.syncPosition)
            {
                curSelection.x = (uint)Global.inst.cursorPointingGrid.x * Global.gridSize + Global.gridSize / 2;
                curSelection.y = (uint)Global.inst.cursorPointingGrid.y * Global.gridSize + Global.gridSize / 2;
            }
        } 
        
        void ClearView()
        {
            var ta = Global.inst.textAgent;
            ta.Update(reqX, "");
            ta.Update(reqY, "");
            ta.Update(reqOwner, "");
            ta.Update(reqType, "");
        }
        
        void InitModifier()
        {
            AddCallback(new Signal(x.cancelSignal), () => curSelection.x = Grab(x, curSelection.x));
            AddCallback(new Signal(y.cancelSignal), () => curSelection.y = Grab(y, curSelection.y));
            AddCallback(new Signal(owner.cancelSignal), () => curSelection.owner = Grab(owner, curSelection.owner));
            AddCallback(new Signal(type.cancelSignal), () => curSelection.type = Grab(type, curSelection.type));
        }
        
        Owner Grab(RadiacTextInput source, Owner back)
        {
            var val = back;
            try
            {
                val = (Owner)uint.Parse(source.text);
                if(!Enum.IsDefined(typeof(Owner), val))
                    return back;
            }
            catch(Exception) { }
            return val;
        }
        
        UnitType Grab(RadiacTextInput source, UnitType back)
        {
            var val = back;
            try
            {
                val = (UnitType)uint.Parse(source.text);
                if(!val.IsBattleUnit())
                    val = back;
            }
            catch(Exception) { }
            return val;
        }
        
        uint Grab(RadiacTextInput source, uint back)
        {
            var val = back;
            try { val = uint.Parse(source.text); }
            catch(Exception) { }
            return val;
        }
    }
}
