using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObject : MonoBehaviour
{
    [SerializeField] StageManagerBase stageManager;
    [SerializeField] GameObject targetFragment;

    private void Start()
    {
        stageManager = FindObjectOfType<StageManagerBase>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            HandleHit();
        }
    }

    private void HandleHit()
    {
        GameObject vcamTarget = Instantiate(targetFragment, transform.position, transform.rotation);
        stageManager.vcamTarget = vcamTarget;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // ターゲットオブジェクトが破壊される際にStageManagerから削除
        if (stageManager != null)
        {
            stageManager.RemoveTarget(this);
        }
    }
}
