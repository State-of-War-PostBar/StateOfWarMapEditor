using UnityEngine;
using RadiacUI;
using StateOfWarUtility;
using System;

namespace MapEditor
{
    public class BuildingSelectionSynchronizer : SignalReceiver
    {
        const int updateDelay = 1;
        
        public string emitBuildingPanelOn;
        public string emitBuildingPanelOff;
        
        public RadiacTextInput health;
        public RadiacTextInput x;
        public RadiacTextInput y;
        public RadiacTextInput owner;
        public RadiacTextInput sat;
        public RadiacTextInput upd4;
        public RadiacTextInput upd3;
        public RadiacTextInput upd2;
        public RadiacTextInput upd1;
        public RadiacTextInput upd0;
        public RadiacTextInput prod4;
        public RadiacTextInput prod3;
        public RadiacTextInput prod2;
        public RadiacTextInput prod1;
        public RadiacTextInput prod0;
        public RadiacTextInput level;
        public RadiacTextInput type;
    
        public string reqHealth;
        public string reqX;
        public string reqY;
        public string reqSat;
        public string reqOwner;
        public string reqUpd4;
        public string reqUpd3;
        public string reqUpd2;
        public string reqUpd1;
        public string reqUpd0;
        public string reqProd4;
        public string reqProd3;
        public string reqProd2;
        public string reqProd1;
        public string reqProd0;
        public string reqLevel;
        public string reqType;
        
        [SerializeField] bool inited = false;
        
        void Start()
        {
            var ta = Global.inst.textAgent;
            ta.Register(reqHealth);
            ta.Register(reqX);
            ta.Register(reqY);
            ta.Register(reqSat);
            ta.Register(reqOwner);
            ta.Register(reqUpd4);
            ta.Register(reqUpd3);
            ta.Register(reqUpd2);
            ta.Register(reqUpd1);
            ta.Register(reqUpd0);
            ta.Register(reqProd4);
            ta.Register(reqProd3);
            ta.Register(reqProd2);
            ta.Register(reqProd1);
            ta.Register(reqProd0);
            ta.Register(reqLevel);
            ta.Register(reqType);
        }
        
        Func<string, string> local = LocalizationSupport.GetLocalizedString;
        Building curSelection { get { return Global.inst.edt?.buildings[Global.inst.selection.id]; } }
        
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
            if(sel.selected && sel.building != null)
            {
                UpdateView();
                UpdateInput();
                UpdateChangePosition();
                SignalManager.EmitSignal(emitBuildingPanelOn);
            }
            else
            {
                SignalManager.EmitSignal(emitBuildingPanelOff);
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
            ta.Update(reqHealth, "" + b.health);
            ta.Update(reqX, "" + b.x);
            ta.Update(reqY, "" + b.y);
            ta.Update(reqSat, local("$" + b.satellite + "$"));
            ta.Update(reqOwner, local("$" + b.owner + "$"));
            ta.Update(reqUpd4, "" + b.upgrade4);
            ta.Update(reqUpd3, "" + b.upgrade3);
            ta.Update(reqUpd2, "" + b.upgrade2);
            ta.Update(reqUpd1, "" + b.upgrade1);
            ta.Update(reqUpd0,"" + b.upgrade0);
            ta.Update(reqProd4, local("$" + b.production4 + "$"));
            ta.Update(reqProd3, local("$" + b.production3 + "$"));
            ta.Update(reqProd2, local("$" + b.production2 + "$"));
            ta.Update(reqProd1, local("$" + b.production1 + "$"));
            ta.Update(reqProd0, local("$" + b.production0 + "$"));
            ta.Update(reqLevel, "" + b.level);
            ta.Update(reqType, local("$" + b.type + "$"));
        }
        
