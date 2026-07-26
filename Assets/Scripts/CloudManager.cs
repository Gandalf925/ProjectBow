using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    [Header("Cloud Settings")]
    public List<GameObject> cloudPrefabs;
    public int cloudCount = 10;
    public float minY = -30f;
    public float maxY = -20f;
    public float minX = 100f;
    public float resetX = -50f;
    public float minZ = -50f;
    public float maxZ = 50f;
    public float moveSpeed = 20f;
    public float spawnIntervalMin = 0.1f;
    public float spawnIntervalMax = 0.5f;

    private readonly List<Transform> clouds = new List<Transform>();

    private void Start()
    {
        StartCoroutine(SpawnCloudsRandomly());
    }

    private void Update()
    {
        float movement = moveSpeed * Time.deltaTime;

        for (int i = clouds.Count - 1; i >= 0; i--)
        {
            Transform cloud = clouds[i];
            if (cloud == null)
            {
                clouds.RemoveAt(i);
                continue;
            }

            cloud.Translate(Vector3.left * movement, Space.World);

            if (cloud.position.x < resetX)
            {
                cloud.position = new Vector3(
                    minX,
                    Random.Range(minY, maxY),
                    Random.Range(minZ, maxZ));
            }
        }
    }

    private IEnumerator SpawnCloudsRandomly()
    {
        int targetCount = Mathf.Max(0, cloudCount);
        for (int i = 0; i < targetCount; i++)
        {
            SpawnCloud();

            if (i < targetCount - 1)
            {
                float delay = Random.Range(
                    Mathf.Max(0f, spawnIntervalMin),
                    Mathf.Max(spawnIntervalMin, spawnIntervalMax));
                yield return new WaitForSeconds(delay);
            }
        }
    }

    private void SpawnCloud()
    {
        if (cloudPrefabs == null || cloudPrefabs.Count == 0)
        {
            Debug.LogWarning("No cloud prefabs assigned to the CloudManager.");
            return;
        }

        GameObject selectedCloudPrefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Count)];
        if (selectedCloudPrefab == null)
        {
            return;
        }

        Vector3 startPosition = new Vector3(
            minX,
            Random.Range(minY, maxY),
            Random.Range(minZ, maxZ));

        GameObject cloudObject = Instantiate(
            selectedCloudPrefab,
            startPosition,
            Quaternion.Euler(0f, Random.Range(0f, 360f), 0f),
            transform);

        cloudObject.transform.localScale = Vector3.one * Random.Range(20f, 30f);
        clouds.Add(cloudObject.transform);
    }
}
