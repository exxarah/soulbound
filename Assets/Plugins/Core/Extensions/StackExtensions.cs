using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Extensions
{
    public static class StackExtensions
    {
        public static void Replace<T>(this Stack<T> stack, T valueToReplace, T valueToReplaceWith, IEqualityComparer<T> comparer = null)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;

            var temp = new Stack<T>();
            while (stack.Count > 0)
            {
                var value = stack.Pop();
                if (comparer.Equals(value, valueToReplace))
                {
                    stack.Push(valueToReplaceWith);
                    break;
                }
                temp.Push(value);
            }

            while (temp.Count > 0)
                stack.Push(temp.Pop());
        }

        public static void Replace<T>(this Stack<T> stack, int idxToReplace, T valueToReplaceWith)
        {
            var temp = new Stack<T>();
            while (stack.Count - 1 > idxToReplace)
            {
                var value = stack.Pop();
                temp.Push(value);
            }
            
            // Add the replacement
            stack.Pop();
            stack.Push(valueToReplaceWith);

            while (temp.Count > 0)
                stack.Push(temp.Pop());
        }
    }
}