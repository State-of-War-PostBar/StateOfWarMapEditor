using System;
using UnityEngine;

using System.Collections.Generic;

using StateOfWarUtility;

namespace MapEditor
{
    public sealed class EdtDisplayer : MonoBehaviour
    {
        readonly Vector2 gridSize = new Vector2(32, 32);
        
        [SerializeField] Sprite[] sprites;
        
        readonly List<SpriteRenderer> pool = new List<SpriteRenderer>();
        
        readonly Dictionary<BuildingType, Sprite> buildingPlayerSprites = new Dictionary<BuildingType, Sprite>();
        readonly Dictionary<BuildingType, Sprite> buildingEnemySprites = new Dictionary<BuildingType, Sprite>();
        readonly Dictionary<BuildingType, Sprite> buildingNeutralSprites = new Dictionary<BuildingType, Sprite>();
        
        readonly Dictionary<BuildingType, Vector2> buildingOffsets = new Dictionary<BuildingType, Vector2>();
        
        readonly Dictionary<UnitType, Sprite> unitPlayerSprites = new Dictionary<UnitType, Sprite>();
        readonly Dictionary<UnitType, Sprite> unitEnemySprites = new Dictionary<UnitType, Sprite>();
        readonly Dictionary<UnitType, Sprite> unitNeutralSprite = new Dictionary<UnitType, Sprite>();
        
        readonly Dictionary<UnitType, Vector2> unitOffsets = new Dictionary<UnitType, Vector2>();
        
        const int preserve = 300;
        
        void Start()
        {
            Preserve(preserve);
            PrepareResources();
        }
        
        void Update()
        {
            if(Global.inst.edt != null)
                Refresh();
        }
        
        
        void PrepareResources()
        {
            var buildingText = Resources.Load("BuildingOffset") as TextAsset;
            var offsetBuildings = INIParser.Parse(buildingText.text);
            foreach(var i in offsetBuildings)
            {
                string[] offsetStr = i.Value.Split(',');
                float x = float.Parse(offsetStr[0]);
                float y = float.Parse(offsetStr[1]);
                buildingOffsets[i.Key.ToEnum<BuildingType>()] = new Vector2(x, y);
            }
            Resources.UnloadAsset(buildingText);
            
            
            var unitText = Resources.Load("UnitOffset") as TextAsset;
            var offsetUnits = INIParser.Parse(unitText.text);
            foreach(var i in offsetUnits)
            {
                string[] offsetStr = i.Value.Split(',');
                float x = float.Parse(offsetStr[0]);
                float y = float.Parse(offsetStr[1]);
                unitOffsets[i.Key.ToEnum<UnitType>()] = new Vector2(x, y);
            }
            Resources.UnloadAsset(unitText);
            
            
            
            // Hard coded file name and configuration file syntax.
            Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
            foreach(var i in this.sprites)
                sprites.Add(i.name, i);
            
            
            foreach(BuildingType bt in Enum.GetValues(typeof(BuildingType)))
                buildingPlayerSprites.Add(bt, sprites["P-" + bt.ToString()]);
            
            
            foreach(BuildingType bt in Enum.GetValues(typeof(BuildingType)))
                buildingEnemySprites.Add(bt, sprites["E-" + bt.ToString()]);
            
            foreach(BuildingType bt in Enum.GetValues(typeof(BuildingType)))
            {
                string n = "N-" + bt.ToString();
                if(sprites.ContainsKey(n))
                    buildingNeutralSprites.Add(bt, sprites[n]);
                else
                    buildingNeutralSprites.Add(bt, buildingEnemySprites[bt]);
            }
            
            
            foreach(UnitType bt in Enum.GetValues(typeof(UnitType)))
                if(!bt.IsProductionOnly())
                    unitPlayerSprites.Add(bt, sprites["P-" + bt.ToString()]);
                
                
            foreach(UnitType bt in Enum.GetValues(typeof(UnitType)))
                if(!bt.IsProductionOnly())
                    unitEnemySprites.Add(bt, sprites["E-" + bt.ToString()]);
            
            
            foreach(UnitType bt in Enum.GetValues(typeof(UnitType)))
            {
                if(!bt.IsProductionOnly())
                {
                    string n = "N-" + bt.ToString();
                    if(sprites.ContainsKey(n))
                        unitNeutralSprite.Add(bt, sprites[n]);
                    else
                        unitNeutralSprite.Add(bt, unitEnemySprites[bt]);
                }
            }
        }
        
        void Refresh()
        {
            var edt = Global.inst.edt;
            Preserve(edt.buildings.count + edt.units.count);
            
            int cur = 0;
            
            foreach(var b in edt.buildings)
            {
                var rd = pool[cur];
                rd.gameObject.SetActive(true);
                switch(b.owner)
                {
                    case Owner.Player:
                    {
                        rd.sprite = buildingPlayerSprites[b.type];
                        break;
                    }
                    case Owner.Enemy:
                    {
                        rd.sprite = buildingEnemySprites[b.type];
                        break;
                    }
                    case Owner.Neutral:
                    default:
                    {
                        rd.sprite = buildingNeutralSprites[b.type];
                        break;
                    }
                }
                rd.gameObject.transform.position = Vector2.Scale(
                    new Vector2(b.x, -b.y) - new Vector2(buildingOffsets[b.type].x, -buildingOffsets[b.type].y),
                    gridSize);
                    
                rd.sortingOrder = - cur + (int)(b.x * 100 + b.y);
                cur++;
            }
            
            foreach(var b in edt.units)
            {
                var rd = pool[cur];
                rd.gameObject.SetActive(true);
                switch(b.owner)
                {
                    case Owner.Player:
                    {
                        rd.sprite = unitPlayerSprites[b.type];
                        break;
                    }
                    case Owner.Enemy:
                    {
                        rd.sprite = unitEnemySprites[b.type];
                        break;
                    }
                    case Owner.Neutral:
                    default:
                    {
                        rd.sprite = unitNeutralSprite[b.type];
                        break;
                    }
                }
                rd.gameObject.transform.position = 
                    new Vector2(b.x - 16f, -b.y + 16f)
                    - Vector2.Scale(new Vector2(unitOffsets[b.type].x, -unitOffsets[b.type].y), gridSize);
                rd.sortingOrder = -cur + (b.type == UnitType.Disk ? 32768 : 0);
                cur++;
            }
            
            while(cur < pool.Count)
            {
                pool[cur].gameObject.SetActive(false);
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
                rd.sortingLayerName = "Units";
                pool.Add(rd);
                x.SetActive(false);
            }
        }
    }
}
