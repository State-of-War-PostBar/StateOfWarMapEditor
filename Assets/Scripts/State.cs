using System;
using UnityEngine;

using RadiacUI;

namespace MapEditor
{
    public enum StateType
    {
        None = 0,
        BuildingAndUnits,
        MapMainpulating,
        TilMainpulating
    }
    
    /// <summary>
    /// Indicates the user mode, mouse state, selection, and so on.
    /// </summary>
    public sealed class State
    {
        public StateType current;
        
        
        /// <summary>
        /// The viewport in map coordinates.
        /// </summary>
        public Rect viewport
        {
            get { throw new NotImplementedException(); }
        }
        
        /// <summary>
        /// The cursor position in map coordinates.
        /// </summary>
        /// <returns></returns>
        public Vector2 cursorPos
        {
            get { throw new NotImplementedException(); }
        }
        
        /// <summary>
        /// The grid (tail) the cursor is pointing to.
        /// </summary>
        /// <returns></returns>
        public Vector2Int cursorTarget
        {
            get { throw new NotImplementedException(); }   
        }
        
        
        
        
    }
}
