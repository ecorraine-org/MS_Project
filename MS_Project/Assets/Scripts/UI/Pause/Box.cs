using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField,Header("�ړ����x")]
    Vector3 speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ���̈ړ�������timeScale=0�Œ�~����
        transform.position += speed * Time.deltaTime;
    }
}
