using UnityEngine;
using System;
using System.Collections.Generic;
using StateOfWarUtility;

namespace MapEditor
{
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
        
        public SrfFileAccesser srfAccesser;
        public Texture2D srf;
        public string srfName;
        
        public bool showTiles;
        public bool showGridPointer;
        public bool showDecoration;
        public bool showMousePosition;
        
        public object clipBoard;
        
        public readonly Selection selection = new Selection();
        
        public readonly Dictionary<UnitType, Vector2Int> offsets = new Dictionary<UnitType, Vector2Int>();
        public readonly Dictionary<UnitType, Vector2Int> buildingSize = new Dictionary<UnitType, Vector2Int>();
        
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
            SetVec2FromConfig(offsets, "Offsets");
            SetVec2FromConfig(buildingSize, "BuildingSize");
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
            => Vector2.Scale(
                RadiacUI.VirtualCursor.position
                + (Vector2)Camera.main.transform.position
                - new Vector2(Screen.width, Screen.height) * 0.5f,
                new Vector2(1f, -1f));
        
        // TODO:
        // Hard code grid size.
        // This should be move to other place...
        // Alongside the same value that appears everywhere.
        public Vector2Int cursorPointingGrid
            => new Vector2Int(
                Mathf.FloorToInt((cursorPointing / gridSize).x),
                Mathf.FloorToInt((cursorPointing / gridSize).y));
    }
    
    
    
    /// <summary>
    /// A bug represented when put this class onto the top of this file.
    /// Unity 2018.1.6
    /// </summary>
    [Serializable]
    public class Selection
    {
        bool isBattleUnit { get; set; }
        
        // The index of the Unit/Building that is selected.
        // Used with the edt file.
        public int id;
        
        public bool selected { get; private set; }
        
        public Building building => (!selected || isBattleUnit) ? null : Global.inst.edt.buildings[id];
        public BattleUnit battleUnit => (!selected || !isBattleUnit) ? null : Global.inst.edt.units[id];
        
        public void SetBuilding(int i)
        {
            isBattleUnit = false;
            id = i;
            selected =true;
        }
        
        public void SetUnit(int i)
        {
            isBattleUnit = true;
            id = i;
            selected = true;
        }
        
        public void Reset()
        {
            isBattleUnit = true;
            id = -1;
            selected = false;
        }
        
        public Selection() => Reset();
        
        public static implicit operator bool(Selection v) => v.selected;
    }
    
}
