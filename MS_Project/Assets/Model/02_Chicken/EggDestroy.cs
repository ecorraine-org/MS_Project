using UnityEngine;

public class ThrownEgg : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // "Player" タグのオブジェクトに接触した場合に削除
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
