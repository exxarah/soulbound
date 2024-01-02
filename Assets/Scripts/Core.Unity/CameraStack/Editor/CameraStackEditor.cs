using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Core.Unity.CameraStack.Editor
{
    [CustomEditor(typeof(CameraStack))]
    public class CameraStackEditor : UnityEditor.Editor
    {
        private ReorderableList m_list;
        private CameraStack m_stack;

        private void OnEnable()
        {
            m_stack ??= (CameraStack)target;
            SerializedProperty cameraList = serializedObject.FindProperty("m_cameras");

            m_list = new ReorderableList(serializedObject, cameraList, true, true, true, true);
            m_list.drawElementCallback = (rect, index, active, focused) =>
            {
                SerializedProperty stackedCam = m_list.serializedProperty.GetArrayElementAtIndex(index);

                GUILayout.BeginHorizontal();

                SerializedProperty camera = stackedCam.FindPropertyRelative("Camera");
                EditorGUI.PropertyField(
                                        new Rect(rect.xMin, rect.yMin, rect.width / 4 * 3, rect.height),
                                        camera, GUIContent.none
                                       );

                EditorGUILayout.Space(10.0f);

                SerializedProperty cameraOrder = stackedCam.FindPropertyRelative("CameraOrder");
                EditorGUI.PropertyField(
                                        new Rect(rect.xMin + rect.width / 4 * 3, rect.yMin, rect.width / 4,
                                                 rect.height),
                                        cameraOrder, GUIContent.none
                                       );

                GUILayout.EndHorizontal();
            };

            m_list.drawHeaderCallback = rect => { EditorGUI.LabelField(rect, "Cameras"); };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            m_list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}