using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�G�t�F�N�g�Ǘ�
/// </summary>
public class PlayerEffectManager : MonoBehaviour
{
    [SerializeField, Header("�o�t�G�t�F�N�g�f�[�^")]
    PlayerEffectData buffEffectData;

    //�G�t�F�N�g�f�[�^���i�[����ϐ�
    List<PlayerEffectParam> buffEffects = new List<PlayerEffectParam>();

    /// <summary>
    /// �o�t�G�t�F�N�g����
    /// </summary>
    ///     //�d�l�ɂ���ĕς��
    public void GenerateBuffEffect(int _index)
    {
        GameObject effectInstance;
        // �t�F�N�g�𐶐�
        effectInstance = Instantiate(buffEffects[_index].effectL, transform.TransformPoint(buffEffects[_index].position), transform.rotation * Quaternion.Euler(buffEffects[_index].rotation), buffEffects[_index].isFollow ? transform : null);

        ParticleManager particle = effectInstance.GetComponent<ParticleManager>();
        if (particle != null)
        {
            particle.ChangeScale(buffEffects[_index].scale);
            particle.ChangePlaybackSpeed(buffEffects[_index].speed);
            particle.SetStartSize(buffEffects[_index].startSize);

        }


    }

    //�d�l�ɂ���ĕς��
    //�Ăѓ����o�t���������A�p�����Ԃ������Z�b�g���邩�A���ʂ�ݐς��邩
    public void SetbuffEffects(int _index, PlayerEffect _effectType)
    {

        while (buffEffects.Count <= _index)
        {
            //�v�f�ǉ�
            buffEffects.Add(new PlayerEffectParam());
        }

        buffEffects[_index] = buffEffectData.dicEffect[_effectType];

    }


    public List<PlayerEffectParam> BuffEffects
    {
        get => buffEffects;
    }
}
