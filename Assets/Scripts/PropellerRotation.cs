using UnityEngine;

public class PropellerRotation : MonoBehaviour
{
    public float rotationSpeed = 1000f; // 回転速度 (度/秒)

    void Update()
    {

        // Y軸方向に回転
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
