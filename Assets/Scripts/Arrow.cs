using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 savedPosition;
    private Quaternion savedRotation;
    private bool isStuck = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isStuck && collision.gameObject.CompareTag("Target"))
        {
            StickToTarget(collision);
        }
    }

    private void StickToTarget(Collision collision)
    {
        // 矢が刺さった位置と回転を保存
        ContactPoint contact = collision.GetContact(0);
        savedPosition = contact.point;
        savedRotation = Quaternion.LookRotation(contact.normal * -1);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        isStuck = true;
    }

    // 刺さった位置と回転で矢を再配置
    public void StickToRagdoll(Transform newParent)
    {
        rb.collisionDetectionMode = CollisionDetectionMode.Discrete; // CCDを無効化

        // 矢の位置と回転を設定し、指定されたボーンを親に設定
        transform.position = savedPosition;
        transform.rotation = savedRotation;
        rb.isKinematic = true; // 物理演算を無効化
        GetComponent<Collider>().enabled = false; // コライダーを無効化

        transform.SetParent(newParent); // ラグドールの対応する最下層の部位に設定
    }
}
