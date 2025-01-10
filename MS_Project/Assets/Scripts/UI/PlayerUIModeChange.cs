using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIModeChange : MonoBehaviour
{
    private PlayerController player;

    [SerializeField]
    private Image iconHolder;

    [SerializeField, Header("プレイヤー武器アイコンまたはプレハブ"), Tooltip("武器モードに対応するスプライトまたはプレハブ")]
    private Object[] weaponIconsOrPrefabs; // SpriteまたはGameObjectを受け付ける

    [SerializeField, Header("プレイヤー暴走中武器アイコンまたはプレハブ"), Tooltip("暴走モードに対応するスプライトまたはプレハブ")]
    private Object[] onRageIconsOrPrefabs; // SpriteまたはGameObjectを受け付ける

    private GameObject currentUIElement; // 現在のUIオブジェクト（Spriteまたはプレハブ）

    private void Start()
    {
        iconHolder = this.GetComponent<Image>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        SwitchWeaponIconOrPrefab();
    }

    private void Update()
    {
        if (player.StatusManager.FrenzyTimer > 0 && player.StatusManager.IsFrenzy)
        {
            SwitchRageWeaponIconOrPrefab();
        }
        else
        {
            SwitchWeaponIconOrPrefab();
        }
    }

    /// <summary>
    /// 武器アイコンまたはプレハブを切替
    /// </summary>
    private void SwitchWeaponIconOrPrefab()
    {
        UpdateUI(weaponIconsOrPrefabs);
    }

    /// <summary>
    /// 暴走中の武器アイコンまたはプレハブを切替
    /// </summary>
    private void SwitchRageWeaponIconOrPrefab()
    {
        UpdateUI(onRageIconsOrPrefabs);
    }

    /// <summary>
    /// UI要素を更新する（スプライトまたはプレハブに対応）
    /// </summary>
    /// <param name="iconsOrPrefabs">スプライトまたはプレハブの配列</param>
    private void UpdateUI(Object[] iconsOrPrefabs)
    {
        int modeIndex = (int)player.ModeManager.Mode;

        if (iconsOrPrefabs != null && iconsOrPrefabs.Length > modeIndex)
        {
            // 既存のUI要素を削除
            if (currentUIElement != null)
            {
                Destroy(currentUIElement);
            }

            // 新しい要素をスプライトまたはプレハブから生成
            Object iconOrPrefab = iconsOrPrefabs[modeIndex];
            if (iconOrPrefab is Sprite)
            {
                // スプライトの場合はImageコンポーネントを更新
                if (iconHolder != null)
                {
                    iconHolder.sprite = iconOrPrefab as Sprite;
                    iconHolder.enabled = true;
                }
            }
            else if (iconOrPrefab is GameObject prefab)
            {
                // プレハブの場合はインスタンス化
                GameObject newElement = Instantiate(prefab, this.transform);
                currentUIElement = newElement;

                // Imageコンポーネントを非表示（プレハブ表示を優先）
                Image imageComponent = this.gameObject.GetComponent<Image>();
                if (imageComponent != null)
                {
                    imageComponent.enabled = false;
                }
            }
        }
    }
}
