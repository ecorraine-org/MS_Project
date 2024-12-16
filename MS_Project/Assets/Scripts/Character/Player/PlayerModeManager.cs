using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのモードを管理するビヘイビア
/// </summary>
public class PlayerModeManager : MonoBehaviour
{
    //モードチェンジイベント定義
    public delegate void ModelCHangeEvtHandler(PlayerMode _mode);
    public static event ModelCHangeEvtHandler OnModelCHange;

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

        playerController.BattleManager.CurPlayerMode = mode;

     //   playerController.SkillManager.SetCurSkill(mode);
    }

    /// <summary>
    /// モードチェンジ
    /// </summary>
    private void ModeChange(PlayerMode _mode)
    {
        //モード設定
        mode = _mode;
        playerController.BattleManager.CurPlayerMode = mode;

        //プレイヤーの体力を回復
        playerController.StatusManager.TakeDamage(-10);

        //スキル設定
        //  playerController.SkillManager.SetCurSkill(mode);

        TimerUtility.TimeBasedTimer(this, 0.5f, () =>
        {
            //モードチェンジイベント発信
            OnModelCHange?.Invoke(_mode);
        });


        //if (playerController.tutorialStage == TutorialStage.Step5)
        //{
        //    Time.timeScale = 0;
        //    //UI操作
        //    InputController.Instance.SetInputContext(InputController.InputContext.UI);

        //    playerController.tutorialStage = TutorialStage.Step6;

        //    //新しい会話(会話7:変身)
        //    TalkManager.Instance.LoadNextStory();
        //    TalkManager.Instance.ShowNextPrefab();
        //}
    }

    public PlayerMode Mode
    {
        get => this.mode;
        set { this.mode = value; }
    }
}
