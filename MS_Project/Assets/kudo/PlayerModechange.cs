using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerModeChange : MonoBehaviour
{
    private PlayerController player;

    // 切り替えたいImageを持つGameObjectの配列
    public GameObject[] images; // 新しくアクティブにするImage
    public GameObject[] targetImages; // 現在表示されているImage

    public Sprite[] icons;

    // PlayerRageコンポーネント
    public PlayerRage playerRage;

    // 切り替えが行われたかどうかのフラグ
    private bool hasChanged = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        // PlayerRageのRageが最大になった時のみImageを切り替える
        if (player.StatusManager.FrenzyTimer > 0 && player.StatusManager.IsFrenzy)
        {
            //SwitchImages();
            this.gameObject.GetComponent<Image>().sprite = icons[1];
            hasChanged = true; // 切り替えが行われたのでフラグを更新
        }
        else
        {
            this.gameObject.GetComponent<Image>().sprite = icons[0];
        }
    }

    // 画像を切り替えるメソッド
    private void SwitchImages()
    {
        // targetImagesを非アクティブにする
        foreach (GameObject target in targetImages)
        {
            if (target != null)
            {
                target.SetActive(false);
            }
        }

        // 配列内の新しいImageをアクティブにする
        foreach (GameObject image in images)
        {
            if (image != null)
            {
                image.SetActive(true);
            }
        }

        Debug.Log("Rage maxed out: Target images have been deactivated, and new images have been activated.");
    }

    // Rageが変化したときにフラグをリセットするメソッド
    public void ResetChangeFlag()
    {
        hasChanged = false;
    }
}
