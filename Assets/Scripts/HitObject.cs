using UnityEngine;

public class HitObject : MonoBehaviour
{
    Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            Destroy(gameObject, 1f);
        }
    }
}
