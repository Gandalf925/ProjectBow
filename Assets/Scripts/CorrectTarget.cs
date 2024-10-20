using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectTarget : MonoBehaviour
{
    [SerializeField] private StageManagerBase stageManager;
    private Collider coll;
    void Start()
    {
        coll = GetComponent<Collider>();
        stageManager = FindObjectOfType<StageManagerBase>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            stageManager.isHitCorrectTarget = true;
            stageManager.vcamTarget = this.gameObject;
        }
    }
}
