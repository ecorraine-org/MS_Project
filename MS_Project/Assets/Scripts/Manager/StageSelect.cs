using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect : MonoBehaviour
{
    public List<RectTransform> icons; // UI�A�C�R���̃��X�g
    public float radius = 200f;       // �����O�̔��a
    public float depth = 50f;         // ���s���̐[��
    public float rotationSpeed = 100f; // ��]�̃X�s�[�h
    private int currentIndex = 0;     // �����ɕ\�������A�C�R���̃C���f�b�N�X

    void Start()
    {
        ArrangeIconsInCylinder();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            RotateRight();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            RotateLeft();
        }
        HighlightCenterIcon();
    }

    // �A�C�R�����~���`�ɔz�u����֐�
    void ArrangeIconsInCylinder()
    {
        int iconCount = icons.Count;
        for (int i = 0; i < iconCount; i++)
        {
            float angle = i * Mathf.PI * 2 / iconCount;
            Vector3 position = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * depth, Mathf.Sin(angle) * radius);
            icons[i].localPosition = position;
            icons[i].localScale = Vector3.one; // �S�A�C�R���������T�C�Y��
        }
    }

    // �A�C�R�����E�ɉ�]������
    void RotateRight()
    {
        currentIndex = (currentIndex + 1) % icons.Count;
        StartCoroutine(RotateRing(-1));
    }

    // �A�C�R�������ɉ�]������
    void RotateLeft()
    {
        currentIndex = (currentIndex - 1 + icons.Count) % icons.Count;
        StartCoroutine(RotateRing(1));
    }

    // �����O����]������R���[�`��
    IEnumerator RotateRing(int direction)
    {
        float elapsedTime = 0f;
        float duration = 0.3f; // ��]�ɂ����鎞��

        while (elapsedTime < duration)
        {
            float angleStep = rotationSpeed * direction * Time.deltaTime;
            transform.Rotate(0, angleStep, 0); // Y���ŉ�]�����ĉ��s���̌��ʂ��o��
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ArrangeIconsInCylinder(); // �ʒu���X�V
    }

    // �����̃A�C�R�����n�C���C�g�\������֐�
    void HighlightCenterIcon()
    {
        for (int i = 0; i < icons.Count; i++)
        {
            if (i == currentIndex)
            {
                icons[i].localScale = Vector3.one * 1.5f; // �����A�C�R�����g��
            }
            else
            {
                icons[i].localScale = Vector3.one; // ���̑��̃A�C�R�������̃T�C�Y��
            }
        }
    }
}