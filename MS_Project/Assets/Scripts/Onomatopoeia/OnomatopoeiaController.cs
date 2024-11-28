using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class OnomatopoeiaController : MonoBehaviour
{
    [HideInInspector, Header("単体オノマトペ情報")]
    public GameObject objOnomatopoeia;
    [Tooltip("オノマトペ名")]
    public string onomatopoeiaName;
    [Tooltip("生きているか？")]
    public bool isAlive = true;
    [Tooltip("生命周期")]
    public float fLifetime = 5f;
    [HideInInspector, Tooltip("動いているか？")]
    private bool isMoving = true;
    [HideInInspector, Tooltip("移動方向")]
    public Vector3 emissionDirection = Vector3.up;
    [SerializeField, Tooltip("速度")]
    public float fStartSpeed = 5f;
    private Vector3 fVelocity = Vector3.zero;
    [HideInInspector, Tooltip("初期位置")]
    private Vector3 initialPosition;
    [SerializeField, Tooltip("最大距離")]
    public float fStopDistance = 5f;

    public OnomatopoeiaData data;
    private Rigidbody rb;
    private GameObject player;

    private ObjectCollector collector;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        initialPosition = this.transform.position - player.GetComponent<PlayerController>().CurDirecVector * 1.5f;
        initialPosition = new Vector3(initialPosition.x, initialPosition.y / 1.5f, player.transform.position.z);

        collector = GameObject.FindGameObjectWithTag("GarbageCollector").gameObject.GetComponent<ObjectCollector>();

        objOnomatopoeia = this.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        //CustomLogger.Log(data.wordToUse);

        GetComponent<TextMeshPro>().font = data.fontAsset;
        GetComponent<TextMeshPro>().color = new Color32(data.fontColor.r, data.fontColor.g, data.fontColor.b, data.fontColor.a);
        GetComponent<TextMeshPro>().outlineWidth = 0.1f;
        GetComponent<TextMeshPro>().outlineColor = new Color32(255, 255, 255, 255);

        GetComponent<TextMeshPro>().text = "<rotate=90>" + onomatopoeiaName;

        fVelocity = RandomizeVelocity(emissionDirection * fStartSpeed);
    }

    void Update()
    {
        if (!isAlive)
        {
            collector.DestroyOtherObjectFromPool(objOnomatopoeia);
        }
        else
        {
            UpdateParticle();
        }

        if (Debug.isDebugBuild)
        {
            Debug.DrawRay(objOnomatopoeia.transform.position, objOnomatopoeia.transform.forward, Color.red);
        }
    }

    void UpdateParticle()
    {
        if (isMoving)
        {
            objOnomatopoeia.transform.position += fVelocity * Time.deltaTime;

            // 一定距離離れたら、飛び出す停止
            float distanceTraveled = Vector3.Distance(initialPosition, gameObject.transform.position);
            if (distanceTraveled >= fStopDistance)
            {
                isMoving = false;
            }
        }

        fLifetime -= Time.deltaTime;

        if (fLifetime < 1f)
        {
            bool hasRB = this.gameObject.TryGetComponent<Rigidbody>(out rb);
            if (!hasRB)
            {
                this.gameObject.AddComponent<Rigidbody>();
            }
        }
        if (fLifetime <= 0)
        {
            isAlive = false;
        }
    }

    Vector3 RandomizeVelocity(Vector3 baseVelocity)
    {
        // 速度に乱数
        return baseVelocity + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }

    public OnomatopoeiaData Data
    {
        get => data;
    }
}
