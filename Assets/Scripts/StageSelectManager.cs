using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StageSelectManager : MonoBehaviour
{

    [SerializeField] private Button easy1StageButton; // ステージボタン
    [SerializeField] private Button easy2StageButton; // ステージボタン

    private void Start()
    {
        // ボタンが押されたときに「Easy-1」ステージをロード
        easy1StageButton.onClick.AddListener(() => LoadStage("Easy-1"));
        easy2StageButton.onClick.AddListener(() => LoadStage("Easy-2"));

    }

    public void LoadStage(string stageName)
    {
        // Addressables から stageName に基づいて StageData をロード
        Addressables.LoadAssetAsync<StageData>(stageName).Completed += OnStageDataLoaded;
    }

    private void OnStageDataLoaded(AsyncOperationHandle<StageData> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            StageData stageData = handle.Result;
            // ステージデータが正常にロードできた場合に、StageLoaderを使ってステージを読み込む
            StageLoader.Instance.LoadStage("StageBase", stageData);
        }
        else
        {
            Debug.LogError("Failed to load StageData from Addressables.");
        }
    }
}
