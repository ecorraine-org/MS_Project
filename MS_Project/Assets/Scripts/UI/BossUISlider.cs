using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BossUISlider : MonoBehaviour
{
    private EnemyController enemy;

    [Tooltip("ラベル")]
    private string label;
    [Tooltip("最大値")]
    private float maxValue;
    [Tooltip("現在値")]
    private float currentValue;

    [Header("スライダーオブジェクト"), Tooltip("スライダー")]
    public Slider slider;
    [Header("スライダー背景"), Tooltip("スライダー背景")]
    public Image sliderBackground;
    [Header("スライダー塗り"), Tooltip("スライダー塗り")]
    public Image sliderFill;

    public UnityEvent InitSliderValues;

    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();

        InitSliderValues?.Invoke();

        CustomLogger.Log(label + "初期値：" + currentValue);
    }

    private void Update()
    {
        if(enemy == null)
        {
            if (enemy.Status.StatusData.enemyRank == EnemyRank.Boss)
            {
                //float hp = 100;
            }
        }
        //enemy = GameObject.FindGameObjectWithTag("Boss").GetComponent<EnemyController>();

        if (label == "HP")
        {
            currentValue = enemy.Status.CurrentHealth;
        }
       
        UpdateSliderBar();
    }

    /// <summary>
    /// スライダーを更新
    /// </summary>
    public void UpdateSliderBar()
    {
        // スライダーの値を更新
        float normalizedValue = (float)currentValue / maxValue;
        slider.value = currentValue;

        // 塗りを調整
        sliderFill.fillAmount = normalizedValue;
    }

    /// <summary>
    /// ＨＰ値初期化
    /// </summary>
    public void InitHPValues()
    {
        
        if(enemy == null)
        {
            Debug.Log("NULL");
        }

        float hp = enemy.Status.StatusData.maxHealth;

        //float hp = 100;

        // 最大値設定
        maxValue = hp;
        slider.maxValue = maxValue;
        // 現在値設定
        currentValue = maxValue;
        slider.value = currentValue;

        label = "HP";
    }

    /// <summary>
    /// 暴走値初期化
    /// </summary>
    


    /// <summary>
    /// ＨＰを更新する
    /// </summary>
    /// <param name="_currentHp">現在ＨＰ</param>
    public void SetCurrentHp(float _currentHp)
    {
        currentValue = _currentHp;
    }

    /// <summary>
    /// 暴走値を更新する
    /// </summary>
    /// <param name="_currentRage">現在暴走値</param>
    public void SetCurrentRage(float _currentRage)
    {
        currentValue = _currentRage;
    }

    public string Label
    {
        get => label;
    }
    /*
    private void OnEnable()
    {
        //イベントをバインドする
        PlayerStatusManager.OnUpdateHPBarEvent += SetCurrentHp;
    }

    private void OnDisable()
    {
        //バインドを解除する
        PlayerStatusManager.OnUpdateHPBarEvent -= SetCurrentHp;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            int damage = Random.Range(1, 100);
            Debug.Log("damage : " + damage);
            currentHp -= damage;
            currentHp = Mathf.Clamp(currentHp, 0, maxHp); // HPが0未満にならないように

            // HPバーを更新
            UpdateHPBar();
        }
    }

    // 背景HPを前景HPまで減少させるコルーチン
    private IEnumerator DecreaseBackgroundHp()
    {
        while (true)
        {
            // 前景のHPに基づいて背景のHPを減少
            float targetValue = (float)currentHp / maxHp;

            // 背景のHPを前景のHPまで1ずつ減少
            if (background.fillAmount > targetValue)
            {
                background.fillAmount -= 0.01f; // 0.01ずつ減少（調整可能）
            }
            else if (background.fillAmount < targetValue)
            {
                background.fillAmount += 0.01f; // 0.01ずつ増加（調整可能）
            }

            yield return new WaitForSeconds(0.01f); // 一定の時間待機（調整可能）
        }
    }
    */
}
