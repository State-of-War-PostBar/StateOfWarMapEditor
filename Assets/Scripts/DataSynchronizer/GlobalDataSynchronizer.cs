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
        
        EdtInfo curInfo { get { return Global.inst.edt.headerInfo; } }
        
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
            
            AddCallback(new Signal(pMoney.cancelSignal), WrapTestEdt(WrapTestEdt(() => CheckAndModify(pMoney, ref curInfo.pMoney))));
            AddCallback(new Signal(pResearch.cancelSignal), WrapTestEdt(() => CheckAndModify(pResearch, ref curInfo.pResearch)));
            AddCallback(new Signal(pDiskPower.cancelSignal), WrapTestEdt(() => CheckAndModify(pDiskPower, ref curInfo.pDiskAttack)));
            AddCallback(new Signal(pBomber.cancelSignal), WrapTestEdt(() => CheckAndModify(pBomber, ref curInfo.pBomber)));
            AddCallback(new Signal(pMeteor.cancelSignal), WrapTestEdt(() => CheckAndModify(pMeteor, ref curInfo.pMeteor)));
            AddCallback(new Signal(pCarrier.cancelSignal), WrapTestEdt(() => CheckAndModify(pCarrier, ref curInfo.pCarrier)));
            AddCallback(new Signal(pTripler.cancelSignal), WrapTestEdt(() => CheckAndModify(pTripler, ref curInfo.pTripler)));
            AddCallback(new Signal(pFighter.cancelSignal), WrapTestEdt(() => CheckAndModify(pFighter, ref curInfo.pFighter)));
            
            AddCallback(new Signal(nMoney.cancelSignal), WrapTestEdt(() => CheckAndModify(nMoney, ref curInfo.nMoney)));
            AddCallback(new Signal(nResearch.cancelSignal), WrapTestEdt(() => CheckAndModify(nResearch, ref curInfo.nResearch)));
            AddCallback(new Signal(nDiskPower.cancelSignal), WrapTestEdt(() => CheckAndModify(nDiskPower, ref curInfo.nDiskAttack)));
            AddCallback(new Signal(nBomber.cancelSignal), WrapTestEdt(() => CheckAndModify(nBomber, ref curInfo.nBomber)));
            AddCallback(new Signal(nMeteor.cancelSignal), WrapTestEdt(() => CheckAndModify(nMeteor, ref curInfo.nMeteor)));
            AddCallback(new Signal(nCarrier.cancelSignal), WrapTestEdt(() => CheckAndModify(nCarrier, ref curInfo.nCarrier)));
            AddCallback(new Signal(nTripler.cancelSignal), WrapTestEdt(() => CheckAndModify(nTripler, ref curInfo.nTripler)));
            AddCallback(new Signal(nFighter.cancelSignal), WrapTestEdt(() => CheckAndModify(nFighter, ref curInfo.nFighter)));
            
            AddCallback(new Signal(timeLimit.cancelSignal), WrapTestEdt(() => CheckAndModify(timeLimit, ref curInfo.timeLimit)));
            
            AddCallback(new Signal(pDisk.switchSignal),
                WrapTestEdt(() => CheckAndModify(pDisk, ref curInfo.pDisk, DiskRebuildType.Enabled, DiskRebuildType.Disabled)));
            AddCallback(new Signal(nDisk.switchSignal),
                WrapTestEdt(() => CheckAndModify(nDisk, ref curInfo.nDisk, DiskRebuildType.Enabled, DiskRebuildType.Disabled)));
            
            AddCallback(new Signal(pTurretDefence.switchSignal),
                WrapTestEdt(() => CheckAndModify(pTurretDefence, ref curInfo.pTurretDefence, true, false)));
            AddCallback(new Signal(pTurretAntiair.switchSignal),
                WrapTestEdt(() => CheckAndModify(pTurretAntiair, ref curInfo.pTurretAntiair, true, false)));
            AddCallback(new Signal(pTurretIon.switchSignal),
                WrapTestEdt(() => CheckAndModify(pTurretIon, ref curInfo.pTurretIon, true, false)));
            AddCallback(new Signal(pTurretLed.switchSignal),
                WrapTestEdt(() => CheckAndModify(pTurretLed, ref curInfo.pTurretLed, true, false)));
            AddCallback(new Signal(pTurretCluster.switchSignal),
                WrapTestEdt(() => CheckAndModify(pTurretCluster, ref curInfo.pTurretCluster, true, false)));
            
            AddCallback(new Signal(nTurretDefence.switchSignal),
                WrapTestEdt(() => CheckAndModify(nTurretDefence, ref curInfo.nTurretDefence, true, false)));
            AddCallback(new Signal(nTurretAntiair.switchSignal),
                WrapTestEdt(() => CheckAndModify(nTurretAntiair, ref curInfo.nTurretAntiair, true, false)));
            AddCallback(new Signal(nTurretIon.switchSignal),
                WrapTestEdt(() => CheckAndModify(nTurretIon, ref curInfo.nTurretIon, true, false)));
            AddCallback(new Signal(nTurretLed.switchSignal),
                WrapTestEdt(() => CheckAndModify(nTurretLed, ref curInfo.nTurretLed, true, false)));
            AddCallback(new Signal(nTurretCluster.switchSignal),
                WrapTestEdt(() => CheckAndModify(nTurretCluster, ref curInfo.nTurretCluster, true, false)));
            
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
        
        void CheckAndModify<N>(RadiacUIComponent toggle, ref N val, N trueVal, N falseVal)
        {
            if(toggle.selfActive)
            {
                val = trueVal;
            }
            else
            {
                val = falseVal;
            }
        }
        
        void CheckAndModify(RadiacTextInput text, ref uint val)
        {
            try
            {
                uint v = uint.Parse(text.text);
                val = v;
                return;
            }
            catch(ArgumentNullException) { }
            catch(FormatException) { }
            catch(OverflowException) { }
            
            // Reset the text to the original value,
            // otherwise this text box will always contains the wrong value...
            text.text = "" + val;
        }
    }
}