        void UpdateInput()
        {
            var b = curSelection;
            health.text = "" + b.health;
            x.text = "" + b.x;
            y.text = "" + b.y;
            sat.text = "" + (b.satellite ? 1 : 0);
            owner.text = "" + (uint)b.owner;
            upd4.text = "" + b.upgrade4;
            upd3.text = "" + b.upgrade3;
            upd2.text = "" + b.upgrade2;
            upd1.text = "" + b.upgrade1;
            upd0.text = "" + b.upgrade0;
            prod4.text = "" + (uint)b.production4;
            prod3.text = "" + (uint)b.production3;
            prod2.text = "" + (uint)b.production2;
            prod1.text = "" + (uint)b.production1;
            prod0.text = "" + (uint)b.production0;
            level.text = "" + b.level;
            type.text = "" + (uint)b.type;
        }
        
        void UpdateChangePosition()
        {
            if(Global.inst.syncPosition)
            {
                curSelection.x = (uint)Global.inst.cursorPointingGrid.x;
                curSelection.y = (uint)Global.inst.cursorPointingGrid.y;
            }
        } 
        
        void ClearView()
        {
            var ta = Global.inst.textAgent;
            ta.Update(reqHealth, "");
            ta.Update(reqX, "");
            ta.Update(reqY, "");
            ta.Update(reqSat, "");
            ta.Update(reqOwner, "");
            ta.Update(reqUpd4, "");
            ta.Update(reqUpd3, "");
            ta.Update(reqUpd2, "");
            ta.Update(reqUpd1, "");
            ta.Update(reqUpd0,"");
            ta.Update(reqProd4, "");
            ta.Update(reqProd3, "");
            ta.Update(reqProd2, "");
            ta.Update(reqProd1, "");
            ta.Update(reqProd0, "");
            ta.Update(reqLevel, "");
            ta.Update(reqType, "");
        }
        
        void InitModifier()
        {
            AddCallback(new Signal(health.cancelSignal), () => { GrabValueFromText(health, ref curSelection.health); });
            AddCallback(new Signal(x.cancelSignal), () => { GrabValueFromText(x, ref curSelection.x); });
            AddCallback(new Signal(y.cancelSignal), () => { GrabValueFromText(y, ref curSelection.y); });
            AddCallback(new Signal(owner.cancelSignal), () => { GrabValueFromText(owner, ref curSelection.owner); });
            AddCallback(new Signal(sat.cancelSignal), () => { GrabValueFromText(sat, ref curSelection.satellite); });
            AddCallback(new Signal(upd4.cancelSignal), () => { GrabValueFromText(upd4, ref curSelection.upgrade4); });
            AddCallback(new Signal(upd3.cancelSignal), () => { GrabValueFromText(upd3, ref curSelection.upgrade3); });
            AddCallback(new Signal(upd2.cancelSignal), () => { GrabValueFromText(upd2, ref curSelection.upgrade2); });
            AddCallback(new Signal(upd1.cancelSignal), () => { GrabValueFromText(upd1, ref curSelection.upgrade1); });
            AddCallback(new Signal(upd0.cancelSignal), () => { GrabValueFromText(upd0, ref curSelection.upgrade0); });
            AddCallback(new Signal(prod4.cancelSignal), () => { GrabValueFromText(prod4, ref curSelection.production4); });
            AddCallback(new Signal(prod3.cancelSignal), () => { GrabValueFromText(prod3, ref curSelection.production3); });
            AddCallback(new Signal(prod2.cancelSignal), () => { GrabValueFromText(prod2, ref curSelection.production2); });
            AddCallback(new Signal(prod1.cancelSignal), () => { GrabValueFromText(prod1, ref curSelection.production1); });
            AddCallback(new Signal(prod0.cancelSignal), () => { GrabValueFromText(prod0, ref curSelection.production0); });
            AddCallback(new Signal(level.cancelSignal), () => { GrabValueFromText(level, ref curSelection.level); });
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
        
        void GrabValueFromText(RadiacTextInput source, ref bool target)
        {
            try
            {
                uint val = uint.Parse(source.text);
                target = val != 0;
                UpdateView();
            }
            catch(Exception) { }
        }
        
        void GrabValueFromText(RadiacTextInput source, ref BuildingType target)
        {
            try
            {
                uint val = uint.Parse(source.text);
                if(!Enum.IsDefined(typeof(BuildingType), val))
                    return;
                target = (BuildingType)val;
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
