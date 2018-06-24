using UnityEngine;
using System;
using System.Collections.Generic;

using StateOfWarUtility;

namespace MapEditor
{
    public sealed class MapDisplayer : MonoBehaviour
    {
        /// <summary>
        /// states:
        ///     Ground Air Turret
        /// 0:     o    o    o
        /// 1:     o    o    x
        /// 2:     o    x    o
        /// 3:     o    x    x
        /// 4:     x    o    o
        /// 5:     x    o    x
        /// 6:     x    x    o
        /// 7:     x    x    x
        /// </summary>
        [SerializeField] Sprite[] states;
        
        readonly List<SpriteRenderer> pool = new List<SpriteRenderer>();
        const int preserve = 8000;
        
        void Start()
        {
            if(states.Length != 8)
            {
                Debug.LogWarning("Not setting the states correctly!");
            }
        
            Preserve(preserve);
        }
        
        
        void Update()
        {
            if(Global.inst.map != null)
                Refresh();
        }
        
        void Refresh()
        {
            var map = Global.inst.map;
            Preserve(map.width * map.height);
            int cur = 0;
            // Optimize:
            // Do not use iterator. The MoveNext() function of it may causes bad performance.
            for(int x=0; x<map.headerInfo.width; x++) for(int y=0; y<map.headerInfo.height; y++)
            {
                var rd = pool[cur];
                
                int code = 0;
                var t = map[x, y];
                code += t.ground == TileGround.Blocked ? 4 : 0;
                code += t.air == TileAir.Blocked ? 2 : 0;
                code += t.turret == TileTurret.Blocked ? 1 : 0;
                
                // Optimize:
                // The sprite assign has some performance cost...
                if(rd.sprite != states[code])
                {
                    rd.sprite = states[code];
                }
                
                rd.transform.position = new Vector2(rd.sprite.texture.width * t.x, -t.y * rd.sprite.texture.height);
                
                rd.gameObject.SetActive(true);
                
                cur++;
            }
        }
        
        
        void Preserve(int cnt)
        {
            while(cnt > pool.Count)
            {
                var x = new GameObject();
                x.transform.SetParent(this.transform);
                var rd = x.AddComponent<SpriteRenderer>();
                rd.sortingLayerName = "Tiles";
                pool.Add(rd);
                x.SetActive(false);
            }
        }
        
        
    }
    
    
}
