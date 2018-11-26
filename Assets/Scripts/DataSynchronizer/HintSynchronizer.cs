using UnityEngine;
using RadiacUI;

namespace MapEditor
{
    public class HintSynchronizer : MonoBehaviour
    {
        void Start()
        {
            {
                var file = Resources.Load("CodexBuildings") as TextAsset;
                Global.inst.textAgent.Register("BuildingCodex", LocalizationSupport.GetLocalizedString(file.text));
            }
            
            {
                var file = Resources.Load("CodexUnits") as TextAsset;
                Global.inst.textAgent.Register("UnitCodex", LocalizationSupport.GetLocalizedString(file.text));
            }
            
            {
                var file = Resources.Load("CodexShortcut") as TextAsset;
                Global.inst.textAgent.Register("ShortcutCodex", LocalizationSupport.GetLocalizedString(file.text));
            }
        }
        
    }
    
}
