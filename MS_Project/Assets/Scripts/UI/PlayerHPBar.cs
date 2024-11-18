using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    private PlayerController player;

    public float maxHp;
    public float currentHp;

    // スライダー
    public Slider hpSlider;

    // スライダーの子オブジェクト（背景と前景）
    public Image background; // 背景用イメージ
    public Image fill;       // 前景用イメージ

    // 背景HPを減少させるためのコルーチン
    //private Coroutine decreaseBackgroundHpCoroutine;

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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //最大HP設定
        maxHp = player.StatusManager.StatusData.maxHealth;
        hpSlider.maxValue = maxHp;

        // スライダーを満タンにする
        currentHp = maxHp;
        hpSlider.value = currentHp;
        //UpdateHPBar();
        Debug.Log("Start currentHp : " + currentHp);

        // 背景HPを減少させるコルーチンを常に開始
        //decreaseBackgroundHpCoroutine = StartCoroutine(DecreaseBackgroundHp());
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

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    currentHp -= 10;
        //    currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        //    Debug.Log("After I key, currentHp : " + currentHp);
        UpdateHPBar();
        //}

        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    currentHp += 10;
        //    currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        //    Debug.Log("After O key, currentHp : " + currentHp);
        //    UpdateHPBar();
        //}
    }

    // HPバーを更新するメソッド
    private void UpdateHPBar()
    {
        // HPバーのスライダーの値を更新
        float normalizedValue = (float)currentHp / maxHp;
        hpSlider.value = currentHp;

        // 前景のサイズを調整
        fill.fillAmount = normalizedValue; // 子オブジェクトの前景のサイズを調整
        //Debug.Log("HP: " + currentHp + " / " + maxHp + ", Fill Amount: " + fill.fillAmount);
    }

    //イベントでHPを更新する
    public void SetCurrentHp(float _currentHp)
    {
        currentHp = _currentHp;

        UpdateHPBar();
    }

    // 背景HPを前景HPまで減少させるコルーチン
    /*private IEnumerator DecreaseBackgroundHp()
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
