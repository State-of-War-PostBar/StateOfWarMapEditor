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
        bool inited;
        
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
        
        EdtInfo curInfo => Global.inst.edt.headerInfo;
        
        /// <summary>
        /// This function should be executed *after* all UI-interactive signals are prepared.
        /// All operations should be executed *after* all UI-interactions are done when a signal is emitted.
        /// </summary>
        void Begin()
        {
            AddCallback(new Signal(signalEdtLoaded), () =>
            {
                if(Global.inst.edt == null) return;
                
                var edt = curInfo;
                
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
            
            AddCallback(new Signal(pMoney.cancelSignal),
                WrapTestEdt(() => curInfo.pMoney = CheckAndModify(pMoney, curInfo.pMoney)));
            AddCallback(new Signal(pResearch.cancelSignal),
                WrapTestEdt(() => curInfo.pResearch = CheckAndModify(pResearch, curInfo.pResearch)));
            AddCallback(new Signal(pDiskPower.cancelSignal),
                WrapTestEdt(() => curInfo.pDiskAttack = CheckAndModify(pDiskPower, curInfo.pDiskAttack)));
            AddCallback(new Signal(pBomber.cancelSignal),
                WrapTestEdt(() => curInfo.pBomber = CheckAndModify(pBomber, curInfo.pBomber)));
            AddCallback(new Signal(pMeteor.cancelSignal),
                WrapTestEdt(() => curInfo.pMeteor = CheckAndModify(pMeteor, curInfo.pMeteor)));
            AddCallback(new Signal(pCarrier.cancelSignal),
                WrapTestEdt(() => curInfo.pCarrier = CheckAndModify(pCarrier, curInfo.pCarrier)));
            AddCallback(new Signal(pTripler.cancelSignal),
                WrapTestEdt(() => curInfo.pTripler = CheckAndModify(pTripler, curInfo.pTripler)));
            AddCallback(new Signal(pFighter.cancelSignal),
                WrapTestEdt(() => curInfo.pFighter = CheckAndModify(pFighter, curInfo.pFighter)));
            
            AddCallback(new Signal(nMoney.cancelSignal),
                WrapTestEdt(() => curInfo.nMoney = CheckAndModify(nMoney, curInfo.nMoney)));
            AddCallback(new Signal(nResearch.cancelSignal),
                WrapTestEdt(() => curInfo.nResearch = CheckAndModify(nResearch, curInfo.nResearch)));
            AddCallback(new Signal(nDiskPower.cancelSignal),
                WrapTestEdt(() => curInfo.nDiskAttack = CheckAndModify(nDiskPower, curInfo.nDiskAttack)));
            AddCallback(new Signal(nBomber.cancelSignal),
                WrapTestEdt(() => curInfo.nBomber = CheckAndModify(nBomber, curInfo.nBomber)));
            AddCallback(new Signal(nMeteor.cancelSignal),
                WrapTestEdt(() => curInfo.nMeteor = CheckAndModify(nMeteor, curInfo.nMeteor)));
            AddCallback(new Signal(nCarrier.cancelSignal),
                WrapTestEdt(() => curInfo.nCarrier = CheckAndModify(nCarrier, curInfo.nCarrier)));
            AddCallback(new Signal(nTripler.cancelSignal),
                WrapTestEdt(() => curInfo.nTripler = CheckAndModify(nTripler, curInfo.nTripler)));
            AddCallback(new Signal(nFighter.cancelSignal),
                WrapTestEdt(() => curInfo.nFighter = CheckAndModify(nFighter, curInfo.nFighter)));
            
            AddCallback(new Signal(timeLimit.cancelSignal),
                WrapTestEdt(() => curInfo.timeLimit = CheckAndModify(timeLimit, curInfo.timeLimit)));
            
            AddCallback(new Signal(pDisk.switchSignal),
                WrapTestEdt(() => curInfo.pDisk = CheckAndModify(pDisk, DiskRebuildType.Enabled, DiskRebuildType.Disabled)));
            AddCallback(new Signal(nDisk.switchSignal),
                WrapTestEdt(() => curInfo.nDisk = CheckAndModify(nDisk, DiskRebuildType.Enabled, DiskRebuildType.Disabled)));
            
            AddCallback(new Signal(pTurretDefence.switchSignal),
                WrapTestEdt(() => curInfo.pTurretDefence = CheckAndModify(pTurretDefence, true, false)));
            AddCallback(new Signal(pTurretAntiair.switchSignal),
                WrapTestEdt(() => curInfo.pTurretAntiair = CheckAndModify(pTurretAntiair, true, false)));
            AddCallback(new Signal(pTurretIon.switchSignal),
                WrapTestEdt(() => curInfo.pTurretIon = CheckAndModify(pTurretIon, true, false)));
            AddCallback(new Signal(pTurretLed.switchSignal),
                WrapTestEdt(() => curInfo.pTurretLed = CheckAndModify(pTurretLed, true, false)));
            AddCallback(new Signal(pTurretCluster.switchSignal),
                WrapTestEdt(() => curInfo.pTurretCluster = CheckAndModify(pTurretCluster, true, false)));
            
            AddCallback(new Signal(nTurretDefence.switchSignal),
                WrapTestEdt(() => curInfo.nTurretDefence = CheckAndModify(nTurretDefence, true, false)));
            AddCallback(new Signal(nTurretAntiair.switchSignal),
                WrapTestEdt(() => curInfo.nTurretAntiair = CheckAndModify(nTurretAntiair, true, false)));
            AddCallback(new Signal(nTurretIon.switchSignal),
                WrapTestEdt(() => curInfo.nTurretIon = CheckAndModify(nTurretIon, true, false)));
            AddCallback(new Signal(nTurretLed.switchSignal),
                WrapTestEdt(() => curInfo.nTurretLed = CheckAndModify(nTurretLed, true, false)));
            AddCallback(new Signal(nTurretCluster.switchSignal),
                WrapTestEdt(() => curInfo.nTurretCluster = CheckAndModify(nTurretCluster, true, false)));
            
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
        
        void CheckTimeState()
        {
            bool vict = timeVictory.selfActive;
            bool fail = timeFail.selfActive;
            bool rnfc = timeReinforce.selfActive;
            if(!vict && !fail && !rnfc)
            {
                curInfo.hasTimeLimit = false;
            }
            else if(vict)
            {
                curInfo.hasTimeLimit = true;
                curInfo.timeLimitType = TimeLimitType.Victory;
            }
            else if(fail)
            {
                curInfo.hasTimeLimit = true;
                curInfo.timeLimitType = TimeLimitType.Fail;
            }
            else if(rnfc)
            {
                curInfo.hasTimeLimit = true;
                curInfo.timeLimitType = TimeLimitType.Reinforcement;
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
            // otherwise this text box will always contains the wrong value...
            text.text = "" + v;
            return v;
        }
    }
}
