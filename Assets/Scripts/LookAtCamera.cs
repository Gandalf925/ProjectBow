using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform mainCameraTransform;
    public bool isActivated = false;

    private void Start()
    {
        // メインカメラのTransformを取得
        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogError("Main Camera is not found. Please ensure a camera is tagged as MainCamera.");
        }
    }

    private void Update()
    {
        if (mainCameraTransform != null)
        {
            if (isActivated)
            {
                // カメラの方向を向く
                Vector3 directionToCamera = mainCameraTransform.position - transform.position;
                transform.rotation = Quaternion.LookRotation(directionToCamera);
            }
        }
    }

}
