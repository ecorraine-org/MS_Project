using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBlade : SpriteBullet,IAttack
{
    ParticleManager particleManager;

    [SerializeField, NonEditable, Header("アタックコライダーマネージャー")]
    AttackColliderManagerV3 attackColliderV3;

    [SerializeField, Header("エフェクトスケール")]
    float windBladeScale = 0.5f;



    private void Awake()
    {
        attackColliderV3 = GetComponentInChildren<AttackColliderManagerV3>();

        if (attackColliderV3 != null)
        {
            attackColliderV3.Damage = attackDamage;

            // LayerMask からレイヤー番号を取得して設定
            attackColliderV3.gameObject.layer = (int)Mathf.Log(targetLayer.value, 2);
        }
        else Debug.LogError("attackCollider NULL");

        if ((targetLayer.value & (targetLayer.value - 1)) != 0)
        {
            Debug.LogError("targetLayer に複数のレイヤーが含まれています。1つのレイヤーのみを指定してください！");
        }

    }

    public override void Init(bool _isFlipX, AttackerParams _attackerParams)
    {
        base.Init(_isFlipX, _attackerParams);

        spriteRenderer.flipX = !_isFlipX;
        //親オブジェクトの向きによる反転処理
        Vector3 currentEulerAngles = transform.rotation.eulerAngles;
        if (_isFlipX)
        {
            transform.rotation = Quaternion.Euler(currentEulerAngles.x, -currentEulerAngles.y, currentEulerAngles.z);
        }

        float randomZRotation = Random.Range(-70f, 70f);
        //ランダム回転
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, randomZRotation);


        //パーティクル処理
        particleManager = GetComponentInChildren<ParticleManager>();
        particleManager.ChangeScale(windBladeScale);

    }

    private void Update()
    {

        //画面内かをチェック
        CheckCamera();

        //当たると消す
        if (attackColliderV3.HasCollided)
        {
            Destroy(gameObject);
        }
    }

    public void Attack(Collider _hitCollider)
    {
        
    }

    public AttackerParams GetAttackerParams()
    {
        return attackerParams;
    }
}
