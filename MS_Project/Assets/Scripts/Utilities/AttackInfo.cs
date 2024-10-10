using UnityEngine;

public struct AttackInfo
{
    public Vector3 Position { get; }
    public Quaternion Rotation { get; }
    public Vector3 Size { get; }
    public float Damage { get; }
    public LayerMask TargetLayer { get; }

    public AttackInfo(Vector3 position, Quaternion rotation, Vector3 size, float damage, LayerMask targetLayer)
    {
        Position = position;
        Rotation = rotation;
        Size = size;
        Damage = damage;
        TargetLayer = targetLayer;
    }
}