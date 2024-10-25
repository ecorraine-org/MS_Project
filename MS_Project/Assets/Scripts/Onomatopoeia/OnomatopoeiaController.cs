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

    private OnomatoManager onomatoManager;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        initialPosition = this.transform.position - player.GetComponent<PlayerController>().CurDirecVector * 1.5f;
        initialPosition = new Vector3(initialPosition.x, initialPosition.y / 1.5f, player.transform.position.z);

        onomatoManager = this.gameObject.GetComponent<OnomatoManager>();

        objOnomatopoeia = this.gameObject;
        objOnomatopoeia.transform.rotation = new Quaternion(0, 0, -90, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(data.wordToUse);

        GetComponent<TextMeshPro>().text = "<rotate=90>" + onomatopoeiaName;

        fVelocity = RandomizeVelocity(emissionDirection * fStartSpeed);
    }

    void Update()
    {
        FaceScreen(true);

        if (!isAlive)
        {
            Destroy(objOnomatopoeia);
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

    void FaceScreen(bool _value)
    {
        if (_value)
        {
            Vector3 NewDirection = objOnomatopoeia.transform.position - Camera.main.transform.position;
            // カメラと同じ方向に向く
            objOnomatopoeia.transform.LookAt(NewDirection);
            objOnomatopoeia.transform.Rotate(0, 0, -90);
        }
    }

    public OnomatopoeiaData Data
    {
        get => data;
    }
}
