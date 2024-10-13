using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class StageLoader : MonoBehaviour
{
    public static StageLoader Instance { get; private set; }

    private List<GameObject> loadedObjects = new List<GameObject>();
    private StageData currentStageData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadStage(string stageName, StageData stageData)
    {
        currentStageData = stageData;

        // StageBase シーンをロードしてからオブジェクトを読み込む
        StartCoroutine(LoadStageSceneAsync("StageBase"));
    }

    private IEnumerator LoadStageSceneAsync(string stageSceneName)
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(stageSceneName);
        yield return loadScene;

        // シーンのロード完了後、StageManagerBaseを初期化してステージオブジェクトを読み込み
        yield return StartCoroutine(LoadStageObjects());
        InitializeStageManager();
    }

    private void InitializeStageManager()
    {
        // StageManagerBaseを取得し、Initializeメソッドを呼び出す
        StageManagerBase stageManager = FindObjectOfType<StageManagerBase>();
        if (stageManager != null && currentStageData != null)
        {
            stageManager.Initialize(currentStageData);
        }
        else
        {
            Debug.LogError("StageManagerBase or StageData is missing.");
        }
    }

    private IEnumerator LoadStageObjects()
    {
        foreach (var obj in currentStageData.objects)
        {
            AsyncOperationHandle<GameObject> handle = obj.assetReference.InstantiateAsync(obj.position, obj.rotation);
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                loadedObjects.Add(handle.Result);
            }
            else
            {
                Debug.LogError($"Failed to load {obj.assetReference.RuntimeKey}");
            }
        }
    }

    public void UnloadStage()
    {
        foreach (var obj in loadedObjects)
        {
            Addressables.ReleaseInstance(obj);
        }
        loadedObjects.Clear();
    }
}
