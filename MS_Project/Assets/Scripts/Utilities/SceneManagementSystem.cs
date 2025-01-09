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

        private void Update()
        {
            //StartScene01という名のシーンが2つロードされている場合、片方をアンロードする
            RemoveInactiveDuplicateScenes("StartScene01");
        }

        private void RemoveInactiveDuplicateScenes(string sceneName)
        {
            // 現在ロードされているすべてのシーンを取得
            int sceneCount = SceneManager.sceneCount;
            Scene activeScene = SceneManager.GetActiveScene();

            for (int i = 0; i < sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);

                // 名前が一致し、アクティブではないシーンを確認
                if (scene.name == sceneName && scene != activeScene)
                {
                    Debug.Log($"非アクティブなシーン {sceneName} をアンロードします: {scene.path}");
                    // シーンを非同期でアンロード
                    SceneManager.UnloadSceneAsync(scene);
                }
            }
        }
        /// <summary>
        /// シーンを非同期で読み込む
        /// </summary>
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
            loadOperation.allowSceneActivation = false;

            while (!loadOperation.isDone)
            {
                // ロードが完了したらシーンをアクティブ化
                if (loadOperation.progress >= 0.9f)
                {
                    loadOperation.allowSceneActivation = true;
                }

                yield return null;
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
                yield return StartCoroutine(LoadSceneAsync(targetSceneName));
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
            Debug.Log("Main game scene loaded");
        }

        private IEnumerator LoadSingleScene(string sceneName)
        {
            // シーンを読み込む
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