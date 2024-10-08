using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private TargetRagdoll targetRagdoll;
    [SerializeField] StageManagerBase stageManager;

    private void Start()
    {
        targetRagdoll = GetComponent<TargetRagdoll>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            // 衝突したオブジェクトが"Head"という名前か確認
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.thisCollider.gameObject.name == "Head")
                {
                    // Headに当たったときの処理
                    Vector3 hitPoint = contact.point;
                    Vector3 hitDirection = collision.relativeVelocity.normalized;
                    float forceMagnitude = collision.relativeVelocity.magnitude * collision.rigidbody.mass;

                    targetRagdoll?.Hit(hitPoint, hitDirection, forceMagnitude);
                    Debug.Log("Head hit! Game Cleared.");
                    return; // 他の部位のチェックをスキップ
                }
                else
                {
                    // 他の部位に当たった場合の処理
                    Debug.Log("Body hit!");
                    Vector3 hitPoint = contact.point;
                    Vector3 hitDirection = collision.relativeVelocity.normalized;
                    float forceMagnitude = collision.relativeVelocity.magnitude * collision.rigidbody.mass;

                    targetRagdoll?.Hit(hitPoint, hitDirection, forceMagnitude);
                    Debug.Log("Head hit! Game Cleared.");
                }
            }
        }
    }

    private void OnDestroy()
    {
        // ターゲットが破壊される際にStageManagerから削除
        stageManager.RemoveTarget(this);
    }
}
