using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnomatoprBox : MonoBehaviour
{
    [SerializeField, Header("通知BOX")]
    public RectTransform box; // 操作するPanelのRectTransform
    [SerializeField, Header("開始地点")]
    public Vector2 offScreenPosition; // Panelが画面外にあるときの座標
    [SerializeField, Header("終了地点")]
    public Vector2 onScreenPosition;  // Panelが画面内にあるときの座標
    [SerializeField, Header("スライド時間")]
    public float slideDuration; // スライドアニメーションの所要時間（秒）

    [SerializeField, Header("ぷよぷよ")]
    public GameObject puyobox;
    [SerializeField, Header("シュッ")]
    public GameObject syubox;
    [SerializeField, Header("フワフワ")]
    public GameObject huwahuwabox;
    [SerializeField, Header("ブゥウン")]
    public GameObject buunbox;
    [SerializeField, Header("ガシャ")]
    public GameObject gasyabox;
    [SerializeField, Header("シュッ")]
    public GameObject kisi_syubox;
    [SerializeField, Header("ズシズシ")]
    public GameObject zusizusibox;
    [SerializeField, Header("ドン")]
    public GameObject donbox;
    [SerializeField, Header("ペタペタ")]
    public GameObject petapetabox;
    [SerializeField, Header("ポイッ")]
    public GameObject poibox;

    private bool isVisible = false; // boxが表示中かどうか
    private bool isSliding = false; // スライド中かどうか

    float cunt;
    PlayerMode playermode; //プレイヤーの状態を保存(武器)
    string Onomatope;
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
    private void ModeChange(PlayerMode _mode, OnomatopoeiaData _data)
    {
        playermode = _mode; //モード設定
        Onomatope = _data.wordToUse; //オノマトペを設定
        Debug.Log("モードチェンジしたよ" + playermode);
        Debug.Log("オノマトペを食べたよ" + _data.wordToUse);
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
        if (isVisible == true)
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
        if (Onomatope == "ぷよぷよ")
        {
            SetWeapon("ぷよぷよ");
            puyobox.SetActive(true);
            isVisible = true;
            cunt = 0;
        }
        else if (Onomatope == "シュッ")
        {
            SetWeapon("シュッ");
            syubox.SetActive(true);
            isVisible = true;
            cunt = 0;
        }
        else if (Onomatope == "フワフワ")
        {
            SetWeapon("フワフワ");
            huwahuwabox.SetActive(true);
            isVisible = true;
            cunt = 0;
        }
        else if (Onomatope == "ブゥウン")
        {
            SetWeapon("ブゥウン");
            buunbox.SetActive(true);
            isVisible = true;
            cunt = 0;
        }
        else if (Onomatope == "ガシャ")
        {
            SetWeapon("ガシャ");
            gasyabox.SetActive(true);
            isVisible = true;
            cunt = 0;
        }
        else if (Onomatope == "シュッ")
        {
            SetWeapon("シュッ");
            kisi_syubox.SetActive(true);
            isVisible = true;
            cunt = 0;
        }
        else if (Onomatope == "ズシズシ")
        {
            SetWeapon("ズシズシ");
            zusizusibox.SetActive(true);
            isVisible = true;
            cunt = 0;
        }
        else if (Onomatope == "ドン")
        {
            SetWeapon("ドン");
            donbox.SetActive(true);
            isVisible = true;
            cunt = 0;
        }
        else if (Onomatope == "ペタペタ")
        {
            SetWeapon("ペタペタ");
            petapetabox.SetActive(true);
            isVisible = true;
            cunt = 0;
        }
        else if (Onomatope == "ポイッ")
        {
            SetWeapon("ポイッ");
            poibox.SetActive(true);
            isVisible = true;
            cunt = 0;
        }

    }
    private void SetWeapon(string weaponType)
    {
        //確認用
        switch (weaponType)
        {
            case "ぷよぷよ":
                Debug.Log("ぷよぷよを食べました");
                break;
            case "シュッ":
                Debug.Log("シュッを食べました");
                break;
            case "フワフワ":
                Debug.Log("フワフワ");
                break;
            case "ブゥウン":
                Debug.Log("ブゥウン");
                break;
            case "ガシャ":
                Debug.Log("ガシャ");
                break;
            case "ズシズシ":
                Debug.Log("ズシズシ");
                break;
            case "ドン":
                Debug.Log("ドン");
                break;
            case "ペタペタ":
                Debug.Log("ペタペタ");
                break;
            case "ポイッ":
                Debug.Log("ポイッ");
                break;
            default:
                Debug.LogWarning("無効な武器タイプが指定されました: " + weaponType);
                break;
        }
    }

    // すべての武器素材を非表示
    private void AllOff()
    {
        puyobox.SetActive(false);
        syubox.SetActive(false);
        huwahuwabox.SetActive(false);
        buunbox.SetActive(false);
        gasyabox.SetActive(false);
        kisi_syubox.SetActive(false);
        zusizusibox.SetActive(false);
        donbox.SetActive(false);
        petapetabox.SetActive(false);
        poibox.SetActive(false);
    }
}
