using UnityEngine;

public interface IAttack
{
   // AttackInfo ExecuteAttack(Vector3 position, Quaternion rotation);
    //bool CanAttack();

    /// <summary>
    /// 攻撃側の処理
    /// </summary>
    void Attack(Collider _hitCollider);

    /// <summary>
    ///  ヒットリアクションの情報を取得、受け側へ渡す準備をする
    /// </summary>
    AttackerParams GetAttackerParams();
}