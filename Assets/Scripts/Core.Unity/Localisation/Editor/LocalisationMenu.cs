using System;
using System.Collections.Generic;
using Core.Extensions;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine.Networking;

namespace Core.Unity.Localisation.Editor
{
    public static class LocalisationMenu
    {
        private const string GoogleSheetsURL = "https://docs.google.com/spreadsheets/d/{0}/export?format=csv&gid={1}";
        private const string GoogleSheetsID = "1yXZFQGOaE1FhUJchamhoj_PwZpkarHntfiWuBK2G734";
        private const string TabID = "0";
        
        [MenuItem("Game/Import Strings", false, 30)]
        public static void ImportStrings()
        {
            Async_ImportFromGSheet().Forget();
        }
        
        private static async UniTask Async_ImportFromGSheet()
        {
            string url = string.Format(GoogleSheetsURL, GoogleSheetsID, TabID);
            string data = null;

            EditorUtility.DisplayProgressBar("Importing Database", $"Downloading strings from GoogleSheets", 0.2f);
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.disposeDownloadHandlerOnDispose = true;
                request.disposeCertificateHandlerOnDispose = true;
                request.disposeUploadHandlerOnDispose = true;

                await request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    Log.Error($"Download Error: {request.error}");
                    EditorUtility.ClearProgressBar();
                    return;
                }
                
                data = request.downloadHandler.text;
            }

            string[] rows = data.Split('\n');
            
            // First row is headers / column keys
            string[] columns = rows[0].Split(',');
            Dictionary<string, LanguageData> languages = new();
            foreach (string key in columns)
            {
                LanguageData languageData = LocalisationManager.Instance.GetLanguageData(key);
                if (languageData != null)
                {
                    languages.Add(key, languageData);
                }
            }

            for (int idx = 1; idx < rows.Length; idx++)
            {
                float progress = 0.2f + (idx / (float)rows.Length) * 0.6f;
                EditorUtility.DisplayProgressBar("Importing Database", $"Importing strings from GoogleSheets: {idx - 1}/{rows.Length - 2}", progress);

                string[] currentRow = rows[idx].Split(',');
                int rowLength = Math.Min(currentRow.Length, columns.Length);
                for (int i = 1; i < rowLength; i++)
                {
                    if (languages.TryGetValue(columns[i], out LanguageData language))
                    {
                        language.SetString(currentRow[0], currentRow[i]);
                    }
                }
            }
            
            // Final 20% is for validation
            EditorUtility.DisplayProgressBar("Importing Database",
                                             $"Validating strings from GoogleSheets",
                                             progress: 0.85f);
            bool valid = IsDataValid();
            foreach (KeyValuePair<string,LanguageData> language in languages)
            {
                EditorUtility.SetDirty(language.Value);
            }
            
            EditorUtility.ClearProgressBar();

            EditorUtility.DisplayDialog("Importing Database",
                                        $"Importing strings finished with result: {(valid ? "SUCCESS" : "FAIL")}",
                                        "ok");
            
        }

        private static bool IsDataValid()
        {
            return true;
        }
    }
}