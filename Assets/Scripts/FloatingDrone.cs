using UnityEngine;
using DG.Tweening;
using System.Collections;

public class FloatingDrone : MonoBehaviour
{
    [SerializeField] private float moveDistance = 0.5f; // 上下に動く距離
    [SerializeField] private float moveDuration = 1f;   // 上下の動きにかかる時間

    [SerializeField] private StageManagerBase stageManager;
    [SerializeField] private LookAtCamera lookAtCamera;
    [SerializeField] private GameObject targetVcam;
    [SerializeField] private GameObject afterStageClearPrefab;
    [SerializeField] private ParticleSystem sparkEffect1;
    [SerializeField] private ParticleSystem sparkEffect2;
    Rigidbody rb;

    private Vector3 startPosition;
    private bool isActivate;

    void Start()
    {
        // ドローンの初期位置を記録
        stageManager = FindObjectOfType<StageManagerBase>();
        startPosition = transform.position;

        if (afterStageClearPrefab != null)
        {
            afterStageClearPrefab.SetActive(false);
        }

        rb = GetComponent<Rigidbody>();  // Rigidbodyの取得を忘れずに

        StartFloating();
    }

    void StartFloating()
    {
        // ドローンの上下移動
        transform.DOMoveY(startPosition.y + moveDistance, moveDuration)
            .SetEase(Ease.InOutSine)  // 滑らかな動き
            .SetLoops(-1, LoopType.Yoyo); // 無限にループする (Yoyoで上下に行ったり来たり)
    }

    private IEnumerator OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            isActivate = true;

            // 上下の動きを停止する
            transform.DOKill();
            sparkEffect1.Play();
            sparkEffect2.Play();

            rb.isKinematic = false;
            stageManager.vcamTarget = targetVcam;

            yield return new WaitForSeconds(0.5f);

            lookAtCamera.isActivated = true;
            stageManager.RemoveTarget(this);

            if (afterStageClearPrefab != null)
            {
                yield return new WaitForSeconds(5f);

                afterStageClearPrefab.SetActive(true);
            }
        }
    }
}
