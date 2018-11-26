using UnityEngine;
using RadiacUI;
using StateOfWarUtility;
using System;

namespace MapEditor
{
    public sealed class BuildingSelectionSynchronizer : SignalReceiver
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
        Building curSelection => Global.inst.edt?.buildings[Global.inst.selection.id];
        
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
            AddCallback(new Signal(health.cancelSignal), () => curSelection.health = Grab(health, curSelection.health));
            AddCallback(new Signal(x.cancelSignal), () => curSelection.x = Grab(x, curSelection.x));
            AddCallback(new Signal(y.cancelSignal), () => curSelection.y = Grab(y, curSelection.y));
            AddCallback(new Signal(owner.cancelSignal), () => curSelection.owner = Grab(owner, curSelection.owner));
            AddCallback(new Signal(sat.cancelSignal), () => curSelection.satellite = Grab(sat, curSelection.satellite));
            AddCallback(new Signal(upd4.cancelSignal), () => curSelection.upgrade4 = Grab(upd4, curSelection.upgrade4));
            AddCallback(new Signal(upd3.cancelSignal), () => curSelection.upgrade3 = Grab(upd3, curSelection.upgrade3));
            AddCallback(new Signal(upd2.cancelSignal), () => curSelection.upgrade2 = Grab(upd2, curSelection.upgrade2));
            AddCallback(new Signal(upd1.cancelSignal), () => curSelection.upgrade1 = Grab(upd1, curSelection.upgrade1));
            AddCallback(new Signal(upd0.cancelSignal), () => curSelection.upgrade0 = Grab(upd0, curSelection.upgrade0));
            AddCallback(new Signal(prod4.cancelSignal), () => curSelection.production4 = GrabProdType(prod4, curSelection.production4));
            AddCallback(new Signal(prod3.cancelSignal), () => curSelection.production3 = GrabProdType(prod3, curSelection.production3));
            AddCallback(new Signal(prod2.cancelSignal), () => curSelection.production2 = GrabProdType(prod2, curSelection.production2));
            AddCallback(new Signal(prod1.cancelSignal), () => curSelection.production1 = GrabProdType(prod1, curSelection.production1));
            AddCallback(new Signal(prod0.cancelSignal), () => curSelection.production0 = GrabProdType(prod0, curSelection.production0));
            AddCallback(new Signal(level.cancelSignal), () => curSelection.level = Grab(level, curSelection.level));
            AddCallback(new Signal(type.cancelSignal), () => curSelection.type = GrabType(type, curSelection.type));
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
        
        bool Grab(RadiacTextInput source, bool back)
        {
            var val = back;
            try { val = uint.Parse(source.text) != 0; }
            catch(Exception) { }
            return val;
        }
        
        
        UnitType GrabProdType(RadiacTextInput source, UnitType back)
        {
            var val = back;
            try
            {
                val = (UnitType)uint.Parse(source.text);
                var type = curSelection.type;
                
                if(val.IsNothing()) // I can always remove a production.
                {
                    val = UnitType.None;
                }
                else if(type == UnitType.Headquater)
                {
                    if(!val.IsProduction())
                        val = back;
                }
                else if(type == UnitType.Radar)
                {
                    if(!val.IsAirforce())
                        val = back;
                }
                else if(type.IsProductionBuilding())
                {
                    if(!val.IsProduction())
                        val = back;
                }
                else if(type.IsResourcesBuilding())
                {
                    if(!val.IsResources())
                        val = back;
                }
            }
            catch(Exception)
            {
                if(source.text == "")
                {
                    val = UnitType.None;
                }
            }
            return val;
        }
        
        UnitType GrabType(RadiacTextInput source, UnitType back)
        {
            var val = back;
            try
            {
                val = (UnitType)uint.Parse(source.text);
                if(!val.IsBuilding())
                    val = back;
            }
            catch(Exception) { }
            return val;
        }
        
        int Grab(RadiacTextInput source, int back)
        {
            var val = back;
            try { val = int.Parse(source.text); }
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
