using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IrisShot : MonoBehaviour
{
    [SerializeField] RectTransform unmask;
    readonly Vector2 IRIS_IN_SCALE = new Vector2(30, 30);
    readonly float SCALE_DURATION = 2;
    [SerializeField] string sceneToLoad; // 切り替えるシーン名を指定

    private void Start()
    {
        // シーン開始時にアイリスイン
        IrisIn();
    }

    public void IrisIn()
    {
        // アイリスイン（大きくして表示）
        unmask.localScale = Vector3.zero;  // 初期状態で小さくしておく
        unmask.DOScale(IRIS_IN_SCALE, SCALE_DURATION).SetEase(Ease.InCubic);
    }

    public void IrisOut()
    {
        // アイリスアウト（小さくして消える）とシーン遷移を開始
        StartCoroutine(IrisOutAndLoadScene());
    }

    private IEnumerator IrisOutAndLoadScene()
    {
        // アニメーションの完了を待つ
        yield return unmask.DOScale(Vector3.zero, SCALE_DURATION).SetEase(Ease.OutCubic).WaitForCompletion();

        // シーン遷移
        SceneManager.LoadScene(sceneToLoad);
    }

    private void Update()
    {
        // Enterキーでアイリスアウト
        if (Input.GetKeyDown(KeyCode.Return))
        {
            IrisOut();
        }
    }
}