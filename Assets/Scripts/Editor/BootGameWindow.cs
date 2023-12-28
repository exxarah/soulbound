using UnityEditor;
using UnityEditor.SceneManagement;

namespace Editor
{
    public class BootGameWindow : EditorWindow
    {
        // [MenuItem("Game/Open GameScene", false, 0)]
        // public static void OpenGameScene()
        // {
        //     EditorSceneManager.SaveOpenScenes();
        //     EditorSceneManager.OpenScene("Assets/Scenes/Game/GameScreen.unity");
        // }
    
        [MenuItem("Game/Boot Game", false, 20)]
        public static void BootGame()
        {
            EditorSceneManager.SaveOpenScenes();
            EditorSceneManager.OpenScene("Assets/Scenes/BootScene.unity");
            EditorApplication.EnterPlaymode();
        }
    }
}