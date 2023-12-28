using UnityEditor;
using UnityEngine;

namespace Core.Unity.Flow.Editor
{
    public class FlowGraphWindow : EditorWindow
    {
        [MenuItem("Core/Flow Graph")]
        private static void ShowWindow()
        {
            var window = GetWindow<FlowGraphWindow>();
            window.titleContent = new GUIContent("Flow Graph");
            window.Show();
        }

        private void OnGUI()
        {
            DrawNodes();

            ProcessEvents(Event.current);

            if (GUI.changed)
            {
                Repaint();
            }
        }

        private void DrawNodes()
        {
            
        }

        private void ProcessEvents(Event e)
        {
            
        }
    }
}