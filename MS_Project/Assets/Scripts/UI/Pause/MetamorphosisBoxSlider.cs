using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetamorphosisBoxSlider : MonoBehaviour
{
    [SerializeField, Header("通知BOX")]
    public RectTransform box; // 操作するPanelのRectTransform
    [SerializeField, Header("開始地点")]
    public Vector2 offScreenPosition; // Panelが画面外にあるときの座標
    [SerializeField, Header("終了地点")]
    public Vector2 onScreenPosition;  // Panelが画面内にあるときの座標
    [SerializeField, Header("スライド時間")]
    public float slideDuration; // スライドアニメーションの所要時間（秒）

    [SerializeField, Header("剣の素材")]
    public GameObject swordbox;
    [SerializeField, Header("ハンマーの素材")]
    public GameObject hammerdbox;
    [SerializeField, Header("槍の素材")]
    public GameObject spearbox;
    
    private bool isVisible = false; // boxが表示中かどうか
    private bool isSliding = false; // スライド中かどうか

    private void Start()
    {
        // 初期位置を設定
        if (box != null) 
        {
            box.anchoredPosition = offScreenPosition;
        }
    }
    private void Update()
    {
        // ここを変身した時の変数にしてください
        if (Input.GetKeyDown(KeyCode.K))
        {
            TogglePanel();
        }
    }
    //スライドさせる関数
    public void TogglePanel()
    {
        //if (isSliding) return; //スライド中は操作しない
        
        isSliding = true; //スライド中かどうか
        
        //isVisible = true なら offScreenPositionまでスライド、false なら onScreenPosition までスライド
        Vector2 targetPosition = isVisible ? offScreenPosition : onScreenPosition;
        StartCoroutine(SlidePanel(targetPosition));

    }

    private IEnumerator SlidePanel(Vector2 targetPosition)
    {
        float elapsedTime = 0f; //アニメーションの経過時間を追跡するための変数
        Vector2 startPosition = box.anchoredPosition; //スライド開始時のパネルの現在位置を保持

        //アニメーションが終了時間までループするよ
        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime; //前フレームから時間経過を加算
            float t = elapsedTime / slideDuration;
            box.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;

            // 割り込みチェック（必要なら条件を追加）
            if (Input.GetKeyDown(KeyCode.K))
            {
                Debug.Log("割り込み通知");
                //強制初期位置送還
                box.anchoredPosition = offScreenPosition;
                isVisible = true;
            }
        }
        box.anchoredPosition = targetPosition; //最後に目標位置にピッタリ合わせる
        isVisible = !isVisible; //真偽値の入れ替え
        isSliding = false; //スライドアニメーションの初期化
    }
}