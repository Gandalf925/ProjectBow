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
        if (!collision.gameObject.CompareTag("Arrow")) return;

        foreach (ContactPoint contact in collision.contacts)
        {
            string hitPartName = contact.thisCollider.gameObject.name;
            Vector3 hitPoint = contact.point;
            Vector3 hitDirection = collision.relativeVelocity.normalized;
            float forceMagnitude = collision.relativeVelocity.magnitude * collision.rigidbody.mass;

            ProcessHit(hitPartName, hitPoint, hitDirection, forceMagnitude);
            return;
        }
    }

    private void ProcessHit(string hitPartName, Vector3 hitPoint, Vector3 hitDirection, float forceMagnitude)
    {
        if (stageManager.clearConditionType == ClearConditionType.HitSpecificPart)
        {
            HandleSpecificPartHit(hitPartName, hitPoint, hitDirection, forceMagnitude);
        }
        else if (stageManager.clearConditionType == ClearConditionType.WeakPointOnly)
        {
            HandleWeakPointHit(hitPartName, hitPoint, hitDirection, forceMagnitude);
        }
        else
        {
            ApplyHit(hitPoint, hitDirection, forceMagnitude);
        }
    }

    private void HandleSpecificPartHit(string hitPartName, Vector3 hitPoint, Vector3 hitDirection, float forceMagnitude)
    {
        if (hitPartName == stageManager.stageData.specificPartName)
        {
            ApplyHit(hitPoint, hitDirection, forceMagnitude);
            stageManager?.HitSpecificPart();
        }
    }

    private void HandleWeakPointHit(string hitPartName, Vector3 hitPoint, Vector3 hitDirection, float forceMagnitude)
    {
        if (hitPartName == stageManager.stageData.specificPartName)
        {
            ApplyHit(hitPoint, hitDirection, forceMagnitude);
            stageManager?.HitSpecificPart();
        }
        else
        {
            ApplyHit(hitPoint, hitDirection, forceMagnitude);
            stageManager?.HitNonWeakPoint();
        }
    }

    private void ApplyHit(Vector3 hitPoint, Vector3 hitDirection, float forceMagnitude)
    {
        targetRagdoll?.Hit(hitPoint, hitDirection, forceMagnitude);
    }

    private void OnDestroy()
    {
        stageManager.RemoveTarget(this);
    }
}
