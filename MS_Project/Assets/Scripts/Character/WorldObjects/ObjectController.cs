using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public enum ObjectStateType
{
    [InspectorName("待機")]Idle,
    [InspectorName("移動")]Walk,
    [InspectorName("攻撃")]Attack,
    [InspectorName("被ダメージ")]Damaged,
    [InspectorName("破棄")]Destroyed
}

public abstract class ObjectController : MonoBehaviour
{
    [HideInInspector]
    protected Transform player;
    
    [ReadOnly, Tooltip("生成されたオブジェクト")]
    public GameObject gameObj;
    
    [HideInInspector, Tooltip("生成するオノマトペオブジェクト")]
    protected GameObject onomatoObj;

    [HideInInspector, Tooltip("ステータスマネージャー")]
    public ObjectStatusHandler status;

    // Start is called before the first frame update
    public virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if(!this.transform.GetChild(0).gameObject.TryGetComponent<ObjectStatusHandler>(out status))
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
        GameObject instance = Instantiate(onomatoObj, this.transform.position, Quaternion.identity, onomatoCollector.transform);
    }
}
