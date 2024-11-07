using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CloudManager : MonoBehaviour
{
    [Header("Cloud Settings")]
    public List<GameObject> cloudPrefabs;      // 雲のプレハブリスト
    public int cloudCount = 10;                // 最大で生成する雲の数
    public float minY = -30f;                  // 雲の生成位置の最小Y座標
    public float maxY = -20f;                  // 雲の生成位置の最大Y座標
    public float minX = 100f;                   // 雲の初期生成位置のX座標 (右側の生成位置)
    public float resetX = -50f;                // 雲が戻るX座標 (左側のリセット位置)
    public float minZ = -50f;                  // 雲の最小Z座標
    public float maxZ = 50f;                   // 雲の最大Z座標
    public float moveSpeed = 20f;               // 雲の移動速度
    public float spawnIntervalMin = 0.1f;        // 雲の生成間隔の最小時間
    public float spawnIntervalMax = 0.5f;        // 雲の生成間隔の最大時間

    private List<GameObject> clouds = new List<GameObject>();

    private void Start()
    {
        // 指定した数の雲をランダムなタイミングで生成
        InitializationClouds();
        StartCoroutine(SpawnCloudsRandomly());
    }

    private void Update()
    {
        // 各雲を右から左へ移動
        foreach (GameObject cloud in clouds)
        {
            cloud.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);

            // X座標がresetXを超えたら、minXの位置に戻し、Z軸とY軸の位置をランダムに再設定
            if (cloud.transform.position.x < resetX)
            {
                float newY = Random.Range(minY, maxY);
                float newZ = Random.Range(minZ, maxZ);
                cloud.transform.position = new Vector3(minX, newY, newZ);
            }
        }
    }

    private IEnumerator SpawnCloudsRandomly()
    {
        for (int i = 0; i < cloudCount; i++)
        {
            SpawnCloud();
            yield return new WaitForSeconds(Random.Range(spawnIntervalMin, spawnIntervalMax));
        }
    }

    private void InitializationClouds()
    {
        for (int i = 0; i < cloudCount; i++)
        {
            SpawnCloud();
        }
    }

    private void SpawnCloud()
    {
        if (cloudPrefabs.Count == 0)
        {
            Debug.LogWarning("No cloud prefabs assigned to the CloudManager.");
            return;
        }

        // ランダムに雲のプレハブを選択
        GameObject selectedCloudPrefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Count)];

        // ランダムな位置で生成
        float startY = Random.Range(minY, maxY);
        float startZ = Random.Range(minZ, maxZ);
        Vector3 startPosition = new Vector3(minX, startY, startZ);

        GameObject cloud = Instantiate(selectedCloudPrefab, startPosition, Quaternion.Euler(0, Random.Range(0, 360), 0));
        cloud.transform.localScale = Vector3.one * Random.Range(20f, 30f); // 雲のサイズをランダムに設定
        cloud.transform.parent = transform; // Managerの子オブジェクトに設定
        clouds.Add(cloud);
    }
}
