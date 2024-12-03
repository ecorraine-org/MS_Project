using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField,Header("ˆÚ“®‘¬“x")]
    Vector3 speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Ÿ‚ÌˆÚ“®ˆ—‚ÍtimeScale=0‚Å’â~‚·‚é
        transform.position += speed * Time.deltaTime;
    }
}
