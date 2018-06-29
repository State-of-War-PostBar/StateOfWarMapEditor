using UnityEngine;
using System;
using System.Collections.Generic;
using StateOfWarUtility;

namespace MapEditor
{
    [Serializable]
    public class Selection
    {
        bool isUnit { get; set; }
        
        public Building building
        {
            get
            {
                if(!selected || isUnit) return null;
                return Global.inst.edt.buildings[id];
            }
        }
        
        public Unit unit
        {
            get
            {
                if(!selected || !isUnit) return null;
                return Global.inst.edt.units[id];
            }
        }
        
        public void SetBuilding(int i)
        {
            isUnit = false;
            id = i;
            selected =true;
        }
        
        public void SetUnit(int i)
        {
            isUnit = true;
            id = i;
            selected = true;
        }
        
        public void Reset()
        {
            isUnit = true;
            id = -1;
            selected = false;
        }
        
        // The index of the Unit/Building that is selected.
        // Used with the edt file.
        public int id;
        
        public bool selected;
        
        public Selection()
        {
            Reset();
        }
        
        public static implicit operator bool(Selection v) { return v.selected; }
    }
    
    /// <summary>
    /// Where global resources are used.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class Global : MonoBehaviour
    {
        public static Global inst;
        
        // Pixels per grid.
        // Assume it is a rectangle.
        // Shouldn't be changed forever...
        public const int gridSize = 32;
        
        public TextAgent textAgent = new TextAgent();
        
        public EdtFileAccesser edtAccesser;
        public Edt edt;
        public string edtName;
        
        public MapFileAccesser mapAccesser;
        public Map map;
        public string mapName;
        
        public MapFileAccesser srfAccesser;
        public Texture2D srf;
        public string srfName;
        
        public bool showTiles;
        public bool showGridPointer;
        public bool showDecoration;
        public bool showMousePosition;
        public bool syncPosition;
        
        public object clipBoard;
        
        public readonly Selection selection = new Selection();
        
        public readonly Dictionary<BuildingType, Vector2Int> buildingOffsets = new Dictionary<BuildingType, Vector2Int>();
        public readonly Dictionary<UnitType, Vector2Int> unitOffsets = new Dictionary<UnitType, Vector2Int>();
        public readonly Dictionary<BuildingType, Vector2Int> buildingSize = new Dictionary<BuildingType, Vector2Int>();
        
        void Awake()
        {
            if(inst != null)
                throw new InvalidOperationException("Re-init global object!");
            inst = this;
            
            showGridPointer = true;
            
            PrepareResources();
        }
        
        
        // ============================================================================================================
        // ============================================================================================================
        // ============================================================================================================
        
        
        void PrepareResources()
        {
            SetVec2FromConfig(buildingOffsets, "BuildingOffset");
            SetVec2FromConfig(unitOffsets, "UnitOffset");
            SetVec2FromConfig(buildingSize, "BuildingSize");
        }
        
        void SetVec2FromConfig(Dictionary<BuildingType, Vector2Int> target, string configFile)
        {
            var text = Resources.Load(configFile) as TextAsset;
            var values = INIParser.Parse(text.text);
            foreach(var i in values)
            {
                string[] vstr = i.Value.Split(',');
                int x = int.Parse(vstr[0]);
                int y = int.Parse(vstr[1]);
                target[i.Key.ToEnum<BuildingType>()] = new Vector2Int(x, y);
            }
        }
        
        void SetVec2FromConfig(Dictionary<UnitType, Vector2Int> target, string configFile)
        {
            var text = Resources.Load(configFile) as TextAsset;
            var values = INIParser.Parse(text.text);
            foreach(var i in values)
            {
                string[] vstr = i.Value.Split(',');
                int x = int.Parse(vstr[0]);
                int y = int.Parse(vstr[1]);
                target[i.Key.ToEnum<UnitType>()] = new Vector2Int(x, y);
            }
            Resources.UnloadAsset(text);
        }
        
        // ============================================================================================================
        // ============================================================================================================
        // ============================================================================================================
        
        
        public Vector2 cursorPointing
        {
            get
            {
                return RadiacUI.VirtualCursor.position
                    + (Vector2)Camera.main.transform.position
                    - new Vector2(Screen.width, Screen.height) * 0.5f;
            }
        }
        
        public Vector2Int cursorPointingGrid
        {
            get
            {
                // TODO:
                // Hard code grid size.
                // This should be move to other place...
                // Alongside the same value that appears everywhere.
                var cur = cursorPointing / gridSize;
                return new Vector2Int(Mathf.FloorToInt(cur.x), -Mathf.CeilToInt(cur.y));
            }
        }
    }
    
    
    
    
}
