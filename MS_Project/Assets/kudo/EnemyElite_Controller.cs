using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class EnemyElite_Controller : MonoBehaviour
{

    public float minInterval = 0f; // �A�j���[�V���������̍ŏ��Ԋu�i�b�j�g��Ȃ��\��
    public float maxInterval = 0f; // �A�j���[�V���������̍ő�Ԋu�i�b�j
    public string[] animationTriggers; // �����\�ȃA�j���[�V�����̃g���K�[���̔z��

    private List<GameObject> projectiles = new List<GameObject>(); // ���݂̓��������X�g

    public float approachSpeed = 1f; // �v���C���[�ɋ߂Â����x
    public Transform player; // Inspector�Ńv���C���[��ݒ肷�邽�߂�Transform
    public float followDistance = 1f; // �v���C���[�Ƃ̍Œ዗��

    private Rigidbody rb;
    private Animator animator;

    private float nextAnimationTime; // ���̃A�j���[�V�������Đ����鎞��



    void Start()
    {
        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();

        // �ŏ��̃A�j���[�V���������̃X�P�W���[��
        //ScheduleNextAnimation();
    }



    void Update()
    {
        // �v���C���[�����݂���ꍇ�ɂ̂ݕ����������A�Ǐ]����
        if (player != null)
        {
            // �v���C���[�Ƃ̋������v�Z
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // ���̋�����艓���ꍇ�̂ݒǏ]����
            if (distanceToPlayer > followDistance)
            {
                Vector3 direction = (player.position - transform.position).normalized;

                // �v���C���[�̕����ɉ�]�iy�������j
                transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

                // �v���C���[�ɋ߂Â��iapproachSpeed���g���đ��x�𒲐��j
                transform.position += direction * approachSpeed * Time.deltaTime;
            }
        }

        // �����_���A�j���[�V�����̍Đ��^�C�~���O���m�F
        if (Time.time >= nextAnimationTime)
        {
            PlayRandomAnimation();
            ScheduleNextAnimation();
        }

        // �����_���ŃA�j���[�V�������Đ�����
        void PlayRandomAnimation()
        {
            if (animationTriggers.Length > 0)
            {
                // �����_���ȃC���f�b�N�X���擾
                int randomIndex = Random.Range(0, animationTriggers.Length);
                string randomTrigger = animationTriggers[randomIndex];

                // �����_���ȃg���K�[�ŃA�j���[�V�������Đ�
                animator.SetTrigger(randomTrigger);
            }
        }

        // ���̃A�j���[�V�������Đ����鎞�Ԃ��X�P�W���[��
        void ScheduleNextAnimation()
        {
            // minInterval��maxInterval�̊ԂŃ����_���Ȏ��Ԃ�ݒ�
            float interval = Random.Range(minInterval, maxInterval);
            nextAnimationTime = Time.time + interval;
        }

    }

}
