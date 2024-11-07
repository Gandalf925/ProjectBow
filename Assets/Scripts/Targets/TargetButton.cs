using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetButton : MonoBehaviour
{
    [SerializeField] StageManagerBase stageManager;
    [SerializeField] ButtonActivater activateObject;

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
        activateObject.isActivated = true;
        stageManager.vcamTarget = activateObject.gameObject;
        stageManager.RemoveTarget(this);
    }
}
