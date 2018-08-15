using UnityEngine;
using RadiacUI;
using StateOfWarUtility;
using System;

namespace MapEditor
{
    /// <summary>
    /// The major interface with the global settings and Global setting section of UI.
    /// </summary>
    public sealed class GlobalDataSynchronizer : SignalReceiver
    {
        public string signalEdtLoaded;
        public string signalMapLoaded;
        bool inited;
        
        public RadiacTextInput initViewX;
        public RadiacTextInput initViewY;
        
        public RadiacTextInput pMoney;
        public RadiacTextInput pResearch;
        public RadiacTextInput pDiskPower;
        public RadiacTextInput pBomber;
        public RadiacTextInput pMeteor;
        public RadiacTextInput pCarrier;
        public RadiacTextInput pTripler;
        public RadiacTextInput pFighter;
        
        public RadiacTextInput nMoney;
        public RadiacTextInput nResearch;
        public RadiacTextInput nDiskPower;
        public RadiacTextInput nBomber;
        public RadiacTextInput nMeteor;
        public RadiacTextInput nCarrier;
        public RadiacTextInput nTripler;
        public RadiacTextInput nFighter;
        
        public RadiacTextInput timeLimit;
        
        public RadiacUIComponent pDisk;
        public RadiacUIComponent nDisk;
        
        public RadiacUIComponent pTurretDefence;
        public RadiacUIComponent pTurretAntiair;
        public RadiacUIComponent pTurretIon;
        public RadiacUIComponent pTurretLed;
        public RadiacUIComponent pTurretCluster;
        
        public RadiacUIComponent nTurretDefence;
        public RadiacUIComponent nTurretAntiair;
        public RadiacUIComponent nTurretIon;
        public RadiacUIComponent nTurretLed;
        public RadiacUIComponent nTurretCluster;
        
        public RadiacUIComponent timeVictory;
        public RadiacUIComponent timeFail;
        public RadiacUIComponent timeReinforce;
        
        EdtInfo edtInfo => Global.inst.edt.headerInfo;
        MapInfo mapInfo => Global.inst.map.headerInfo;
        
