using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// プレイヤーのモードを管理するビヘイビア
/// </summary>
public class PlayerModeManager : MonoBehaviour
{
    //PlayerControllerの参照
    PlayerController playerController;

    [SerializeField, Header("モード")]
    PlayerMode mode = PlayerMode.Sword;

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

    public void Init(PlayerController _playerController)
    {
        playerController = _playerController;

    }

    /// <summary>
    /// モードチェンジ
    /// </summary>
    private void ModeChange(PlayerMode _mode)
    {
        mode = _mode;
        Debug.Log("Manager: モードチェンジ処理" + mode);
    }

    public PlayerMode Mode
    {
        get => this.mode;
        set { this.mode = value; }
    }
}