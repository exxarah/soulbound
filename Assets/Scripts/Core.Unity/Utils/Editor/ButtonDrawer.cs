// MIT License
//
// Copyright (c) 2022 Irakli Turabelidze
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Core.Unity.Utils.Editor
{
    public class Button
    {
        public readonly ButtonAttribute ButtonAttribute;
        public readonly string DisplayName;
        public readonly MethodInfo MethodInfo;

        public Button(MethodInfo methodInfo, ButtonAttribute buttonAttribute)
        {
            ButtonAttribute = buttonAttribute;
            DisplayName = String.IsNullOrEmpty(buttonAttribute.Name)
                ? ObjectNames.NicifyVariableName(methodInfo.Name)
                : buttonAttribute.Name;
            MethodInfo = methodInfo;
        }

        internal void Draw(IEnumerable<object> targets)
        {
            if (!GUILayout.Button(DisplayName)) return;

            foreach (object target in targets) MethodInfo.Invoke(target, null);
        }
    }

    public class ButtonDrawer
    {
        public readonly List<IGrouping<string, Button>> ButtonGroups;

        public ButtonDrawer(object target)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                                       BindingFlags.NonPublic;
            MethodInfo[] methods = target.GetType().GetMethods(flags);
            List<Button> buttons = new();
            int rowNumber = 0;

            foreach (MethodInfo method in methods)
            {
                ButtonAttribute buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();

                if (buttonAttribute == null)
                    continue;

                buttons.Add(new Button(method, buttonAttribute));
            }

            ButtonGroups = buttons.GroupBy(button =>
            {
                ButtonAttribute attribute = button.ButtonAttribute;
                bool hasRow = attribute.HasRow;
                return hasRow ? attribute.Row : $"__{rowNumber++}";
            }).ToList();
        }

        public void DrawButtons(IEnumerable<object> targets)
        {
            foreach (IGrouping<string, Button> buttonGroup in ButtonGroups)
            {
                if (buttonGroup.Count() > 0)
                {
                    float space = buttonGroup.First().ButtonAttribute.Space;
                    if (space != 0) EditorGUILayout.Space(space);
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    foreach (Button button in buttonGroup) button.Draw(targets);
                }
            }
        }
    }
}