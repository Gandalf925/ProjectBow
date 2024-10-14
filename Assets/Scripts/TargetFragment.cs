using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFragment : MonoBehaviour
{
    StageManagerBase stageManager;
    void Start()
    {
        stageManager = FindObjectOfType<StageManagerBase>();
        stageManager.vcamTarget = this.gameObject;
    }

}
