using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UISceneChange : MonoBehaviour
{
    /*public Image fadePanel;             // フェード用のUIパネル（Image）
    //public float fadeDuration = 1.0f;   // フェードの完了にかかる時間
    //private bool isFading = false;      // フェード中かどうかを判定
    public GameObject buttonObject;     // ボタンのGameObjectを参照する変数
    private Button button;              // Buttonコンポーネントの参照
    public Sprite pushButton;           // ボタンが押されたときのスプライト
    private Sprite originalSprite;      // 元のボタンのスプライト
    */
    [SerializeField] string sceneToLoad; // 切り替えるシーン名を指定

    /*private void Start()
    {
        if (buttonObject == null)
        {
            Debug.LogError("buttonObjectが設定されていません。インスペクターで設定してください。");
            return;
        }

        button = buttonObject.GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("buttonObjectにButtonコンポーネントがアタッチされていません。正しいオブジェクトを設定してください。");
            return;
        }

        originalSprite = button.image.sprite; // 元のスプライトを保存
    }*/
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
    // ボタン用のメソッド
    public void OnButtonClick()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //StartCoroutine(FadeOutAndLoadScene());

            SceneManager.LoadScene(sceneToLoad);
        }
    }

    /*public IEnumerator FadeOutAndLoadScene()
    {
        fadePanel.enabled = true;   // フェードパネルを有効化

        isFading = true;                                 // フェード中のフラグを立てる

        float elapsedTime = 0.0f;                        // 経過時間を初期化
        Color startColor = fadePanel.color;              // フェードパネルの開始色を取得
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // フェードパネルの最終色を設定

         フェードアウトアニメーションを実行
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;                        // 経過時間を増やす
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // フェードの進行度を計算
            fadePanel.color = Color.Lerp(startColor, endColor, t); // パネルの色を変更してフェードアウト
            //yield return null;                                     // 1フレーム待機
        }

        //fadePanel.color = endColor;                                // フェードが完了したら最終色に設定
        SceneManager.LoadScene(sceneToLoad);                    // シーンをロードしてメニューシーンに遷移
    }*/
}
