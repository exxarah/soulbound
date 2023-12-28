using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Core.Unity.Localisation.Editor
{
    [CustomEditor(typeof(LocaliseText), true)]
    public class LocaliseTextEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LocaliseText text = (LocaliseText)target;

            if (GUILayout.Button(new GUIContent("Find String"), EditorStyles.popup))
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)),
                                  LocalisationManager.Instance.GetLanguageData()
                                                     .GetSearchProvider(x => text.SetKey(x)));

            serializedObject.ApplyModifiedProperties();
        }
    }
}