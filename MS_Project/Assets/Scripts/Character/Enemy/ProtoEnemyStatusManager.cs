using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�̃X�e�[�^�X���Ǘ�����r�w�C�r�A
/// </summary>
public class ProtoEnemyStatusManager : StatusManager
{

    protected override void Awake()
    {
        base.Awake();

    }

    public new EnemyStatusData StatusData
    {
        get => (EnemyStatusData)statusData;
    }
}