        /// <summary>
        /// This function should be executed *after* all UI-interactive signals are prepared.
        /// All operations should be executed *after* all UI-interactions are done when a signal is emitted.
        /// </summary>
        void Begin()
        {
            AddCallback(new Signal(signalEdtLoaded), () =>
            {
                if(Global.inst.edt == null) return;
                var edt = edtInfo;
                
                pMoney.text = "" + edt.pMoney;
                pResearch.text = "" + edt.pResearch;
                pDiskPower.text = "" + edt.pDiskAttack;
                pBomber.text = "" + edt.pBomber;
                pMeteor.text = "" + edt.pMeteor;
                pCarrier.text = "" + edt.pCarrier;
                pTripler.text = "" + edt.pTripler;
                pFighter.text = "" + edt.pFighter;
                
                nMoney.text = "" + edt.nMoney;
                nResearch.text = "" + edt.nResearch;
                nDiskPower.text = "" + edt.nDiskAttack;
                nBomber.text = "" + edt.nBomber;
                nMeteor.text = "" + edt.nMeteor;
                nCarrier.text = "" + edt.nCarrier;
                nTripler.text = "" + edt.nTripler;
                nFighter.text = "" + edt.nFighter;
                
                timeLimit.text = "" + edt.timeLimit;
                
                pDisk.selfActive = edt.pDisk == DiskRebuildType.Enabled;
                nDisk.selfActive = edt.nDisk == DiskRebuildType.Enabled;
                
                pTurretDefence.selfActive = edt.pTurretDefence;
                pTurretAntiair.selfActive = edt.pTurretAntiair;
                pTurretIon.selfActive = edt.pTurretIon;
                pTurretLed.selfActive = edt.pTurretLed;
                pTurretCluster.selfActive = edt.pTurretCluster;
                
                nTurretDefence.selfActive = edt.nTurretDefence;
                nTurretAntiair.selfActive = edt.nTurretAntiair;
                nTurretIon.selfActive = edt.nTurretIon;
                nTurretLed.selfActive = edt.nTurretLed;
                nTurretCluster.selfActive = edt.nTurretCluster;
                
                if(!edt.hasTimeLimit)
                {
                    timeVictory.selfActive = timeFail.selfActive = timeReinforce.selfActive = false;
                }
                else
                {
                    switch(edt.timeLimitType)
                    {
                        case TimeLimitType.Victory:
                        {
                            timeVictory.selfActive = true;
                            timeFail.selfActive = false;
                            timeReinforce.selfActive = false;
                            break;
                        }
                        case TimeLimitType.Fail:
                        {
                            timeVictory.selfActive = false;
                            timeFail.selfActive = true;
                            timeReinforce.selfActive = false;
                            break;
                        }
                        case TimeLimitType.Reinforcement:
                        {
                            timeVictory.selfActive = false;
                            timeFail.selfActive = false;
                            timeReinforce.selfActive = true;
                            break;
                        }
                        default: break;
                    }
                }
            });
            
            AddCallback(new Signal(signalMapLoaded), () =>
            {
                if(Global.inst.map == null) return;
                var map = mapInfo;
                initViewX.text = "" + map.initViewX;
                initViewY.text = "" + map.initViewY;
            });
            
            AddCallback(new Signal(initViewX.cancelSignal),
                WrapTestMap(() => mapInfo.initViewX = CheckAndModify(initViewX, mapInfo.initViewX)));
            AddCallback(new Signal(initViewX.cancelSignal),
                WrapTestMap(() => mapInfo.initViewY = CheckAndModify(initViewY, mapInfo.initViewY)));
            
            
            AddCallback(new Signal(pMoney.cancelSignal),
                WrapTestEdt(() => edtInfo.pMoney = CheckAndModify(pMoney, edtInfo.pMoney)));
            AddCallback(new Signal(pResearch.cancelSignal),
                WrapTestEdt(() => edtInfo.pResearch = CheckAndModify(pResearch, edtInfo.pResearch)));
            AddCallback(new Signal(pDiskPower.cancelSignal),
                WrapTestEdt(() => edtInfo.pDiskAttack = CheckAndModify(pDiskPower, edtInfo.pDiskAttack)));
            AddCallback(new Signal(pBomber.cancelSignal),
                WrapTestEdt(() => edtInfo.pBomber = CheckAndModify(pBomber, edtInfo.pBomber)));
            AddCallback(new Signal(pMeteor.cancelSignal),
                WrapTestEdt(() => edtInfo.pMeteor = CheckAndModify(pMeteor, edtInfo.pMeteor)));
            AddCallback(new Signal(pCarrier.cancelSignal),
                WrapTestEdt(() => edtInfo.pCarrier = CheckAndModify(pCarrier, edtInfo.pCarrier)));
            AddCallback(new Signal(pTripler.cancelSignal),
                WrapTestEdt(() => edtInfo.pTripler = CheckAndModify(pTripler, edtInfo.pTripler)));
            AddCallback(new Signal(pFighter.cancelSignal),
                WrapTestEdt(() => edtInfo.pFighter = CheckAndModify(pFighter, edtInfo.pFighter)));
            
            AddCallback(new Signal(nMoney.cancelSignal),
                WrapTestEdt(() => edtInfo.nMoney = CheckAndModify(nMoney, edtInfo.nMoney)));
            AddCallback(new Signal(nResearch.cancelSignal),
                WrapTestEdt(() => edtInfo.nResearch = CheckAndModify(nResearch, edtInfo.nResearch)));
            AddCallback(new Signal(nDiskPower.cancelSignal),
                WrapTestEdt(() => edtInfo.nDiskAttack = CheckAndModify(nDiskPower, edtInfo.nDiskAttack)));
            AddCallback(new Signal(nBomber.cancelSignal),
                WrapTestEdt(() => edtInfo.nBomber = CheckAndModify(nBomber, edtInfo.nBomber)));
            AddCallback(new Signal(nMeteor.cancelSignal),
                WrapTestEdt(() => edtInfo.nMeteor = CheckAndModify(nMeteor, edtInfo.nMeteor)));
            AddCallback(new Signal(nCarrier.cancelSignal),
                WrapTestEdt(() => edtInfo.nCarrier = CheckAndModify(nCarrier, edtInfo.nCarrier)));
            AddCallback(new Signal(nTripler.cancelSignal),
                WrapTestEdt(() => edtInfo.nTripler = CheckAndModify(nTripler, edtInfo.nTripler)));
            AddCallback(new Signal(nFighter.cancelSignal),
                WrapTestEdt(() => edtInfo.nFighter = CheckAndModify(nFighter, edtInfo.nFighter)));
            
            AddCallback(new Signal(timeLimit.cancelSignal),
                WrapTestEdt(() => edtInfo.timeLimit = CheckAndModify(timeLimit, edtInfo.timeLimit)));
            
            AddCallback(new Signal(pDisk.switchSignal),
                WrapTestEdt(() => edtInfo.pDisk = CheckAndModify(pDisk, DiskRebuildType.Enabled, DiskRebuildType.Disabled)));
            AddCallback(new Signal(nDisk.switchSignal),
                WrapTestEdt(() => edtInfo.nDisk = CheckAndModify(nDisk, DiskRebuildType.Enabled, DiskRebuildType.Disabled)));
            
            AddCallback(new Signal(pTurretDefence.switchSignal),
                WrapTestEdt(() => edtInfo.pTurretDefence = CheckAndModify(pTurretDefence, true, false)));
            AddCallback(new Signal(pTurretAntiair.switchSignal),
                WrapTestEdt(() => edtInfo.pTurretAntiair = CheckAndModify(pTurretAntiair, true, false)));
            AddCallback(new Signal(pTurretIon.switchSignal),
                WrapTestEdt(() => edtInfo.pTurretIon = CheckAndModify(pTurretIon, true, false)));
            AddCallback(new Signal(pTurretLed.switchSignal),
                WrapTestEdt(() => edtInfo.pTurretLed = CheckAndModify(pTurretLed, true, false)));
            AddCallback(new Signal(pTurretCluster.switchSignal),
                WrapTestEdt(() => edtInfo.pTurretCluster = CheckAndModify(pTurretCluster, true, false)));
            
            AddCallback(new Signal(nTurretDefence.switchSignal),
                WrapTestEdt(() => edtInfo.nTurretDefence = CheckAndModify(nTurretDefence, true, false)));
            AddCallback(new Signal(nTurretAntiair.switchSignal),
                WrapTestEdt(() => edtInfo.nTurretAntiair = CheckAndModify(nTurretAntiair, true, false)));
            AddCallback(new Signal(nTurretIon.switchSignal),
                WrapTestEdt(() => edtInfo.nTurretIon = CheckAndModify(nTurretIon, true, false)));
            AddCallback(new Signal(nTurretLed.switchSignal),
                WrapTestEdt(() => edtInfo.nTurretLed = CheckAndModify(nTurretLed, true, false)));
            AddCallback(new Signal(nTurretCluster.switchSignal),
                WrapTestEdt(() => edtInfo.nTurretCluster = CheckAndModify(nTurretCluster, true, false)));
            
            AddCallback(new Signal(timeVictory.switchSignal), WrapTestEdt(CheckTimeState));
            AddCallback(new Signal(timeFail.switchSignal), WrapTestEdt(CheckTimeState));
            AddCallback(new Signal(timeReinforce.switchSignal), WrapTestEdt(CheckTimeState));
        }
        
