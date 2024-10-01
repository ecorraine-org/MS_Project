using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurt : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        //‚Ô‚Â‚©‚Á‚½‚Æ‚«‚Ìˆ—
        if (collision.gameObject.CompareTag("Player")) {
            Destroy(this.gameObject);
        }
    }

}
