using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 敵のステータスを管理するビヘイビア
/// </summary>
public class EnemyStatusHandler : StatusManager
{
    [Tooltip("攻撃されたか？")]
    private bool isDamaged = false;

    [Tooltip("生きているか？")]
    private bool isAlive = true;

    //フィニッシャーで殺せるか
    [SerializeField,NonEditable,Tooltip("殺せるか")]
    private bool isKillable = false;

    [SerializeField, Tooltip("速度")]
    private float moveSpeed;

    [SerializeField, Tooltip("攻撃力")]
    private float damage;

    [SerializeField, Tooltip("行動クールタイム")]
    private float actionCooldown;

    [SerializeField, Tooltip("自分の属性")]
    private OnomatoType selfType;

    [SerializeField, Tooltip("オノマトペデータ")]
    private OnomatopoeiaData onomatoData;

    [SerializeField, Tooltip("耐性")]
    private OnomatoType[] tolerances;
    //private OnomatoType[] tolerances;

    [SerializeField, Tooltip("弱点")]
    private OnomatoType[] weaknesses;
    //private OnomatoType[] weakness;

    EnemyController enemy;

    EnemyStatusData enemyStatusData;

    public void Init(EnemyController _enemyController)
    {
        enemy = _enemyController;
    }

    protected override void Awake()
    {
        //base.Awake();
    }

    protected virtual void Start()
    {
        if (!StatusData)
        {
            CustomLogger.Log("No status data found. Instantiating from new.");
            StatusData = (EnemyStatusData)base.StatusData;
        }
        else
        {
            CustomLogger.Log("Found " + StatusData.ToString() + "\nInstantiating...");
        }

        statusData = enemyStatusData;
        currentHealth = enemyStatusData.maxHealth;
        isInvincible = enemyStatusData.isInvincible;
        moveSpeed = enemyStatusData.velocity;
        damage = enemyStatusData.damage;
        actionCooldown = enemyStatusData.timeTillNextAction;
        selfType = enemyStatusData.SelfType;
        onomatoData = enemyStatusData.onomatoAttack;
        tolerances = enemyStatusData.tolerances;
        weaknesses = enemyStatusData.weaknesses;
    }

    public override void TakeDamage(float _damage)
    {
        isDamaged = true;

        //耐性処理(単一)
        //if (enemy.CurReceiverParams.onomatoType == weakness)
        //{
        //    Debug.Log("受けたのは "   + enemy.CurReceiverParams.onomatoType + ", 弱点 "+ weakness);
        //    _damage *= 2;
        //}
        //if (enemy.CurReceiverParams.onomatoType == tolerance)
        //{
        //    Debug.Log("受けたのは " + enemy.CurReceiverParams.onomatoType + ", 耐性 " + tolerance);
        //    _damage /= 2;
        //}

        //耐性処理(複数)
        if (weaknesses.Contains(enemy.CurReceiverParams.onomatoType))
        {
            Debug.Log("受けたのは " + enemy.CurReceiverParams.onomatoType + ", 弱点 " + string.Join(", ", weaknesses));
            _damage *= 2;
        }
        if (tolerances.Contains(enemy.CurReceiverParams.onomatoType))
        {
            Debug.Log("受けたのは " + enemy.CurReceiverParams.onomatoType + ", 耐性 " + string.Join(", ", tolerances));
            _damage /= 2;
        }
        Debug.Log("最終ダメージ "+ _damage);

       base.TakeDamage(_damage);

        //一定hp以下になると、殺せる状態になる
        if (currentHealth <= enemyStatusData.maxHealth * enemyStatusData.killableHealthRate)
        {
            isKillable = true;

            //UIを見えるように
            enemy.UIManager.FinishIcon.SetActive(true);

       
        }
   
    }

    public new EnemyStatusData StatusData
    {
        get => (EnemyStatusData)enemyStatusData;
        set { enemyStatusData = value; }
    }

    #region Getter & Setter

    public float MoveSpeed
    {
        get => moveSpeed;
        set { moveSpeed = value; }
    }

    public float Damage
    {
        get => damage;
        set { damage = value; }
    }

    public bool IsKillable
    {
        get => isKillable;
    //    set { isKillable = value; }
    }

    public float ActionCooldown
    {
        get => actionCooldown;
        set { actionCooldown = value; }
    }

    public bool IsDamaged
    {
        get => isDamaged;
        set { isDamaged = value; }
    }

    public bool IsAlive
    {
        get => isAlive;
        set { isAlive = value; }
    }

    #endregion
}
