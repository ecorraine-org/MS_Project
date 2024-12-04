using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main; 
    }



    void Update()
    {
    
        Vector3 targetPosition = mainCamera.transform.position;
        targetPosition.z = transform.position.z; 

        transform.LookAt(targetPosition);

     
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);


        this.transform.SetAsLastSibling();
    }
}
