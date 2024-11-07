using System.Collections;
using UnityEngine;

public class TargetRagdollSpawner : MonoBehaviour
{
    StageManagerBase stageManager;
    [SerializeField] Transform ragdollPrefab;
    [SerializeField] Transform originalRootBone;
    SkinnedMeshRenderer originalSkinnedMeshRenderer;
    [SerializeField] Cinemachine.CinemachineVirtualCamera vcam;

    private bool hasSpawnedRagdoll = false; // ラグドールが生成済みかのフラグ

    void Awake()
    {
        stageManager = FindObjectOfType<StageManagerBase>();
        originalSkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!hasSpawnedRagdoll && other.gameObject.CompareTag("Arrow"))
        {
            Arrow arrow = other.gameObject.GetComponent<Arrow>(); // 矢を取得
            SpawnRagdoll(arrow, other.GetContact(0).thisCollider.transform);
            Destroy(gameObject); // ターゲットを削除
            hasSpawnedRagdoll = true; // ラグドールを生成したフラグを設定
        }
    }

    void SpawnRagdoll(Arrow arrow, Transform hitTransform)
    {
        // ラグドールを生成
        Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        TargetRagdoll unitRagdoll = ragdollTransform.GetComponent<TargetRagdoll>();
        unitRagdoll.Setup(originalRootBone, originalSkinnedMeshRenderer);

        // 最下層の骨のTransformを取得
        Transform bone = FindDeepestBone(ragdollTransform, hitTransform.name);

        // 矢をラグドールの部位に配置
        if (arrow != null && bone != null)
        {
            arrow.StickToRagdoll(bone); // 最下層の骨を渡す
        }

        // ゲームクリア時のカメラ追尾対象に設定
        stageManager.vcamTarget = unitRagdoll.gameObject;
    }

    // 名前でラグドール内の一致する骨を見つけ、最も深い階層のTransformを返す
    Transform FindDeepestBone(Transform root, string targetName)
    {
        foreach (Transform child in root.GetComponentsInChildren<Transform>())
        {
            if (child.name == targetName) return child;
        }
        return root; // 見つからない場合はrootを返す
    }

}
