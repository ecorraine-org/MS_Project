using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ChangeBox : MonoBehaviour
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

    float cunt;
    PlayerMode playermode; //プレイヤーの状態を保存(武器)
    private void OnEnable()
    {
        //イベントをバインドする
        OnomatoManager.OnModeChangeEvent += ModeChange;
    }

    private void OnDisable()
    {
        //バインドを解除する
        OnomatoManager.OnModeChangeEvent -= ModeChange;
    }
    //モードが変わった時にテキストボックスを表示する
    private void ModeChange(PlayerMode _mode, string _name)
    {
        playermode = _mode; //モード設定
        Debug.Log("モードチェンジしたよ" + playermode);
        Debug.Log("オノマトペを食べたよ" + _name);
        playermode = BattleManager.Instance.CurPlayerMode; //現在の状態を保存
        OnOff(); //通知BOXを表示
        SlideIn(); //通知boxをスライド
    }
    //--------------------------------スライド処理--------------------------------
    public void SlideIn()
    {
        if (isSliding) return; //スライド中は操作しない

        isSliding = true; //スライド中かどうか

        //offScreenPosition から onScreenPosition までスライド
        Vector2 targetPosition = onScreenPosition;
        StartCoroutine(SlidePanel(targetPosition));

    }
    public void SlideOut()
    {
        if (isSliding) return; //スライド中は操作しない

        isSliding = true; //スライド中かどうか

        //onScreenPosition から offScreenPosition までスライド
        Vector2 targetPosition = offScreenPosition;
        StartCoroutine(SlidePanel(targetPosition));

    }
    //スライドする処理
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
        }
        box.anchoredPosition = targetPosition; //最後に目標位置にピッタリ合わせる
        isSliding = false; //スライドアニメーションの初期化
    }

    //----------------------------------表示処理----------------------------------
    void Start()
    {
        AllOff();
    }
    //4秒後に消える処理
    void Update()
    {
        if(isVisible == true)
        {
            cunt += Time.deltaTime;

            if (cunt >= 4)
            {
                SlideOut();
            }
        }
    }
    private void OnOff()
    {
        AllOff();

        // 剣、ハンマー、槍の切り替え用
        if (playermode == PlayerMode.Sword)
        {
            SetWeapon("Sword");
            swordbox.SetActive(true);
            isVisible = true;
            cunt = 0;
        }
        else if (playermode == PlayerMode.Hammer)
        {
            SetWeapon("Hammer");
            hammerbox.SetActive(true);
            isVisible = true;
            cunt = 0;
        }
        else if (playermode == PlayerMode.Spear)
        {
            SetWeapon("Spear");
            spearbox.SetActive(true);
            isVisible = true;
            cunt = 0;
        }
    }
    private void SetWeapon(string weaponType)
    {
        //確認用
        switch (weaponType)
        {
            case "Sword":
                Debug.Log("剣を選択しました");
                break;
            case "Hammer":
                Debug.Log("ハンマーを選択しました");
                break;
            case "Spear":
                Debug.Log("槍を選択しました");
                break;
            default:
                Debug.LogWarning("無効な武器タイプが指定されました: " + weaponType);
                break;
        }
    }

    // すべての武器素材を非表示
    private void AllOff()
    {
        swordbox.SetActive(false);
        hammerbox.SetActive(false);
        spearbox.SetActive(false);
    }
}