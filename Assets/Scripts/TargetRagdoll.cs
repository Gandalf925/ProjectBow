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

        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        ApplyExplosionToRagdoll(ragdollRootBone, 300f, randomDir, 10f);
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

    private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }
            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
