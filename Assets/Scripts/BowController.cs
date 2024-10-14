using System.Collections;
using UnityEngine;
using Cinemachine;
using NUnit.Framework;

public class BowController : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab; // 矢のプレハブ
    [SerializeField] private Transform arrowSpawnPoint; // 矢が発射される位置
    [SerializeField] private float shootForce = 100f; // 矢の飛ばす力（初期値）
    private string setAnimName;
    private GameObject currentArrow; // 現在の矢
    private Animator anim;
    private StageManagerBase stageManager;

    private void Start()
    {
        anim = GetComponent<Animator>();
        stageManager = FindObjectOfType<StageManagerBase>();
    }

    private void Update()
    {
        if (stageManager.bowCount <= 0 || stageManager.isGameEnded)
            return;

        if (Input.GetMouseButtonDown(0)) // 左クリックで矢を構える
        {
            AimBow();
        }

        if (Input.GetMouseButtonUp(0)) // 左クリックを離すと矢を飛ばす
        {
            ShootArrow();
        }
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
        Rigidbody rb = currentArrow.GetComponent<Rigidbody>();
        rb.isKinematic = false; // 重力を有効にする

        // 矢に設定された shootForce を適用して発射
        rb.AddForce(arrowSpawnPoint.forward * shootForce, ForceMode.Impulse);

        currentArrow.transform.SetParent(null); // 矢を弓から離す
        anim.SetBool("isAiming", false); // 弓のアニメーションを停止
        anim.SetTrigger(setAnimName); // 弓のアニメーションを再生
        IsAimingFalse();

        // StageManagerに発射回数を通知
        stageManager.OnArrowShot();
    }

    private void IsAimingFalse()
    {
        stageManager.isAiming = false;
    }

    private void IsAimingTrue()
    {
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