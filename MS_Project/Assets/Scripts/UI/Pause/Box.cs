using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField,Header("移動速度")]
    Vector3 speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 次の移動処理はtimeScale=0で停止する
        transform.position += speed * Time.deltaTime;
    }
}
