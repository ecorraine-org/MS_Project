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

    //�G�t�F�N�g���i�[����
    GameObject speedBuffinstance;
    GameObject damageBuffinstance;
    GameObject healBuffinstance;

    //PlayerController�̎Q��
    PlayerController playerController;

    //�G�t�F�N�g�f�[�^���i�[����ϐ�
    // List<PlayerEffectParam> buffEffects = new List<PlayerEffectParam>();

    public void Init(PlayerController _playerController)
    {
        playerController = _playerController;

    }

    /// <summary>
    /// �o�t�G�t�F�N�g����
    /// </summary>
    public void GenerateDamageBuffEffect()
    {
        PlayerEffectParam curParam = buffEffectData.dicEffect[PlayerEffect.DamageBuff];

        // �t�F�N�g�𐶐�
        damageBuffinstance = Instantiate(curParam.effectL, transform.TransformPoint(curParam.position), transform.rotation * Quaternion.Euler(curParam.rotation), curParam.isFollow ? transform : null);

        ParticleManager particle = damageBuffinstance.GetComponent<ParticleManager>();
        if (particle != null)
        {
            particle.ChangeScale(curParam.scale);
            particle.ChangePlaybackSpeed(curParam.speed);
            particle.SetStartSize(curParam.startSize);
            particle.SetLoop(true);
        }
    }

    public void GenerateSpeedBuffEffect()
    {
        PlayerEffectParam curParam = buffEffectData.dicEffect[PlayerEffect.SpeedBuff];

        // �t�F�N�g�𐶐�
        speedBuffinstance = Instantiate(curParam.effectL, transform.TransformPoint(curParam.position)  , transform.rotation * Quaternion.Euler(curParam.rotation), curParam.isFollow ? transform : null);

        Debug.Log("�G�t�F�N�g����");

        ParticleManager particle = speedBuffinstance.GetComponent<ParticleManager>();
        if (particle != null)
        {
            particle.ChangeScale(curParam.scale);
            particle.ChangePlaybackSpeed(curParam.speed);
            particle.SetStartSize(curParam.startSize);
            particle.SetLoop(true);

        }
    }

    public void GenerateHealBuffEffect()
    {
        PlayerEffectParam curParam = buffEffectData.dicEffect[PlayerEffect.HealBuff];

        // �t�F�N�g�𐶐�
        healBuffinstance = Instantiate(curParam.effectL, transform.TransformPoint(curParam.position), transform.rotation * Quaternion.Euler(curParam.rotation), curParam.isFollow ? transform : null);

        ParticleManager particle = healBuffinstance.GetComponent<ParticleManager>();
        if (particle != null)
        {
            particle.ChangeScale(curParam.scale);
            particle.ChangePlaybackSpeed(curParam.speed);
            particle.SetStartSize(curParam.startSize);
            particle.SetLoop(true);
        }
    }

    public void DestroyHealBuffEffect()
    {
        if (!healBuffinstance) return;

        ParticleManager particle = healBuffinstance.GetComponent<ParticleManager>();
        if (particle != null)
        {
            particle.SetLoop(false);
        }
    }

    public void DestroyDamageBuffEffect()
    {
        if (!damageBuffinstance) return;

        ParticleManager particle = damageBuffinstance.GetComponent<ParticleManager>();
        if (particle != null)
        {
            particle.SetLoop(false);
        }
    }

    public void DestroySpeedBuffEffect()
    {
        if (!speedBuffinstance) return;

        ParticleManager particle = speedBuffinstance.GetComponent<ParticleManager>();
        if (particle != null)
        {
            particle.SetLoop(false);
        }
    }

    /// <summary>
    /// �o�t�G�t�F�N�g����
    /// </summary>
    ///     //�d�l�ɂ���ĕς��
    //public void GenerateBuffEffect(int _index)
    //{
    //    GameObject effectInstance;
    //    // �t�F�N�g�𐶐�
    //    effectInstance = Instantiate(buffEffects[_index].effectL, transform.TransformPoint(buffEffects[_index].position), transform.rotation * Quaternion.Euler(buffEffects[_index].rotation), buffEffects[_index].isFollow ? transform : null);

    //    ParticleManager particle = effectInstance.GetComponent<ParticleManager>();
    //    if (particle != null)
    //    {
    //        particle.ChangeScale(buffEffects[_index].scale);
    //        particle.ChangePlaybackSpeed(buffEffects[_index].speed);
    //        particle.SetStartSize(buffEffects[_index].startSize);

    //    }


    //}

    //�d�l�ɂ���ĕς��
    //�Ăѓ����o�t���������A�p�����Ԃ������Z�b�g���邩�A���ʂ�ݐς��邩
    //public void SetbuffEffects(int _index, PlayerEffect _effectType)
    //{

    //    while (buffEffects.Count <= _index)
    //    {
    //        //�v�f�ǉ�
    //        buffEffects.Add(new PlayerEffectParam());
    //    }

    //    buffEffects[_index] = buffEffectData.dicEffect[_effectType];

    //}


    //public List<PlayerEffectParam> BuffEffects
    //{
    //    get => buffEffects;
    //}
}
