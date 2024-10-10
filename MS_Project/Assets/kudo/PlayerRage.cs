using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRage : MonoBehaviour
{
    public int maxRage = 200;
    public int currentRage;

    // �X���C�_�[
    public Slider rageSlider;

    // �X���C�_�[�̎q�I�u�W�F�N�g�i�w�i�ƑO�i�j
    public Image background; // �w�i�p�C���[�W
    public Image fill;       // �O�i�p�C���[�W

    

    void Start()
    {
        

   
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            int damage = Random.Range(1, 100);
            Debug.Log("damage : " + damage);
            currentRage -= damage;
            currentRage = Mathf.Clamp(currentRage, 0, maxRage); // HP��0�����ɂȂ�Ȃ��悤��

            // HP�o�[���X�V
            UpdateRage();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            currentRage -= 10;
            currentRage = Mathf.Clamp(currentRage, 0, maxRage);
            Debug.Log("After K key, currentHp : " + currentRage);
            UpdateRage();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            currentRage += 10;
            currentRage = Mathf.Clamp(currentRage, 0, maxRage);
            Debug.Log("After L key, currentHp : " + currentRage);
            UpdateRage();
        }
    }

    // HP�o�[���X�V���郁�\�b�h
    private void UpdateRage()
    {
        // HP�o�[�̃X���C�_�[�̒l���X�V
        float normalizedValue = (float)currentRage / maxRage;
        rageSlider.value = normalizedValue;

        // �O�i�̃T�C�Y�𒲐�
        fill.fillAmount = normalizedValue; // �q�I�u�W�F�N�g�̑O�i�̃T�C�Y�𒲐�
        Debug.Log("Rage: " + currentRage + " / " + maxRage + ", Fill Amount: " + fill.fillAmount);
    }

   
}
