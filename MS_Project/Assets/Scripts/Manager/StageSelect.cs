using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect : MonoBehaviour
{
    public List<RectTransform> icons; // �X�e�[�W�A�C�R���̃��X�g
    public float radius = 600f;       // �����O�̔��a
    public float depth = 100f;        // ���s���̐[��
    public float rotationDuration = 0.3f; // ��]�ɂ����鎞��
    public float backZoomScale = 0.5f; // �w�ʃA�C�R���̏k����
    private int currentIndex = 0;     // ���ݑI�𒆂̃A�C�R���̃C���f�b�N�X
    private bool isRotating = false;  // ��]���̏�ԊǗ�

    void Start()
    {
        ArrangeIconsInCylinder();
    }

    void Update()
    {
        // ��]�̓��͌��m
        if (Input.GetKeyDown(KeyCode.A) && !isRotating)
        {
            StartCoroutine(RotateRing(1)); // �E��]
        }
        else if (Input.GetKeyDown(KeyCode.D) && !isRotating)
        {
            StartCoroutine(RotateRing(-1)); // ����]
        }
    }

    // �A�C�R�����~����ɔz�u����
    void ArrangeIconsInCylinder()
    {
        int iconCount = icons.Count;

        for (int i = 0; i < iconCount; i++)
        {
            // �����A�C�R���� currentIndex �ɂȂ�悤�ɁA�c������Ԃɔz�u
            float angle = ((i - currentIndex) * Mathf.PI * 2 / iconCount);
            Vector3 position = new Vector3(Mathf.Sin(angle) * radius, Mathf.Sin(angle) * depth, Mathf.Cos(angle) * radius);
            icons[i].localPosition = position;

            // �ʒu�ɉ������X�P�[������
            float scale = Mathf.Lerp(1f, backZoomScale, Mathf.Abs(Mathf.Cos(angle)));
            icons[i].localScale = Vector3.one * scale;
        }

        // �����̃A�C�R���̃X�P�[��������
        icons[currentIndex].localScale = Vector3.one;
    }

    // �A�C�R�������O����]������
    IEnumerator RotateRing(int direction)
    {
        isRotating = true;
        float elapsedTime = 0f;
        int iconCount = icons.Count;

        // ��]��̐V�����C���f�b�N�X���v�Z
        int newCurrentIndex = (currentIndex + direction + iconCount) % iconCount;

        // ��]�A�j���[�V����
        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / rotationDuration);

            for (int i = 0; i < iconCount; i++)
            {
                // �V�����ʒu�̌v�Z
                float startAngle = ((i - currentIndex) * Mathf.PI * 2 / iconCount);
                float endAngle = ((i - newCurrentIndex) * Mathf.PI * 2 / iconCount);
                float angle = Mathf.Lerp(startAngle, endAngle, t);

                Vector3 position = new Vector3(Mathf.Sin(angle) * radius, Mathf.Sin(angle) * depth, Mathf.Cos(angle) * radius);
                icons[i].localPosition = position;

                // �e�A�C�R���̃X�P�[���𒲐�
                float scale = Mathf.Lerp(1f, backZoomScale, Mathf.Abs(Mathf.Cos(angle)));
                icons[i].localScale = Vector3.one * scale;
            }

            yield return null;
        }

        // ��]������������A�C���f�b�N�X���X�V
        currentIndex = newCurrentIndex;
        ArrangeIconsInCylinder(); // �A�C�R���ʒu�̍X�V
        isRotating = false;
    }
}
