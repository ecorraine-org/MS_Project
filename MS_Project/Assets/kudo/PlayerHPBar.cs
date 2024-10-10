using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    public int maxHp = 200;
    public int currentHp;

    // �X���C�_�[
    public Slider hpSlider;

    // �X���C�_�[�̎q�I�u�W�F�N�g�i�w�i�ƑO�i�j
    public Image background; // �w�i�p�C���[�W
    public Image fill;       // �O�i�p�C���[�W

    // �w�iHP�����������邽�߂̃R���[�`��
    //private Coroutine decreaseBackgroundHpCoroutine;

    void Start()
    {
        // �X���C�_�[�𖞃^���ɂ���
        currentHp = maxHp;
        UpdateHPBar();
        Debug.Log("Start currentHp : " + currentHp);

        // �w�iHP������������R���[�`������ɊJ�n
        //decreaseBackgroundHpCoroutine = StartCoroutine(DecreaseBackgroundHp());
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            int damage = Random.Range(1, 100);
            Debug.Log("damage : " + damage);
            currentHp -= damage;
            currentHp = Mathf.Clamp(currentHp, 0, maxHp); // HP��0�����ɂȂ�Ȃ��悤��

            // HP�o�[���X�V
            UpdateHPBar();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            currentHp -= 10;
            currentHp = Mathf.Clamp(currentHp, 0, maxHp);
            Debug.Log("After I key, currentHp : " + currentHp);
            UpdateHPBar();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            currentHp += 10;
            currentHp = Mathf.Clamp(currentHp, 0, maxHp);
            Debug.Log("After O key, currentHp : " + currentHp);
            UpdateHPBar();
        }
    }

    // HP�o�[���X�V���郁�\�b�h
    private void UpdateHPBar()
    {
        // HP�o�[�̃X���C�_�[�̒l���X�V
        float normalizedValue = (float)currentHp / maxHp;
        hpSlider.value = normalizedValue;

        // �O�i�̃T�C�Y�𒲐�
        fill.fillAmount = normalizedValue; // �q�I�u�W�F�N�g�̑O�i�̃T�C�Y�𒲐�
        Debug.Log("HP: " + currentHp + " / " + maxHp + ", Fill Amount: " + fill.fillAmount);
    }

    // �w�iHP��O�iHP�܂Ō���������R���[�`��
    /*private IEnumerator DecreaseBackgroundHp()
    {
        while (true)
        {
            // �O�i��HP�Ɋ�Â��Ĕw�i��HP������
            float targetValue = (float)currentHp / maxHp;

            // �w�i��HP��O�i��HP�܂�1������
            if (background.fillAmount > targetValue)
            {
                background.fillAmount -= 0.01f; // 0.01�������i�����\�j
            }
            else if (background.fillAmount < targetValue)
            {
                background.fillAmount += 0.01f; // 0.01�������i�����\�j
            }

            yield return new WaitForSeconds(0.01f); // ���̎��ԑҋ@�i�����\�j
        }
    }
    */
}
