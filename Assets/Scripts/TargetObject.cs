using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObject : MonoBehaviour
{
    [SerializeField] StageManagerBase stageManager;

    private void Start()
    {
        stageManager = FindObjectOfType<StageManagerBase>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                HandleHit();
            }
        }
    }

    private void HandleHit()
    {
        // stageManager.vcamTarget = this.gameObject;
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
