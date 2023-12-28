using UnityEditor;
using UnityEngine;

namespace Core.Unity.Utils.Editor
{
    [CustomPropertyDrawer(typeof(ushort))]
    public class UnsignedPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
                                   GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            property.intValue = EditorGUI.IntField(position, label, property.intValue);
            property.intValue = Mathf.Max(0, property.intValue);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}