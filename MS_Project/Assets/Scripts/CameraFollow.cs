using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("オフセット")]
    public Vector3 cameraOffset = new Vector3(0f, 1f, -10f);

    [Header("滑らか平均値")]
    [SerializeField, Range(0f, 1f)]
    public float blendFactor = 0.125f;
    private Transform playerPos;

    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = cameraOffset;
        playerPos = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPosition = playerPos.position + cameraOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, blendFactor);
        transform.position = smoothedPosition;

        transform.GetChild(0).transform.LookAt(playerPos);
    }
}
