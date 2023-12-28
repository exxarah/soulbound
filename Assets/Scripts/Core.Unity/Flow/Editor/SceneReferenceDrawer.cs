// Public Domain. NO WARRANTIES. License: https://opensource.org/licenses/0BSD

using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Core.Unity.Flow.Editor
{
    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferenceDrawer : PropertyDrawer
    {
        private static readonly Regex rArrayItemProp = new(@"\.Array\.data\[(\d+)\]$");

        private static readonly SceneReferencesEditorHelper helperAsset
            = AssetDatabase.LoadAssetAtPath<SceneReferencesEditorHelper>(
                                                                         AssetDatabase
                                                                            .GUIDToAssetPath("3ac7dd1ff29545725a68360083d41e41"));

        private readonly SerializedObject helper = new(helperAsset);

        ~SceneReferenceDrawer()
        {
            helper.Dispose();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.name == "data")
            {
                Match matchArrayItem = rArrayItemProp.Match(property.propertyPath);
                if (matchArrayItem.Success) // if property is array item, label should not be the struct field (GUID)
                    label = new GUIContent("Element " + matchArrayItem.Groups[1].Value, label.image, label.tooltip);
            }

            if (!property.NextVisible(true)) return;
            // could use 128-bit type to serialize, like rectIntValue; but it would be less legible in text assets
            string oldGuid = property.stringValue;
            string oldPath = AssetDatabase.GUIDToAssetPath(oldGuid);
            SceneAsset oldObj = AssetDatabase.LoadAssetAtPath<SceneAsset>(oldPath), newObj;
            try
            {
                SerializedProperty helperProp =
                    oldObj == null && oldGuid != null && oldGuid.Length > 0 // refers to missing GUID
                        ? helper.FindProperty(nameof(SceneReferencesEditorHelper
                                                        .missingScene)) // draw missing scene property
                        : helper.FindProperty(nameof(SceneReferencesEditorHelper
                                                        .nullScene)); // draw null scene property
                if (oldObj != null) helperProp.objectReferenceValue = oldObj;
                EditorGUI.ObjectField(position, helperProp, typeof(SceneAsset), label);
                if (helperProp.objectReferenceInstanceIDValue == 0)
                {
                    newObj = null; // set to null if null is selected
                }
                else
                {
                    // else set to object if not missing
                    newObj = helperProp.objectReferenceValue as SceneAsset;
                    if (newObj == null) return; // refers to missing GUID
                }
            }
            finally
            {
                helper.Update();
            } // reset helper so it does undo right

            if (newObj == null)
                property.stringValue = "";
            else if (newObj == oldObj) return;
            else if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(newObj, out string newGuid, out long _))
                property.stringValue = newGuid;
        }
    }
}