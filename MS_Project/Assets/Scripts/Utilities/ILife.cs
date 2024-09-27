using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HP�����C���^�t�F�[�X
/// </summary>
public interface ILife
{
    /// <summary>
    /// �_���[�W����
    /// </summary>
    void TakeDamage(float _damage);

    /// <summary>
    /// ���S����
    /// </summary>
    void OnDeath();

    float Health { get; set; }
}
