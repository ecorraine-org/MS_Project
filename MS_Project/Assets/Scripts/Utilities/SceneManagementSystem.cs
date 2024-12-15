using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace PixelCrushers.SceneStreamer
{
    /// <summary>
    /// シーンの切り替えを管理するクラス
    /// </summary>
    public class SceneStreamerManager : MonoBehaviour
    {
        private static SceneStreamerManager instance;
        public static SceneStreamerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    var obj = new GameObject("SceneStreamerManager");
                    instance = obj.AddComponent<SceneStreamerManager>();
                    DontDestroyOnLoad(obj);
                }
                return instance;
            }
        }

        private string previousSceneName;
        private bool isTransitioning;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// シーンの切り替えを行う
        /// </summary>
        public IEnumerator TransitionToScene(string targetSceneName, bool isMainGameScene = false)
        {
            if (isTransitioning)
            {
                yield break;
            }

            isTransitioning = true;


            yield return StartCoroutine(CleanupCurrentSceneStreams());

            // シーンを切り替える
            if (isMainGameScene)
            {
                yield return StartCoroutine(InitializeMainGameScene(targetSceneName));
            }
            else
            {
                yield return StartCoroutine(LoadSingleScene(targetSceneName));
            }

            previousSceneName = targetSceneName;
            isTransitioning = false;
        }

        private IEnumerator CleanupCurrentSceneStreams()
        {

            //全てのシーンをアンロード
            SceneStreamer.UnloadAll();

            // リソースのアンロード
            AsyncOperation unloadOperation = Resources.UnloadUnusedAssets();
            yield return unloadOperation;

            // ガベージコレクション
            System.GC.Collect();
        }

        private IEnumerator InitializeMainGameScene(string mainSceneName)
        {
            // メインゲームシーンを読み込む
            yield return StartCoroutine(LoadSingleScene(mainSceneName));
            SceneStreamer.SetCurrentScene(mainSceneName);
        }

        private IEnumerator LoadSingleScene(string sceneName)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
            while (!loadOperation.isDone)
            {
                yield return null;
            }
        }

        /// <summary>
        /// シーンの切り替えを行う
        /// </summary>
        public static void TransitionScene(string targetSceneName, bool isMainGameScene = false)
        {
            Instance.StartCoroutine(Instance.TransitionToScene(targetSceneName, isMainGameScene));
        }
    }

    /// <summary>
    /// シーンの切り替えを補助するクラス
    /// </summary>
    public class SceneTransitionHelper : MonoBehaviour
    {
        public void TransitionToMainGame(string mainSceneName)
        {
            SceneStreamerManager.TransitionScene(mainSceneName, true);
        }

        public void TransitionToMenu(string menuSceneName)
        {
            SceneStreamerManager.TransitionScene(menuSceneName, false);
        }
    }
}