using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private TargetRagdoll targetRagdoll;
    [SerializeField] private StageManagerBase stageManager;

    private void Start()
    {
        targetRagdoll = GetComponent<TargetRagdoll>();
        stageManager = FindObjectOfType<StageManagerBase>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            if (stageManager.clearConditionType == ClearConditionType.HitSpecificPart)
            {
                // 衝突した各コンタクトポイントを確認
                foreach (ContactPoint contact in collision.contacts)
                {
                    // StageManagerBaseのspecificPartNameと比較してチェック
                    if (contact.thisCollider.gameObject.name == stageManager.specificPartName)
                    {
                        // 特定の部位に当たったときの処理
                        Vector3 hitPoint = contact.point;
                        Vector3 hitDirection = collision.relativeVelocity.normalized;
                        float forceMagnitude = collision.relativeVelocity.magnitude * collision.rigidbody.mass;

                        targetRagdoll?.Hit(hitPoint, hitDirection, forceMagnitude);
                        stageManager?.HitSpecificPart();
                        return; // 他の部位のチェックをスキップ
                    }
                }
            }
            else if (stageManager.clearConditionType == ClearConditionType.WeakPointOnly)
            {
                // 衝突した各コンタクトポイントを確認
                foreach (ContactPoint contact in collision.contacts)
                {
                    Debug.Log("Hit Weak Point" + contact.thisCollider.gameObject.name);
                    // StageManagerBaseのspecificPartNameと比較してチェック
                    if (contact.thisCollider.gameObject.name == stageManager.specificPartName)
                    {

                        // 特定の部位に当たったときの処理
                        Vector3 hitPoint = contact.point;
                        Vector3 hitDirection = collision.relativeVelocity.normalized;
                        float forceMagnitude = collision.relativeVelocity.magnitude * collision.rigidbody.mass;

                        targetRagdoll?.Hit(hitPoint, hitDirection, forceMagnitude);
                        stageManager?.HitSpecificPart();
                        return; // 他の部位のチェックをスキップ
                    }
                    else
                    {
                        // 特定の部位以外に当たったときの処理
                        Vector3 hitPoint = contact.point;
                        Vector3 hitDirection = collision.relativeVelocity.normalized;
                        float forceMagnitude = collision.relativeVelocity.magnitude * collision.rigidbody.mass;

                        targetRagdoll?.Hit(hitPoint, hitDirection, forceMagnitude);
                        stageManager?.HitNonWeakPoint();
                        return; // 他の部位のチェックをスキップ
                    }
                }
            }
            else
            {
                // 衝突した各コンタクトポイントを確認
                foreach (ContactPoint contact in collision.contacts)
                {
                    {
                        // 特定の部位以外に当たったときの処理
                        Vector3 hitPoint = contact.point;
                        Vector3 hitDirection = collision.relativeVelocity.normalized;
                        float forceMagnitude = collision.relativeVelocity.magnitude * collision.rigidbody.mass;

                        targetRagdoll?.Hit(hitPoint, hitDirection, forceMagnitude);
                        return; // 他の部位のチェックをスキップ
                    }
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
