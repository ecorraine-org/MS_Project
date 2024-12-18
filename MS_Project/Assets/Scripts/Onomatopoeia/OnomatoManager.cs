using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OnomatoManager : MonoBehaviour, IHit,ISelected
{
    [SerializeField]
    private OnomatopoeiaController controller;

    //  モードチェンジのイベント
    public delegate void OnomatoEventHandler(PlayerMode mode,string onomatoName);
    public static event OnomatoEventHandler OnModeChangeEvent;

    //  食べたオノマトペのデータを渡すイベント
    public delegate void OnEatOmatoEvtHandler(OnomatopoeiaData _data);
    public static event OnEatOmatoEvtHandler OnEatOnomatoEvt;

    // 暴走ゲージを溜めるイベント
    public delegate void FrenzyEventHandler(float amount);
    public static event FrenzyEventHandler OnIncreaseFrenzyEvent;

    [HideInInspector]
    private PlayerController player;
    private PlayerMode currentMode;
    private OnomatoType nextDataType;
    private OnomatoType currentDataType;

    Vector3 defaultScale;

    /*
    private void OnEnable()
    {
        //イベントをバインドする
        AttackColliderManager.OnOnomatoEvent += Absorb;
    }

    private void OnDisable()
    {
        //バインドを解除する
        AttackColliderManager.OnOnomatoEvent -= Absorb;
    }

    /// <summary>
    /// 食べられる処理
    /// </summary>
    public void Absorb()
    {
        controller.isAlive = false;

        Debug.Log("OnomatoManager:イベントを受信、モードチェンジ" + transform.position);
        //モードチェンジのイベント送信
        OnModeChangeEvent?.Invoke(PlayerMode.Hammer);

        //暴走ゲージを溜めるイベント送信
        OnIncreaseFrenzyEvent?.Invoke(5.0f);

    }
     */

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        currentMode = player.ModeManager.Mode;

        defaultScale = transform.localScale;
    }

    public void ReceiveHitData(AttackerParams _attackParams)
    { 
    }

    /// <summary>
    /// 被撃処理
    /// </summary>
    public void Hit(bool _canOneHitKill)
    {
        controller.isAlive = false;

        nextDataType = controller.Data.type;

        Debug.Log("OnomatoManager:イベントを受信、モードチェンジ" + transform.position);
        //モードチェンジのイベント送信
        ChangeMode(nextDataType, controller.Data.wordToUse);

        //暴走ゲージを溜めるイベント送信
        OnIncreaseFrenzyEvent?.Invoke(5.0f);

        //オノマトペのデータを渡すイベント送信
        OnEatOnomatoEvt?.Invoke(controller.Data);
    }

    public static void ChangeMode(OnomatoType _datatype,string name)
    {
        string nextMode = "";
        switch (_datatype)
        {
            case OnomatoType.None:
                break;
            case OnomatoType.SlashType:
                nextMode = PlayerMode.Sword.ToString();
                OnModeChangeEvent?.Invoke(PlayerMode.Sword, name);
                break;
            case OnomatoType.SmashType:
                nextMode = PlayerMode.Hammer.ToString();
                OnModeChangeEvent?.Invoke(PlayerMode.Hammer, name);
                break;
            case OnomatoType.PierceType:
                nextMode = PlayerMode.Spear.ToString();
                OnModeChangeEvent?.Invoke(PlayerMode.Spear, name);
                break;
            case OnomatoType.PunchType:
                nextMode = PlayerMode.Gauntlet.ToString();
                OnModeChangeEvent?.Invoke(PlayerMode.Gauntlet, name);
                break;
            case OnomatoType.OtherType:
                break;
            default:
                break;
        }

        CustomLogger.Log("Change Mode to: " + nextMode);
    }

    /// <summary>
    /// 捕食で選択された時の処理
    /// </summary>
    public void Selected()
    {
        // 仮で大きくする
        transform.localScale = 2 * defaultScale;

    }

    /// <summary>
    /// 捕食で選択されてない時の処理
    /// </summary>
    public void UnSelected()
    {
        transform.localScale = defaultScale;
    }

    public OnomatoType NextDataType
    {
        get => this.nextDataType;
        set { this.nextDataType = value; }
    }
}
