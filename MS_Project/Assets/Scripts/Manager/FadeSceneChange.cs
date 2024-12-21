using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PixelCrushers.SceneStreamer;

public class FadeSceneChange : MonoBehaviour
{
    public Image fadePanel;             // フェード用のUIパネル（Image）
    public float fadeDuration = 1.0f;   // フェードの完了にかかる時間
    private bool isFading = false;      // フェード中かどうかを判定
    public GameObject buttonObject;     // ボタンのGameObjectを参照する変数
    private Button button;              // Buttonコンポーネントの参照

    [SerializeField] string sceneToLoad; // 切り替えるシーン名を指定

    private void Start()
    {
        fadePanel.enabled = false;       // フェードパネルを無効化
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 0.0f); // 初期状態では透明
    }

    void Update()
    {
        // A,B,X,Yキーが押されたらフェードアウトを開始
        if (UIInputManager.Instance.GetAnyKeyTrigger() && !isFading)
        {
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    // ボタン用のメソッド
    public void OnButtonClick()
    {
        if (!isFading)
        {
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    public IEnumerator FadeOutAndLoadScene()
    {
        fadePanel.enabled = true;   // フェードパネルを有効化

        isFading = true;                                 // フェード中のフラグを立てる

        float elapsedTime = 0.0f;                        // 経過時間を初期化
        Color startColor = fadePanel.color;              // フェードパネルの開始色を取得
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // フェードパネルの最終色を設定

        // フェードアウトアニメーションを実行
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;                        // 経過時間を増やす
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // フェードの進行度を計算
            fadePanel.color = Color.Lerp(startColor, endColor, t); // パネルの色を変更してフェードアウト
            yield return null;                                     // 1フレーム待機
        }

        fadePanel.color = endColor;                                // フェードが完了したら最終色に設定
        //SceneManager.LoadScene(sceneToLoad);                    // シーンをロードしてメニューシーンに遷移

        // Set the current Scene to be able to unload it later
        UnityEngine.SceneManagement.Scene currentScene = SceneManager.GetActiveScene();

        // The Application loads the Scene in the background at the same time as the current Scene.
        if (!SceneManager.GetSceneByName("StartScene01").isLoaded)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("StartScene01", LoadSceneMode.Additive);

            // Wait until the last operation fully loads to return anything
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            GameObject stageInstance = Instantiate(Resources.Load("Others/StageInstance") as GameObject, new Vector3(-80f, 0.5f, 0f), Quaternion.identity);
            stageInstance.GetComponent<SetStartScene>().startSceneName = "Area000";
            GameObject player = Instantiate(Resources.Load("Player/Player") as GameObject, new Vector3(-80f, 0.5f, 0f), Quaternion.identity);
            RigidbodyConstraints originalConstraints = player.GetComponent<Rigidbody>().constraints;

            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;

            // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
            SceneManager.MoveGameObjectToScene(stageInstance, SceneManager.GetSceneByName("StartScene01"));
            SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName("StartScene01"));

            if (SceneManager.GetSceneByName("StartScene01").isLoaded)
            {
                player.GetComponent<Rigidbody>().constraints = originalConstraints;
            }
        }

        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);
    }

}