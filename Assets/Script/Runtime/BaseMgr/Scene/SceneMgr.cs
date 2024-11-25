using Jam.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jam.Runtime.Scene_
{

    public class SceneMgr : IMgr
    {
        public void Shutdown(bool isAppQuit)
        {
        }

        public static AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            return SceneManager.LoadSceneAsync(sceneName, mode);
        }

        public static AsyncOperation UnloadSceneAsync(string sceneName)
        {
            return SceneManager.UnloadSceneAsync(sceneName);
        }

        public static bool SetActiveScene(Scene scene)
        {
            return SceneManager.SetActiveScene(scene);
        }

        public static Scene GetSceneByName(string name)
        {
            return SceneManager.GetSceneByName(name);
        }
    }

}