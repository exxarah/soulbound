using System.Collections.Generic;
using UnityEngine;

namespace Core.Unity.Utils
{
    public static class ListExtensions
    {
        public static void SetActive(this List<GameObject> list, bool isActive)
        {
            if (list == null) { return; }

            for (int i = 0; i < list.Count; i++)
            {
                list[i].SetActiveSafe(isActive);
            }
        }
    }
}