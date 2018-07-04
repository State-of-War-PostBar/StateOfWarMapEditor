using UnityEngine;
using System;
using StateOfWarUtility;

namespace MapEditor
{
    public sealed class TileSynchronizer : MonoBehaviour
    {
        void Update()
        {
            if(Global.inst.map == null) return;
            
            for(int i=0; i<=7; i++) if(Input.GetKey(KeyCode.Alpha0 + i))
            {
                    SetPointingTile(i);
            }
            
            // A special treat.
            if(Input.GetKey(KeyCode.BackQuote))
                SetPointingTile(0);
        }
        
        
        void SetPointingTile(int i)
        {
            var pos = Global.inst.cursorPointingGrid;
            var tile = Global.inst.map[pos.x, pos.y];
            tile.ground = (i & 1) != 0 ? TileGround.Blocked : TileGround.Passed;
            tile.air = (i & 2) != 0 ? TileAir.Blocked : TileAir.Passed;
            tile.turret = (i & 4) != 0 ? TileTurret.Blocked : TileTurret.Passed;    
        }
    }
}
