using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class OnomatopoeiaController : MonoBehaviour
{
    [HideInInspector, Header("単体オノマトペ情報")]
    public GameObject objOnomatopoeia;
    [SerializeField, Tooltip("オノマトペデータファイル")]
    private OnomatopoeiaData data;
    [Tooltip("オノマトペ名")]
    public string onomatopoeiaName;
    [Tooltip("生きているか？")]
    public bool isAlive = true;
    [Tooltip("生命周期")]
    public float fLifetime = 5.0f;
    [HideInInspector, Tooltip("動いているか？")]
    private bool isMoving = true;
    [HideInInspector, Tooltip("移動方向")]
    public Vector3 emissionDirection = Vector3.up;
    [SerializeField, Tooltip("速度")]
    public float fStartSpeed = 5.0f;
    private Vector3 fVelocity = Vector3.zero;
    [HideInInspector, Tooltip("初期位置")]
    private Vector3 initialPosition;
    [SerializeField, Tooltip("最大距離")]
    public float fStopDistance = 5.0f;

    [SerializeField, Tooltip("オーナー")]
    private GameObject owningObject;

    private Rigidbody rb;
    private GameObject player;

    private ObjectCollector collector;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        initialPosition = this.transform.position - player.GetComponent<PlayerController>().CurDirecVector * 1.5f;
        initialPosition = new Vector3(initialPosition.x, initialPosition.y / 1.5f, this.transform.position.z);

        collector = GameObject.FindGameObjectWithTag("GarbageCollector").gameObject.GetComponent<ObjectCollector>();

        objOnomatopoeia = this.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        //CustomLogger.Log(data.wordToUse);

        if (data.onomatoSprite && data.onomatoACont)
        {
            if (this.transform.GetChild(1).gameObject.activeInHierarchy)
                this.transform.GetChild(1).gameObject.SetActive(false);

            GetComponentInChildren<SpriteRenderer>().sprite = data.onomatoSprite;
            GetComponentInChildren<Animator>().runtimeAnimatorController = data.onomatoACont;
            this.transform.GetChild(0).gameObject.transform.localScale = new Vector3(data.spriteSize, data.spriteSize, 1.0f);
        }
        else
        {
            GetComponentInChildren<TextMeshPro>().font = data.fontAsset;
            GetComponentInChildren<TextMeshPro>().color = new Color32(data.fontColor.r, data.fontColor.g, data.fontColor.b, data.fontColor.a);
            GetComponentInChildren<TextMeshPro>().outlineWidth = 0.1f;
            GetComponentInChildren<TextMeshPro>().outlineColor = new Color32(255, 255, 255, 255);

            GetComponentInChildren<TextMeshPro>().text = "<rotate=90>" + onomatopoeiaName;
        }

        fVelocity = RandomizeVelocity(emissionDirection * fStartSpeed);
    }

    void Update()
    {
        CustomLogger.Log(OwningObject.name);
        if (!isAlive)
        {
            OwningObject.GetComponent<WorldObject>().onomatoPool.Remove(objOnomatopoeia);
            collector.DestroyOtherObjectFromPool(objOnomatopoeia);
        }
        else
        {
            UpdateParticle();
        }
    }

    void UpdateParticle()
    {
        if (isMoving)
        {
            objOnomatopoeia.transform.position += fVelocity * Time.deltaTime;

            //一定距離離れたら、飛び出す停止
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


    #region Getter & Setter
    
    public OnomatopoeiaData Data
    {
        get => data;
        set { data = value; }
    }

    public GameObject OwningObject
    {
        get => owningObject;
        set { owningObject = value; }
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmos()
    {
        Collider collider = GetComponent<Collider>();
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, collider.bounds.extents);
    }

    #endregion
}
