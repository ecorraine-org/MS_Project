using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModeChange : MonoBehaviour
{
    // �؂�ւ�����Image������GameObject�̔z��
    public GameObject[] images; // �V�����A�N�e�B�u�ɂ���Image
    public GameObject[] targetImages; // ���ݕ\������Ă���Image

    // PlayerRage�R���|�[�l���g
    public PlayerRage playerRage;

    // �؂�ւ����s��ꂽ���ǂ����̃t���O
    private bool hasChanged = false;

    void Update()
    {
        // PlayerRage��Rage���ő�ɂȂ������̂�Image��؂�ւ���
        if (playerRage.currentRage == playerRage.maxRage && !hasChanged)
        {
            SwitchImages();
            hasChanged = true; // �؂�ւ����s��ꂽ�̂Ńt���O���X�V
        }
    }

    // �摜��؂�ւ��郁�\�b�h
    private void SwitchImages()
    {
        // targetImages���A�N�e�B�u�ɂ���
        foreach (GameObject target in targetImages)
        {
            if (target != null)
            {
                target.SetActive(false);
            }
        }

        // �z����̐V����Image���A�N�e�B�u�ɂ���
        foreach (GameObject image in images)
        {
            if (image != null)
            {
                image.SetActive(true);
            }
        }

        Debug.Log("Rage maxed out: Target images have been deactivated, and new images have been activated.");
    }

    // Rage���ω������Ƃ��Ƀt���O�����Z�b�g���郁�\�b�h
    public void ResetChangeFlag()
    {
        hasChanged = false;
    }
}
