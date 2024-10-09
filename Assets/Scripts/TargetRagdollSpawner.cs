using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRagdollSpawner : MonoBehaviour
{
    [SerializeField] Transform ragdollPrefab;
    [SerializeField] Transform originalRootBone;
    SkinnedMeshRenderer originalSkinnedMeshRenderer;
    Collider[] colliders;

    void Awake()
    {
        originalSkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        colliders = GetComponentsInChildren<Collider>();
    }

    private void Start()
    {
        originalSkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Arrow")
        {
            SpawnRagdoll();
        }
    }

    void SpawnRagdoll()
    {
        Destroy(gameObject);
        Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        TargetRagdoll unitRagdoll = ragdollTransform.GetComponent<TargetRagdoll>();
        unitRagdoll.Setup(originalRootBone, originalSkinnedMeshRenderer);
    }
}
