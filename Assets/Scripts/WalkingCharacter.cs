using UnityEngine;
using System.Collections;
using DG.Tweening;

public class WalkingCharacter : MonoBehaviour
{
    public float walkSpeed = 2.0f;      // 歩く速度
    public float walkDistance = 5.0f;   // 一度の移動距離
    public bool turnFlag = false;       // フラグ: Trueで往復動作
    private float walkedDistance = 0f;  // これまでに歩いた距離

    private Animator animator;

    [SerializeField] private RuntimeAnimatorController[] animatorControllers; // アニメーターコントローラーの配列

    void Start()
    {
        animator = GetComponent<Animator>();
        SetRandomAnimatorController(); // ランダムにアニメーターコントローラーをセット
    }

    void Update()
    {
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
            WalkForward();
        }
    }

    private void WalkForward()
    {
        transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
        walkedDistance += walkSpeed * Time.deltaTime;

        animator.SetBool("isWalking", true);
    }

    private IEnumerator TurnAround()
    {
        turnFlag = false;
        animator.SetBool("isWalking", false);

        yield return transform.DORotate(new Vector3(0, transform.eulerAngles.y + 180f, 0), 0.5f)
            .OnStart(() => walkSpeed = 0f)
            .OnComplete(() =>
            {
                walkedDistance = 0f;
                turnFlag = true;
                walkSpeed = 2.0f;
                SetRandomAnimatorController(); // ターン後に新しいアニメーターをランダムにセット
            });
    }

    private void SetRandomAnimatorController()
    {
        if (animatorControllers.Length == 0) return; // アニメーターコントローラーが未設定の場合

        int randomIndex = Random.Range(0, animatorControllers.Length);
        animator.runtimeAnimatorController = animatorControllers[randomIndex];
    }

    public void SetTurnFlag(bool flag)
    {
        turnFlag = flag;
    }
}
