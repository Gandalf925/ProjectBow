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
    private StageManagerBase stageManager;

    private Animator anim;



    void Start()
    {
        stageManager = FindObjectOfType<StageManagerBase>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (stageManager.bowCount <= 0) return;
        if (stageManager.isGameEnded) return;

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
        Debug.Log("arrow Instantiate");
        arrow.transform.SetParent(arrowSpawnPoint); // 矢を弓に取り付ける
        Debug.Log("arrow SetParent");
        currentArrow = arrow;
        Debug.Log("currentArrow = arrow");
        anim.SetBool("isAiming", true); // 弓のアニメーションを再生
        Debug.Log("anim.SetBool");
    }

    void ShootArrow(GameObject arrow)
    {
        Rigidbody rb = arrow.GetComponent<Rigidbody>();

        rb.isKinematic = false; // 重力を有効にする

        // 矢に設定された shootForce を適用して発射
        rb.AddForce(arrowSpawnPoint.forward * shootForce, ForceMode.Impulse);
        Debug.Log("rb.AddForce");

        currentArrow.transform.SetParent(null); // 矢を弓から離す
        Debug.Log("currentArrow.transform.SetParent");
        anim.SetBool("isAiming", false); // 弓のアニメーションを停止
        anim.SetTrigger(setAnimName); // 弓のアニメーションを再生

        // StageManagerに発射回数を通知
        stageManager.OnArrowShot();
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