using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActivater : MonoBehaviour
{
    public bool isActivated = false;
    bool completed = false;

    void Update()
    {
        if (isActivated)
        {
            if (!completed)
            {
                ActivateObject activeObject = GetComponent<ActivateObject>();
                StartCoroutine(activeObject.Activate());
                completed = true;
            }
        }
    }
}
