using System.Collections;
using UnityEngine;
using Cinemachine;

public class BowController : MonoBehaviour
{
    [SerializeField] GameObject bow;
    [SerializeField] GameObject arrowPrefab; // 矢のプレハブ
    GameObject currentArrow; // 矢
    [SerializeField] Transform arrowSpawnPoint; // 矢が発射される位置
    [SerializeField] float shootForce = 100f; // 矢の飛ばす力（初期値）
    private string setAnimName;

    private Animator anim;

    // Cinemachine関連
    public CinemachineVirtualCamera cinemachineCam; // Cinemachine Virtual Camera
    public float slowMotionScale = 0.5f; // スローモーションの速度
    public float slowMotionDuration = 5f; // スローモーションの持続時間
    public Vector3 cameraOffset = new Vector3(0, 2, -5); // カメラのオフセット位置
    public Vector3 cameraResetPosition = new Vector3(0, 2, -20); // カメラのリセット位置

    void Start()
    {
        anim = GetComponent<Animator>();
        if (cinemachineCam != null)
        {
            cinemachineCam.Follow = null; // 初期状態では追従をオフにしておく
            cinemachineCam.LookAt = null;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 左クリックで矢を構える
        {
            AimBow();
        }

        if (Input.GetMouseButtonUp(0)) // 左クリックを離すと矢を飛ばす
        {
            ShootArrow(currentArrow);
        }
    }

    void AimBow()
    {
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation); // 矢を生成
        arrow.transform.SetParent(arrowSpawnPoint); // 矢を弓に取り付ける
        currentArrow = arrow;
        anim.SetBool("isAiming", true); // 弓のアニメーションを再生
    }

    void ShootArrow(GameObject arrow)
    {
        Rigidbody rb = arrow.GetComponent<Rigidbody>();

        rb.isKinematic = false; // 重力を有効にする

        // 矢に設定された shootForce を適用して発射
        rb.AddForce(arrowSpawnPoint.forward * shootForce, ForceMode.Impulse);

        currentArrow.transform.SetParent(null); // 矢を弓から離す
        anim.SetBool("isAiming", false); // 弓のアニメーションを停止
        anim.SetTrigger(setAnimName); // 弓のアニメーションを再生
    }

    // SmallShotの処理
    public void SetVerySmallShot()
    {
        shootForce = 30;
        setAnimName = "SmallShot";
    }

    // SmallShotの処理
    public void SetSmallShot()
    {
        shootForce = 60;
        setAnimName = "SmallShot";
    }

    // MiddleShotの処理
    public void SetMiddleShot()
    {
        shootForce = 90;
        setAnimName = "MiddleShot";
    }

    // FullShotの処理
    public void SetFullShot()
    {
        shootForce = 120;
        setAnimName = "FullShot";
    }
}