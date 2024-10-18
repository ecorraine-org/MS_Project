using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class EnemyElite_Controller : MonoBehaviour
{

    public float minInterval = 0f; // アニメーション発動の最小間隔（秒）使わない可能性
    public float maxInterval = 0f; // アニメーション発動の最大間隔（秒）
    public string[] animationTriggers; // 発動可能なアニメーションのトリガー名の配列

    private List<GameObject> projectiles = new List<GameObject>(); // 現在の投擲物リスト

    public float approachSpeed = 1f; // プレイヤーに近づく速度
    public Transform player; // Inspectorでプレイヤーを設定するためのTransform
    public float followDistance = 1f; // プレイヤーとの最低距離

    private Rigidbody rb;
    private Animator animator;

    private float nextAnimationTime; // 次のアニメーションを再生する時間



    void Start()
    {
        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();

        // 最初のアニメーション発動のスケジュール
        //ScheduleNextAnimation();
    }



    void Update()
    {
        // プレイヤーが存在する場合にのみ方向を向き、追従する
        if (player != null)
        {
            // プレイヤーとの距離を計算
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // 一定の距離より遠い場合のみ追従する
            if (distanceToPlayer > followDistance)
            {
                Vector3 direction = (player.position - transform.position).normalized;

                // プレイヤーの方向に回転（y軸だけ）
                transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

                // プレイヤーに近づく（approachSpeedを使って速度を調整）
                transform.position += direction * approachSpeed * Time.deltaTime;
            }
        }

        // ランダムアニメーションの再生タイミングを確認
        if (Time.time >= nextAnimationTime)
        {
            PlayRandomAnimation();
            ScheduleNextAnimation();
        }

        // ランダムでアニメーションを再生する
        void PlayRandomAnimation()
        {
            if (animationTriggers.Length > 0)
            {
                // ランダムなインデックスを取得
                int randomIndex = Random.Range(0, animationTriggers.Length);
                string randomTrigger = animationTriggers[randomIndex];

                // ランダムなトリガーでアニメーションを再生
                animator.SetTrigger(randomTrigger);
            }
        }

        // 次のアニメーションを再生する時間をスケジュール
        void ScheduleNextAnimation()
        {
            // minIntervalとmaxIntervalの間でランダムな時間を設定
            float interval = Random.Range(minInterval, maxInterval);
            nextAnimationTime = Time.time + interval;
        }

    }

}
