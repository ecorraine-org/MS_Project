using UnityEngine;

public interface IAttack
{
    AttackInfo ExecuteAttack(Vector3 position, Quaternion rotation);
    bool CanAttack();
}