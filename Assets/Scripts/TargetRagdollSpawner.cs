using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        originalSkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!hasSpawnedRagdoll && other.gameObject.CompareTag("Arrow"))
        {
            Arrow arrow = other.gameObject.GetComponent<Arrow>(); // 矢を取得
            SpawnRagdoll(arrow);
            Destroy(gameObject); // ターゲットを削除
            hasSpawnedRagdoll = true; // ラグドールを生成したフラグを設定
        }
    }

    void SpawnRagdoll(Arrow arrow)
    {
        // ラグドールを生成し、矢を刺さった状態で再配置
        Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        TargetRagdoll unitRagdoll = ragdollTransform.GetComponent<TargetRagdoll>();
        unitRagdoll.Setup(originalRootBone, originalSkinnedMeshRenderer);

        // 矢をラグドールの部位に配置
        if (arrow != null)
        {
            arrow.StickToRagdoll(ragdollTransform);
        }

        stageManager.vcamTarget = unitRagdoll.gameObject; // ゲームクリア時のカメラ追尾対象に設定
    }
}