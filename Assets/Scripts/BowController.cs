using UnityEngine;

public class BowController : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;
    [SerializeField] private float shootForce = 100f;

    private const int NoPointer = int.MinValue;

    private string setAnimName;
    private GameObject currentArrow;
    private Animator anim;
    private StageManagerBase stageManager;
    private bool isAiming;
    private int activePointerId = NoPointer;

    private void Start()
    {
        anim = GetComponent<Animator>();
        stageManager = FindObjectOfType<StageManagerBase>();
    }

    private void Update()
    {
        if (stageManager == null)
        {
            return;
        }

        if (stageManager.bowCount <= 0 || stageManager.isGameEnded)
        {
            if (isAiming)
            {
                CancelAim();
            }

            return;
        }

        if (activePointerId == NoPointer)
        {
            TryBeginAim();
            return;
        }

        if (activePointerId == PointerInputUtility.MousePointerId)
        {
            HandleMouseRelease();
        }
        else
        {
            HandleTouchRelease();
        }
    }

    private void TryBeginAim()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && !PointerInputUtility.IsPointerOverUI(touch.fingerId))
            {
                activePointerId = touch.fingerId;
                AimBow();
            }

            return;
        }

        if (Input.GetMouseButtonDown(0) && !PointerInputUtility.IsPointerOverUI(PointerInputUtility.MousePointerId))
        {
            activePointerId = PointerInputUtility.MousePointerId;
            AimBow();
        }
    }

    private void HandleTouchRelease()
    {
        if (!PointerInputUtility.TryGetTouch(activePointerId, out Touch touch))
        {
            CancelAim();
            return;
        }

        if (touch.phase == TouchPhase.Ended)
        {
            ShootArrow();
        }
        else if (touch.phase == TouchPhase.Canceled)
        {
            CancelAim();
        }
    }

    private void HandleMouseRelease()
    {
        if (Input.GetMouseButtonUp(0))
        {
            ShootArrow();
        }
        else if (!Input.GetMouseButton(0))
        {
            CancelAim();
        }
    }

    private void AimBow()
    {
        if (arrowPrefab == null || arrowSpawnPoint == null)
        {
            Debug.LogError("BowController is missing the arrow prefab or spawn point.");
            activePointerId = NoPointer;
            return;
        }

        SetAiming(true);

        currentArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        currentArrow.transform.SetParent(arrowSpawnPoint, true);

        if (anim != null)
        {
            anim.SetBool("isAiming", true);
        }
    }

    private void ShootArrow()
    {
        activePointerId = NoPointer;

        if (currentArrow == null)
        {
            CancelAim();
            return;
        }

        Rigidbody rb = currentArrow.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody is missing on currentArrow.");
            Destroy(currentArrow);
            currentArrow = null;
            CancelAim();
            return;
        }

        rb.isKinematic = false;
        rb.AddForce(arrowSpawnPoint.forward * shootForce, ForceMode.Impulse);
        currentArrow.transform.SetParent(null, true);
        currentArrow = null;

        if (anim != null)
        {
            anim.SetBool("isAiming", false);
            if (!string.IsNullOrEmpty(setAnimName))
            {
                anim.SetTrigger(setAnimName);
            }
        }

        SetAiming(false);
        stageManager.OnArrowShot();
    }

    private void CancelAim()
    {
        activePointerId = NoPointer;

        if (currentArrow != null)
        {
            Destroy(currentArrow);
            currentArrow = null;
        }

        if (anim != null)
        {
            anim.SetBool("isAiming", false);
        }

        SetAiming(false);
    }

    private void SetAiming(bool value)
    {
        isAiming = value;
        if (stageManager != null)
        {
            stageManager.isAiming = value;
        }
    }

    private void OnDisable()
    {
        if (isAiming || currentArrow != null)
        {
            CancelAim();
        }
    }

    public void SetVerySmallShot()
    {
        shootForce = 10f;
        setAnimName = "SmallShot";
    }

    public void SetSmallShot()
    {
        shootForce = 30f;
        setAnimName = "SmallShot";
    }

    public void SetMiddleShot()
    {
        shootForce = 50f;
        setAnimName = "MiddleShot";
    }

    public void SetFullShot()
    {
        shootForce = 80f;
        setAnimName = "FullShot";
    }
}
