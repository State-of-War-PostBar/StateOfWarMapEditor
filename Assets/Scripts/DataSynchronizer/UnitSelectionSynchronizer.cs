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
        Unit curSelection { get { return Global.inst.edt?.units[Global.inst.selection.id]; } }
        
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
            if(sel.selected && sel.unit != null)
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
            AddCallback(new Signal(x.cancelSignal), () => { GrabValueFromText(x, ref curSelection.x); });
            AddCallback(new Signal(y.cancelSignal), () => { GrabValueFromText(y, ref curSelection.y); });
            AddCallback(new Signal(owner.cancelSignal), () => { GrabValueFromText(owner, ref curSelection.owner); });
            AddCallback(new Signal(type.cancelSignal), () => { GrabValueFromText(type, ref curSelection.type); });
        }
        
        void GrabValueFromText(RadiacTextInput source, ref Owner target)
        {
            try
            {
                uint val = uint.Parse(source.text);
                if(!Enum.IsDefined(typeof(Owner), val))
                    return;
                target = (Owner)val;
                UpdateView();
            }
            catch(Exception) { }
        }
        
        void GrabValueFromText(RadiacTextInput source, ref UnitType target)
        {
            try
            {
                uint val = uint.Parse(source.text);
                if(!Enum.IsDefined(typeof(UnitType), val))
                    return;
                target = (UnitType)val;
                UpdateView();
            }
            catch(Exception) { }
        }
        
        void GrabValueFromText(RadiacTextInput source, ref uint target)
        {
            try
            {
                uint val = uint.Parse(source.text);
                target = val;
                UpdateView();
            }
            catch(Exception) { }
        }
        
        Action WrapTestSelection(Action t)
        {
            return () =>
            {
                if(curSelection == null) return;
                t();
            };
        }
    }
}
