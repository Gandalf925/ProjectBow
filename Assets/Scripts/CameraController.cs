using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 0.2f;

    public float minVerticalAngle = -50f;
    public float maxVerticalAngle = 50f;
    public float minHorizontalAngle = -30f;
    public float maxHorizontalAngle = 30f;

    private const int NoPointer = int.MinValue;

    private int activePointerId = NoPointer;
    private Vector2 previousPointerPosition;
    private Vector3 currentRotation;

    private void Start()
    {
        currentRotation = transform.eulerAngles;
    }

    private void Update()
    {
        if (activePointerId == NoPointer)
        {
            TryBeginPointer();
            return;
        }

        if (activePointerId == PointerInputUtility.MousePointerId)
        {
            HandleMousePointer();
        }
        else
        {
            HandleTouchPointer();
        }
    }

    private void TryBeginPointer()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && !PointerInputUtility.IsPointerOverUI(touch.fingerId))
            {
                activePointerId = touch.fingerId;
                previousPointerPosition = touch.position;
            }

            return;
        }

        if (Input.GetMouseButtonDown(0) && !PointerInputUtility.IsPointerOverUI(PointerInputUtility.MousePointerId))
        {
            activePointerId = PointerInputUtility.MousePointerId;
            previousPointerPosition = Input.mousePosition;
        }
    }

    private void HandleTouchPointer()
    {
        if (!PointerInputUtility.TryGetTouch(activePointerId, out Touch touch))
        {
            EndPointer();
            return;
        }

        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            EndPointer();
            return;
        }

        RotateCamera(touch.position);
    }

    private void HandleMousePointer()
    {
        if (Input.GetMouseButtonUp(0) || !Input.GetMouseButton(0))
        {
            EndPointer();
            return;
        }

        RotateCamera(Input.mousePosition);
    }

    private void RotateCamera(Vector2 pointerPosition)
    {
        Vector2 pointerDelta = pointerPosition - previousPointerPosition;
        float resolutionScale = 1080f / Mathf.Max(Screen.height, 1);

        currentRotation.x -= pointerDelta.y * rotationSpeed * resolutionScale;
        currentRotation.y += pointerDelta.x * rotationSpeed * resolutionScale;

        currentRotation.x = Mathf.Clamp(currentRotation.x, minVerticalAngle, maxVerticalAngle);
        currentRotation.y = Mathf.Clamp(currentRotation.y, minHorizontalAngle, maxHorizontalAngle);

        transform.eulerAngles = currentRotation;
        previousPointerPosition = pointerPosition;
    }

    private void EndPointer()
    {
        activePointerId = NoPointer;
    }

    private void OnDisable()
    {
        EndPointer();
    }
}
