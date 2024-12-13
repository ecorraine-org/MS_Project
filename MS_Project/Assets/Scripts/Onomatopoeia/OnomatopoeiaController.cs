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
            //-------------------------------
            // AudioSouceデバック
            Debug.Log($"OnomatopoeiaData: {data}");
            Debug.Log($"SE Clip: {data.onomatoSE}");

            // AudioSouceを追加して音をアタッチ
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // シーン内のAudioSourceをすべて取得して重複チェック
            AudioSource[] allAudioSources = Object.FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
            bool isClipAlreadyAssigned = false;

            foreach (AudioSource existingAudioSource in allAudioSources)
            {
                if (existingAudioSource.clip == data.onomatoSE)
                {
                    isClipAlreadyAssigned = true;
                    break;
                }
            }

            if (!isClipAlreadyAssigned)
            {
                // クリップを設定して再生
                audioSource.clip = data.onomatoSE;
                audioSource.playOnAwake = false;
                audioSource.Play();
            }
            else
            {
                //Debug.Log("同じSEがすでに存在");
            }
            //---------------------------------

        objOnomatopoeia.transform.position = RandomizePosition(objOnomatopoeia.transform.position);
    }

    void Update()
    {
        // CustomLogger.Log(OwningObject.name);
        if (!isAlive)
        {
            switch(OwningObject.GetComponent<WorldObjectController>().Type)
            {
                case WorldObjectType.Enemy:
                    OwningObject.GetComponent<WorldObject>().ParentSpawner.GetComponent<EnemySpawner>().enemyOnomatoPool.Remove(objOnomatopoeia);
                    break;
                case WorldObjectType.StaticObject:
                    OwningObject.GetComponent<WorldObject>().onomatoPool.Remove(objOnomatopoeia);
                    break;
            }
            collector.DestroyOtherObjectFromPool(objOnomatopoeia);
        }
        else
        {
            UpdateParticle();
        }
        
    }

    void UpdateParticle()
    {
        fLifetime -= Time.deltaTime;

        /*
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

        if (fLifetime < 1f)
        {
            bool hasRB = this.gameObject.TryGetComponent<Rigidbody>(out rb);
            if (!hasRB)
            {
                this.gameObject.AddComponent<Rigidbody>();
            }
        }
        */
        if (fLifetime <= 0)
        {
            isAlive = false;
        }
    }

    Vector3 RandomizePosition(Vector3 basePosition)
    {
        // 位置に乱数
        Vector3 finalPosition = basePosition;
        finalPosition += new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        return finalPosition;
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
