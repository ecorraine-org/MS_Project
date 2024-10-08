using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnomatopoeiaHandler : MonoBehaviour
{
    [SerializeField, Header("オノマトペデータ")]
    protected OnomatoData onomatoData;

    [Header("生成情報")]
    [Tooltip("最大生成数")]
    public int iMaxParticles = 5;
    [Tooltip("生成数/毎秒")]
    public float fEmissionRate = 1;
    [Tooltip("生命周期")]
    public float fParticleLifetime = 5f;
    [Tooltip("初期速度")]
    public float fStartSpeed = 5f;
    [Tooltip("生成方向")]
    public Vector3 emissionDirection = Vector3.up;
    [Tooltip("オノマトペ")]
    public GameObject particlePrefab;
    [Tooltip("飛び出す停止時間")]
    public float stopTime = 1f;
    [Tooltip("最大距離")]
    public float stopDistance = 2f;
    private List<Onomatopoeia> particles = new List<Onomatopoeia>();  // 生成されたオノマトペを格納する配列
    private float emissionTimer = 0;

    private class Onomatopoeia
    {
        public GameObject gameObject;
        public Vector3 fVelocity;
        public float fLifetime;
        public bool isMoving = true;
        public Vector3 initialPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        emissionTimer += Time.deltaTime;

        int particlesToEmit = Mathf.FloorToInt(fEmissionRate * emissionTimer);
        emissionTimer -= particlesToEmit / fEmissionRate;

        for (int i = 0; i < particlesToEmit; i++)
        {
            if (particles.Count < iMaxParticles)
            {
                EmitParticle();
            }
        }

        UpdateParticles();
    }

    void EmitParticle()
    {
        // 新しいパーティクルとして生成
        Onomatopoeia newParticle = new Onomatopoeia();
        newParticle.gameObject = Instantiate(particlePrefab, transform.position, Quaternion.identity);
        newParticle.fVelocity = RandomizeVelocity(emissionDirection * fStartSpeed);
        newParticle.fLifetime = fParticleLifetime;

        particles.Add(newParticle);
    }

    void UpdateParticles()
    {
        for (int i = particles.Count - 1; i >= 0; i--)
        {
            Onomatopoeia p = particles[i];

            // 飛び出す
            if (p.isMoving)
            {
                p.gameObject.transform.position += p.fVelocity * Time.deltaTime;

                // 一定時間経過後、飛び出す停止
                if (p.fLifetime <= (fParticleLifetime - stopTime))
                {
                    p.isMoving = false;
                }

                // 一定距離離れたら、飛び出す停止
                float distanceTraveled = Vector3.Distance(p.initialPosition, p.gameObject.transform.position);
                if (distanceTraveled >= stopDistance)
                {
                    p.isMoving = false;
                }
            }

            p.fLifetime -= Time.deltaTime;

            if (p.fLifetime <= 0)
            {
                Destroy(p.gameObject);
                particles.RemoveAt(i);
            }
        }
    }

    Vector3 RandomizeVelocity(Vector3 baseVelocity)
    {
        // 速度に乱数
        return baseVelocity + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }

}
