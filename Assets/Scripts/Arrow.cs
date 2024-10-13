using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 savedPosition;
    private Quaternion savedRotation;
    private StageManagerBase stageManager;
    bool isEnd = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        stageManager = FindObjectOfType<StageManagerBase>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isEnd && collision.gameObject.CompareTag("Target"))
        {
            isEnd = true;
            stageManager.CountHitArrow();
            StickToTarget(collision);

        }
        else if (!isEnd && collision.gameObject.CompareTag("Out"))
        {
            // 「Out」に当たった場合にカウントを増やす
            isEnd = true;
            stageManager.CountOutArrow();
        }
    }

    private void StickToTarget(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);
        savedPosition = contact.point;
        savedRotation = Quaternion.LookRotation(contact.normal * -1);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;
    }

    public void StickToRagdoll(Transform newParent)
    {
        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
        transform.position = savedPosition;
        transform.rotation = savedRotation;
        rb.isKinematic = true;

        transform.SetParent(newParent);
    }
}
