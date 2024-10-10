using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRage : MonoBehaviour
{
    public int maxRage = 200;
    public int currentRage;

    // スライダー
    public Slider rageSlider;

    // スライダーの子オブジェクト（背景と前景）
    public Image background; // 背景用イメージ
    public Image fill;       // 前景用イメージ

    

    void Start()
    {
        

   
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            int damage = Random.Range(1, 100);
            Debug.Log("damage : " + damage);
            currentRage -= damage;
            currentRage = Mathf.Clamp(currentRage, 0, maxRage); // HPが0未満にならないように

            // HPバーを更新
            UpdateRage();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            currentRage -= 10;
            currentRage = Mathf.Clamp(currentRage, 0, maxRage);
            Debug.Log("After K key, currentHp : " + currentRage);
            UpdateRage();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            currentRage += 10;
            currentRage = Mathf.Clamp(currentRage, 0, maxRage);
            Debug.Log("After L key, currentHp : " + currentRage);
            UpdateRage();
        }
    }

    // HPバーを更新するメソッド
    private void UpdateRage()
    {
        // HPバーのスライダーの値を更新
        float normalizedValue = (float)currentRage / maxRage;
        rageSlider.value = normalizedValue;

        // 前景のサイズを調整
        fill.fillAmount = normalizedValue; // 子オブジェクトの前景のサイズを調整
        Debug.Log("Rage: " + currentRage + " / " + maxRage + ", Fill Amount: " + fill.fillAmount);
    }

   
}
