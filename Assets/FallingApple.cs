using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FallingApple : MonoBehaviour
{
    [SerializeField] private StageManagerBase stageManager;
    [SerializeField] private LookAtCamera lookAtCamera;
    [SerializeField] private GameObject targetVcam;
    [SerializeField] private GameObject afterStageClearPrefab;
    [SerializeField] private GameObject targetPositionMarker;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        stageManager = FindObjectOfType<StageManagerBase>();

        if (afterStageClearPrefab != null)
        {
            afterStageClearPrefab.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(Activate(collision));
    }

    private IEnumerator Activate(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            rb.isKinematic = false;
            stageManager.vcamTarget = targetVcam;

            yield return new WaitForSeconds(0.5f);

            lookAtCamera.isActivated = true;
            stageManager.RemoveTarget(this);

            if (afterStageClearPrefab != null)
            {
                yield return new WaitForSeconds(5f);

                afterStageClearPrefab.SetActive(true);
            }
        }
    }
}
