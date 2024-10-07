using UnityEngine;

public class BowController : MonoBehaviour
{
    public GameObject arrowPrefab; // 矢のプレハブ
    public GameObject currentArrow; // 矢
    public Transform arrowSpawnPoint; // 矢が発射される位置
    public float shootForce = 100f; // 矢の飛ばす力（初期値）
    public float rotationSpeed = 1f; // 弓の回転速度
    private bool isAiming = false; // 矢を構えているかどうか
    private int frameCount = 0; // フレーム数カウント
    private string setAnimName;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 左クリックで矢を構える
        {
            AimBow();
            isAiming = true;
            frameCount = 0; // フレームカウントのリセット
        }

        if (isAiming)
        {
            frameCount++; // フレーム数をカウントする
        }

        if (Input.GetMouseButtonUp(0)) // 左クリックを離すと矢を飛ばす
        {
            ShootArrow(currentArrow);
            isAiming = false;
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