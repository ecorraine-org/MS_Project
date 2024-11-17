using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;  // AudioSource��ǉ�
    public AudioClip[] soundEffects; // �Đ�����T�E���h�G�t�F�N�g�̃��X�g

    // �A�j���[�V�����C�x���g�ŌĂяo��
    public void PlaySound(int index)
    {
        //
        if (index >= 0 && index < soundEffects.Length)
        {
            audioSource.PlayOneShot(soundEffects[index]);
        }
        else
        {
            Debug.LogWarning("�Đ�����Ă܂���: " + index);
        }
    }
}
