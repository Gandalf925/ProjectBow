using UnityEngine;
using UnityEngine.EventSystems;

public class BowController : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab; // 矢のプレハブ
    [SerializeField] private Transform arrowSpawnPoint; // 矢が発射される位置
    [SerializeField] private float shootForce = 100f; // 矢の飛ばす力（初期値）
    private string setAnimName;
    private GameObject currentArrow; // 現在の矢
    private Animator anim;
    private StageManagerBase stageManager;
    private bool isAiming;

    private void Start()
    {
        anim = GetComponent<Animator>();
        stageManager = FindObjectOfType<StageManagerBase>();
    }

    private void Update()
    {
        if (stageManager.bowCount <= 0 || stageManager.isGameEnded)
            return;

        // UIボタンを押している間はAimしない
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI() && !isAiming)
        {
            AimBow();
        }

        if (Input.GetMouseButtonUp(0) && !IsPointerOverUI() && isAiming)
        {
            ShootArrow();
        }
    }

    private bool IsPointerOverUI()
    {
        // マウスまたはタッチがUI要素上にあるかをチェック
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void AimBow()
    {
        IsAimingTrue();

        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation); // 矢を生成
        arrow.transform.SetParent(arrowSpawnPoint); // 矢を弓に取り付ける
        currentArrow = arrow;
        anim.SetBool("isAiming", true); // 弓のアニメーションを再生
    }

    private void ShootArrow()
    {
        if (currentArrow == null)
        {
            Debug.LogError("currentArrow is null in ShootArrow");
            return;
        }

        Rigidbody rb = currentArrow.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody is missing on currentArrow");
            return;
        }

        rb.isKinematic = false;
        rb.AddForce(arrowSpawnPoint.forward * shootForce, ForceMode.Impulse);

        currentArrow.transform.SetParent(null); // 矢を弓から離す
        anim.SetBool("isAiming", false);
        anim.SetTrigger(setAnimName);

        IsAimingFalse();

        stageManager.OnArrowShot();
    }

    private void IsAimingFalse()
    {
        isAiming = false;
        stageManager.isAiming = false;
    }

    private void IsAimingTrue()
    {
        isAiming = true;
        stageManager.isAiming = true;
    }

    // SmallShotの処理
    public void SetVerySmallShot()
    {
        shootForce = 10;
        setAnimName = "SmallShot";
    }

    // SmallShotの処理
    public void SetSmallShot()
    {
        shootForce = 30;
        setAnimName = "SmallShot";
    }

    // MiddleShotの処理
    public void SetMiddleShot()
    {
        shootForce = 50;
        setAnimName = "MiddleShot";
    }

    // FullShotの処理
    public void SetFullShot()
    {
        shootForce = 80;
        setAnimName = "FullShot";
    }
}