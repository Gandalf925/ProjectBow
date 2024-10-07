using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 0.2f; // カメラの回転速度
    private Vector3 startMousePosition; // マウスの開始位置
    private bool isDragging = false; // プレイヤーがクリック中かどうか

    // 回転角度の制限値
    public float minVerticalAngle = -50f; // 上方向の制限
    public float maxVerticalAngle = 50f;  // 下方向の制限
    public float minHorizontalAngle = -30f; // 左方向の制限
    public float maxHorizontalAngle = 30f;  // 右方向の制限

    private Vector3 currentRotation; // 現在の回転角度

    void Start()
    {
        // 現在のカメラの回転角度を保存
        currentRotation = transform.eulerAngles;
    }

    void Update()
    {
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        // クリック開始時にマウス位置を保存
        if (Input.GetMouseButtonDown(0))
        {
            startMousePosition = Input.mousePosition;
            isDragging = true;
        }

        // クリック中にカメラを回転させる
        if (Input.GetMouseButton(0) && isDragging)
        {
            RotateCamera();
        }

        // クリックを離したらドラッグ状態を終了
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    void RotateCamera()
    {
        // 現在のマウス位置
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 mouseDelta = currentMousePosition - startMousePosition; // マウスの移動量

        // マウスの動きに基づいてカメラの回転角度を計算
        float rotationX = -mouseDelta.y * rotationSpeed; // 上下の回転
        float rotationY = mouseDelta.x * rotationSpeed;  // 左右の回転

        // 現在のカメラ回転角度に加算
        currentRotation.x += rotationX;
        currentRotation.y += rotationY;

        // 回転角度を制限（上下は±50度、左右は±30度）
        currentRotation.x = Mathf.Clamp(currentRotation.x, minVerticalAngle, maxVerticalAngle);
        currentRotation.y = Mathf.Clamp(currentRotation.y, minHorizontalAngle, maxHorizontalAngle);

        // カメラの回転角度を更新
        transform.eulerAngles = currentRotation;

        // 更新後のマウス位置を保存
        startMousePosition = currentMousePosition;
    }
}
