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
            foreach(var t in map)
            {
                SpriteRenderer rd = null;
                rd = pool[cur].GetComponent<SpriteRenderer>();
                
                int code = 0;
                code += t.ground == TileGround.Blocked ? 4 : 0;
                code += t.air == TileAir.Blocked ? 2 : 0;
                code += t.turret == TileTurret.Blocked ? 1 : 0;
                rd.sprite = states[code];
                
                var sz = new Vector2(rd.sprite.texture.width, rd.sprite.texture.height);
                rd.transform.position = Vector2.Scale(new Vector2(t.x, -t.y), sz);
                
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
