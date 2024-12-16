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
    public GameObject hammerbox;
    [SerializeField, Header("槍の素材")]
    public GameObject spearbox;
    
    private bool isVisible = false; // boxが表示中かどうか
    private bool isSliding = false; // スライド中かどうか

    private void Start()
    {
        // 通知BOXを初期位置に設定
        if (box != null) 
        {
            box.anchoredPosition = offScreenPosition;
        }
    }
    private void Update()
    {
        // 変身したときに～するif文に変更
        if (Input.GetKeyDown(KeyCode.K))
        {
            //通知BOXを表示
            TogglePanel();
        }

        // 武器の切り替え←ここのif文の中を変身の変数に
        if (Input.anyKeyDown)
        {
            // 剣、ハンマー、槍の切り替え用
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetWeapon("Sword");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetWeapon("Hammer");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetWeapon("Spear");
            }
        }
    }

    // 武器を切り替える
    private void SetWeapon(string weaponType)
    {
        // すべての武器素材を非表示
        swordbox.SetActive(false);
        hammerbox.SetActive(false);
        spearbox.SetActive(false);

        //表示する武器を決定
        switch (weaponType)
        {
            case "Sword":
                swordbox.SetActive(true);
                Debug.Log("剣を選択しました");
                break;
            case "Hammer":
                hammerbox.SetActive(true);
                Debug.Log("ハンマーを選択しました");
                break;
            case "Spear":
                spearbox.SetActive(true);
                Debug.Log("槍を選択しました");
                break;
            default:
                Debug.LogWarning("無効な武器タイプが指定されました: " + weaponType);
                break;
        }
    }

    //スライド制御関数
    public void TogglePanel()
    {
        //if (isSliding) return; //スライド中は操作しない
        
        isSliding = true; //スライド中かどうか
        
        //isVisible = true なら offScreenPositionまでスライド、false なら onScreenPosition までスライド
        Vector2 targetPosition = isVisible ? offScreenPosition : onScreenPosition;
        StartCoroutine(SlidePanel(targetPosition));

    }
    //スライドさせる関数
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