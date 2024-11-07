using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRagdoll : MonoBehaviour
{
    [SerializeField] Transform ragdollRootBone;
    [SerializeField] SkinnedMeshRenderer skin;

    private void Awake()
    {
        skin = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public void Setup(Transform originalRootBone, SkinnedMeshRenderer originalSkin)
    {
        MatchAllChildTransforms(originalRootBone, ragdollRootBone);
        skin.sharedMesh = originalSkin.sharedMesh;
    }

    void MatchAllChildTransforms(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;
                MatchAllChildTransforms(child, cloneChild);  // 再帰呼び出しはcloneChildがnullでない場合のみ行う
            }
        }
    }

    public void Hit(Vector3 hitPoint, Vector3 hitForceDirection, float forceMagnitude)
    {
        Vector3 explosionPosition = hitPoint;
        ApplyHitForceToRagdoll(ragdollRootBone, forceMagnitude, hitForceDirection, explosionPosition);
    }

    // 力をラグドールの特定の部位に適用するメソッド
    private void ApplyHitForceToRagdoll(Transform root, float forceMagnitude, Vector3 hitForceDirection, Vector3 hitPoint)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                // 力を衝突点と矢の方向に基づいて適用
                childRigidbody.AddForceAtPosition(hitForceDirection * forceMagnitude, hitPoint, ForceMode.Impulse);
            }
            ApplyHitForceToRagdoll(child, forceMagnitude, hitForceDirection, hitPoint);
        }
    }
}
