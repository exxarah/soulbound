using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace Core.Unity.Utils.Editor
{
    public static class SerializedPropertyExtensions
    {
#if UNITY_EDITOR
        // Gets value from SerializedProperty - even if value is nested
        public static T GetSerializedValue<T>(this SerializedProperty property)
        {
            object @object = property.serializedObject.targetObject;
            string[] propertyNames = property.propertyPath.Split('.');

            List<string> propertyNamesClean = new();

            for (int i = 0; i < propertyNames.Count(); i++)
                if (propertyNames[i] == "Array")
                {
                    if (i != propertyNames.Count() - 1 && propertyNames[i + 1].StartsWith("data"))
                    {
                        int pos = Int32.Parse(propertyNames[i + 1].Split('[', ']')[1]);
                        propertyNamesClean.Add($"-GetArray_{pos}");
                        i++;
                    }
                    else
                    {
                        propertyNamesClean.Add(propertyNames[i]);
                    }
                }
                else
                {
                    propertyNamesClean.Add(propertyNames[i]);
                }

            // Get the last object of the property path.
            foreach (string path in propertyNamesClean)
                if (path.StartsWith("-GetArray"))
                {
                    string[] split = path.Split('_');
                    int index = Int32.Parse(split[split.Count() - 1]);
                    IList l = (IList)@object;
                    @object = l[index];
                }
                else
                {
                    @object = @object.GetType()
                                     .GetField(path,
                                               BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                                     .GetValue(@object);
                }

            return (T)@object;
        }

        // Sets value from SerializedProperty - even if value is nested
        public static void SetValue(this SerializedProperty property, object val)
        {
            object obj = property.serializedObject.targetObject;

            List<KeyValuePair<FieldInfo, object>> list = new();

            FieldInfo field = null;
            foreach (string path in property.propertyPath.Split('.'))
            {
                Type type = obj.GetType();
                field = type.GetField(path);
                list.Add(new KeyValuePair<FieldInfo, object>(field, obj));
                obj = field.GetValue(obj);
            }

            // Now set values of all objects, from child to parent
            for (int i = list.Count - 1; i >= 0; --i)
            {
                list[i].Key.SetValue(list[i].Value, val);
                // New 'val' object will be parent of current 'val' object
                val = list[i].Value;
            }
        }
#endif // UNITY_EDITOR
    }
}