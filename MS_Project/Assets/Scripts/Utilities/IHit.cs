using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �팂�����C���^�t�F�[�X
/// </summary>
public interface IHit
{
    /// <summary>
    /// �팂����
    /// </summary>
    /// <param name="canOneHitKill">���ڎE���邩</param>
    void Hit(bool _canOneHitKill);
}