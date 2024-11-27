using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIModeChange : MonoBehaviour
{
    private PlayerController player;

    [SerializeField, Header("プレイヤー武器アイコン"), Tooltip("プレイヤー武器アイコン")]
    Sprite[] weaponIcons;

    [SerializeField, Header("プレイヤー暴走中武器アイコン"), Tooltip("プレイヤー暴走中武器アイコン")]
    Sprite[] onRageIcons;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        // PlayerRageのRageが最大になった時のみImageを切り替える
        if (player.StatusManager.FrenzyTimer > 0 && player.StatusManager.IsFrenzy)
        {
            SwitchRageWeaponIcon();
            this.gameObject.GetComponent<Image>().sprite = weaponIcons[0];
        }
        else
        {
            SwitchWeaponIcon();
            this.gameObject.GetComponent<Image>().sprite = weaponIcons[0];
        }
    }

    /// <summary>
    /// 武器アイコンを切替
    /// </summary>
    private void SwitchWeaponIcon()
    {
        Sprite newIcon = null;
        UnityEngine.Color newColor = new UnityEngine.Color(1,1,1,1);

        switch (player.ModeManager.Mode)
        {
            case PlayerMode.Sword:
                //newIcon = onRageIcons[((int)PlayerMode.Sword)];
                newColor = new UnityEngine.Color(0,1,0,1);
                break;
            case PlayerMode.Hammer:
                //newIcon = onRageIcons[((int)PlayerMode.Hammer)];
                newColor = new UnityEngine.Color(0,0,1,1);
                break;
            case PlayerMode.Spear:
                //newIcon = onRageIcons[((int)PlayerMode.Spear)];
                newColor = new UnityEngine.Color(1,0,0,1);
                break;
            case PlayerMode.Gauntlet:
                //newIcon = onRageIcons[((int)PlayerMode.Gauntlet)];
                newColor = new UnityEngine.Color(1,1,0,1);
                break;
        }

        //this.gameObject.GetComponent<Image>().sprite = newIcon;
        this.gameObject.GetComponent<Image>().color = newColor;
    }

    /// <summary>
    /// 暴走中武器アイコンを切替
    /// </summary>
    private void SwitchRageWeaponIcon()
    {
        Sprite newIcon = null;
        UnityEngine.Color newColor = new UnityEngine.Color(1,1,1,1);

        switch (player.ModeManager.Mode)
        {
            case PlayerMode.Sword:
                //newIcon = weaponIcons[((int)PlayerMode.Sword)];
                newColor = new UnityEngine.Color(0,1,0,1);
                break;
            case PlayerMode.Hammer:
                //newIcon = weaponIcons[((int)PlayerMode.Hammer)];
                newColor = new UnityEngine.Color(0,0,1,1);
                break;
            case PlayerMode.Spear:
                //newIcon = weaponIcons[((int)PlayerMode.Spear)];
                newColor = new UnityEngine.Color(1,0,0,1);
                break;
            case PlayerMode.Gauntlet:
                //newIcon = weaponIcons[((int)PlayerMode.Gauntlet)];
                newColor = new UnityEngine.Color(1,1,0,1);
                break;
        }

        //this.gameObject.GetComponent<Image>().sprite = newIcon;
        this.gameObject.GetComponent<Image>().color = newColor;
    }

}
