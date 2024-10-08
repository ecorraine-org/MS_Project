using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    Vector3 fVelocity = Vector3.zero;
    [HideInInspector, Tooltip("初期位置")]
    private Vector3 initialPosition;

    public OnomatoData data;

    // Start is called before the first frame update
    void Start()
    {
        objOnomatopoeia = this.gameObject;
        onomatopoeiaName = GetComponent<TextMeshPro>().text.ToString();
    }

    void Update()
    {
        UpdateParticle();
    }

    void UpdateParticle()
    {
        if (isAlive)
        {
            if (isMoving)
            {
                objOnomatopoeia.transform.position += fVelocity * Time.deltaTime;
            }

            fLifetime -= Time.deltaTime;
            if (fLifetime <= 0)
            {
                isAlive = false;
            }
        }
        else if (!isAlive)
        {
            Destroy(objOnomatopoeia);
        }
    }

    public OnomatoData Data {
        get => data;
    }
}
