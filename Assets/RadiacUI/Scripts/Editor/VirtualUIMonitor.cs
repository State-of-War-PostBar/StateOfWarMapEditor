using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;


namespace RadiacUI
{
    public class VirtualUIMonitor : EditorWindow
    {
        [MenuItem("RadiacUI/Open Virtual UI Monitor")]
        static void ShowWindow()
        {
            EditorWindow.GetWindow<VirtualUIMonitor>("Virtual Cursor");
        }
        
        public void OnGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Camera: ");
            EditorGUILayout.Vector2Field("Size", VirtualCamera.size);
            EditorGUILayout.Vector3Field("Positon", VirtualCamera.position);
            EditorGUILayout.Vector3Field("Direction", VirtualCamera.direction);
            GUILayout.EndVertical();
            
            GUILayout.BeginVertical();
            GUILayout.Label("Cursor: ");
            EditorGUILayout.Vector2Field("Position", VirtualCursor.position);
            EditorGUILayout.Vector2Field("Delta Position", VirtualCursor.deltaPosition);
            EditorGUILayout.Vector2Field("Viewport Position", VirtualCursor.viewportPosition);
            EditorGUILayout.EndVertical();
            
            GUILayout.Label(">>> Cursor Hovering: ");
            EditorGUILayout.BeginVertical();
            EditorGUI.indentLevel += 1;
            
            List<RadiacPanel> focused = new List<RadiacPanel>();
            
            foreach(var panel in Component.FindObjectsOfType<RadiacPanel>())
            {
                BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
                var cursorHovering = (bool)typeof(RadiacPanel).GetProperty("cursorHovering", flags).GetValue(panel);
                if(cursorHovering)
                {
                    focused.Add(panel);
                }
            }
            
            focused.Sort((a, b) => { return a.depth - b.depth; }); 
            
            foreach(var panel in focused) GUILayout.Label("    " + panel.name);
            
            EditorGUI.indentLevel -= 1;
            GUILayout.Label("<<< Cursor Hovering ends.");
            EditorGUILayout.EndVertical();
            
            
            Repaint();
        }
    }
}