        void Update()
        {
            if(!inited)
            {
                Begin();
                inited = true;
            }
        } 
        
        Action WrapTestEdt(Action v)
        {
            return () =>
            {
                if(Global.inst.edt == null) return;
                v();
            };
        }
        
        Action WrapTestMap(Action v)
        {
            return () =>
            {
                if(Global.inst.map == null) return;
                v();
            };
        }
        
        void CheckTimeState()
        {
            bool vict = timeVictory.selfActive;
            bool fail = timeFail.selfActive;
            bool rnfc = timeReinforce.selfActive;
            if(!vict && !fail && !rnfc)
            {
                edtInfo.hasTimeLimit = false;
            }
            else
            {
                edtInfo.hasTimeLimit = true;
                edtInfo.timeLimitType =
                    vict ? TimeLimitType.Victory
                    : fail ? TimeLimitType.Fail
                    : TimeLimitType.Reinforcement;
            }
        }
        
        bool CheckAllDisabled(params RadiacUIComponent[] cs)
        {
            foreach(var i in cs) if(i.selfActive) return false;
            return true;
        }
        
        N CheckAndModify<N>(RadiacUIComponent toggle, N trueVal, N falseVal)
            => toggle.selfActive ? trueVal : falseVal;
            
        uint CheckAndModify(RadiacTextInput text, uint val)
        {
            uint v = val;
            try { v = uint.Parse(text.text); }
            catch(Exception) { }
            
            // Reset the text to the original value,
            // otherwise this text box will always contains the wrong value without affecting other things...
            text.text = "" + v;
            return v;
        }
    }
}
