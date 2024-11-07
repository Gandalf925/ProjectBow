using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour
{
    public float windStrength = 10.0f;  // 風の強さ
    public Vector3 windDirection = Vector3.forward; // 風の方向
    public string targetTag = "Arrow"; // 風の効果を与えるタグ
    public List<ParticleSystem> particleSystems; // 風の影響を受けるパーティクルシステムのリスト

    private void Start()
    {
        RandomizeWindDirection(); // 風の方向をランダムに設定

        // パーティクルの方向を風に同期させる
        SyncParticlesWithWind();
    }

    public void RandomizeWindDirection()
    {
        // 風の方向をランダムに設定
        windDirection = new Vector3(
            UnityEngine.Random.Range(-1.0f, 1.0f),
            0f,
            UnityEngine.Random.Range(-1.0f, 1.0f)).normalized; // 正規化
    }

    public void RandomizeWindStrength(float maxWindStrength)
    {
        // 風の強さをランダムに設定
        windStrength = UnityEngine.Random.Range(1f, maxWindStrength);
    }

    private void OnTriggerStay(Collider other)
    {
        // Rigidbodyを持ち、指定されたタグを持つオブジェクトに風の効果を与える
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null && other.CompareTag(targetTag))
        {
            // 力を加える
            rb.AddForce(windDirection * windStrength);
        }
    }

    private void SyncParticlesWithWind()
    {
        foreach (var particleSystem in particleSystems)
        {
            if (particleSystem != null)
            {
                // パーティクルシステムのVelocityOverLifetimeモジュールを取得
                var velocityOverLifetime = particleSystem.velocityOverLifetime;

                // 風の方向を正規化して、VelocityOverLifetimeに設定
                Vector3 normalizedWindDirection = windDirection.normalized;

                // 風の力を考慮してVelocityOverLifetimeのX、Y、Zを設定
                velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(normalizedWindDirection.x * windStrength / 3);
                velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(normalizedWindDirection.y * windStrength / 3);
                velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(normalizedWindDirection.z * windStrength / 3);
            }
        }
    }

    public void SetWindDirection(Vector3 newWindDirection)
    {
        windDirection = newWindDirection; // 風の方向を設定
        SyncParticlesWithWind(); // パーティクルの方向を風に同期
    }

    public void SetWindStrength(float newWindStrength)
    {
        windStrength = newWindStrength; // 風の強さを設定
        SyncParticlesWithWind(); // パーティクルの方向を風に同期
    }

    public Vector3 GetWindDirection()
    {
        return windDirection; // 風の方向を取得
    }

    public float GetWindStrength()
    {
        return windStrength; // 風の強さを取得
    }
}
