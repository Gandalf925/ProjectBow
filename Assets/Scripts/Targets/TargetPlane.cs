using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TargetPlane : MonoBehaviour
{
    [SerializeField] private StageManagerBase stageManager;
    [SerializeField] private List<ParticleSystem> explosionParticles; // 爆発パーティクルのリスト
    [SerializeField] private float shakeDuration = 1f;               // 揺れの持続時間
    [SerializeField] private float shakeStrength = 1f;               // 揺れの強さ
    [SerializeField] private float tiltAngle = 30f;                  // 傾き角度
    [SerializeField] private float descentDuration = 5f;             // 降下の時間
    [SerializeField] private float descentDistance = -100f;          // 降下の距離
    [SerializeField] private float minDelay = 0.1f;                  // 最小遅延時間
    [SerializeField] private float maxDelay = 0.5f;                  // 最大遅延時間
    [SerializeField] private ParticleSystem firstExplosion;          // 最初の爆発エフェクト

    private void Start()
    {
        stageManager = FindObjectOfType<StageManagerBase>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            HandleHit();
        }
    }

    private void HandleHit()
    {
        GameObject vcamTarget = this.gameObject;
        stageManager.vcamTarget = vcamTarget;

        Debug.Log("TargetPlane Hit");

        stageManager.RemoveTarget(this);

        // 爆発エフェクトを再生
        StartCoroutine(PlayExplosionEffectsWithDelay());

        // 機体を揺らしてから、傾けて降下
        Sequence planeSequence = DOTween.Sequence();

        // 揺れ
        planeSequence.Append(transform.DOShakePosition(shakeDuration, shakeStrength));

        // 傾きと降下
        planeSequence.Append(transform.DORotate(new Vector3(tiltAngle, 90, 0), 2f))
                     .Append(transform.DOMoveY(transform.position.y + descentDistance, descentDuration)
                     .SetEase(Ease.InOutSine))
                     .OnComplete(() => Destroy(gameObject)); // 降下完了後にオブジェクトを削除
    }

    private IEnumerator PlayExplosionEffectsWithDelay()
    {
        List<ParticleSystem> particles = new List<ParticleSystem>(explosionParticles);

        firstExplosion.Play();

        while (particles.Count > 0)
        {
            int randomIndex = Random.Range(0, particles.Count);
            particles[randomIndex].Play();
            particles.RemoveAt(randomIndex);

            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
        }
    }
}
