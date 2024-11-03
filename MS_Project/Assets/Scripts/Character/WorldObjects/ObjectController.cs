using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class ObjectController : MonoBehaviour
{
    [HideInInspector]
    protected Transform player;

    [ReadOnly, Tooltip("生成されたオブジェクト")]
    public GameObject gameObj;

    [SerializeField, Tooltip("攻撃しているか？")]
    private bool canAttack = true;

    [SerializeField, Tooltip("攻撃されているか？")]
    private bool isDamaged = false;

    [HideInInspector, Tooltip("生成するオノマトペオブジェクト")]
    protected GameObject onomatoObj;

    [HideInInspector, Tooltip("ステータスマネージャー")]
    public ObjectStatusHandler status;

    // Start is called before the first frame update
    public virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (!this.transform.GetChild(0).gameObject.TryGetComponent<ObjectStatusHandler>(out status))
            CustomLogger.LogWarning(status.GetType(), status.name);
    }

    public virtual void Start()
    {
        onomatoObj = Resources.Load<GameObject>("Onomatopoeia/OnomatoItem");
    }

    protected void GenerateOnomatopoeia()
    {
        GameObject onomatoCollector = GameObject.FindGameObjectWithTag("OnomatopoeiaCollector").gameObject;
        onomatoObj.GetComponent<OnomatopoeiaController>().data = status.StatusData.onomatoData;
        onomatoObj.GetComponent<OnomatopoeiaController>().onomatopoeiaName = status.StatusData.onomatoData.wordToUse;

        Transform mainCamera = Camera.main.transform;
        // カメラと同じ角度
        Quaternion newRotation = mainCamera.rotation;
        newRotation = newRotation * Quaternion.Euler(0, 0, -90.0f);

        GameObject instance = Instantiate(onomatoObj, this.transform.position, newRotation, onomatoCollector.transform);
    }

    public bool CanAttack
    {
        get => canAttack;
        set { canAttack = value; }
    }

    public bool IsDamaged
    {
        get => isDamaged;
        set { isDamaged = value; }
    }
}
