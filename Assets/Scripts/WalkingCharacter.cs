using UnityEngine;
using System.Collections;
using DG.Tweening; // DoTweenの名前空間を追加

public class WalkingCharacter : MonoBehaviour
{
    public float walkSpeed = 2.0f;      // 歩く速度
    public float walkDistance = 5.0f;   // 一度の移動距離
    public bool turnFlag = false;      // フラグ: Trueで往復動作
    private float walkedDistance = 0f;   // これまでに歩いた距離

    private Animator animator;           // アニメーター参照

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // フラグが立っている場合に歩く
        if (turnFlag)
        {
            if (walkedDistance < walkDistance)
            {
                WalkForward();
            }
            else
            {
                StartCoroutine(TurnAround());
            }
        }
        else
        {
            WalkForward(); // フラグが立っていない場合は常に歩く
        }
    }

    // 前方に歩く処理
    private void WalkForward()
    {
        transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
        walkedDistance += walkSpeed * Time.deltaTime;

        // アニメーション: isWalkingフラグをTrueに
        animator.SetBool("isWalking", true);
    }

    // 反転処理
    private IEnumerator TurnAround()
    {
        turnFlag = false; // フラグをリセット
        animator.SetBool("isWalking", false); // Walkingアニメーションを停止
        animator.SetTrigger("Idle"); // Idleアニメーションを再生

        // 0.5秒で180度反転
        // 反転中は移動を停止
        yield return transform.DORotate(new Vector3(0, transform.eulerAngles.y + 180f, 0), 0.5f)
            .OnStart(() =>
            {
                // 反転開始時に移動を停止
                walkSpeed = 0f;
            })
            .OnComplete(() =>
            {
                walkedDistance = 0f; // 歩いた距離をリセット
                animator.SetBool("isWalking", true); // 歩き始める
                turnFlag = true; // フラグを再設定
                walkSpeed = 2.0f; // 移動速度を元に戻す
            });
    }

    // フラグの設定
    public void SetTurnFlag(bool flag)
    {
        turnFlag = flag;
    }
}
