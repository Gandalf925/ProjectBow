using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TransitionsPlus;
using System.Collections;

public class StageSelectManager : MonoBehaviour
{

    [SerializeField] private Button easy1StageButton; // ステージボタン
    [SerializeField] private Button easy2StageButton; // ステージボタン
    TransitionAnimator transitionAnimator;

    private void Start()
    {
        transitionAnimator = FindObjectOfType<TransitionAnimator>();
        TransitionIn();

        easy1StageButton.onClick.AddListener(() => StartCoroutine(LoadStage("Easy-1")));
        easy2StageButton.onClick.AddListener(() => StartCoroutine(LoadStage("Easy-2")));

    }

    public IEnumerator LoadStage(string stageName)
    {
        TransitionOut();
        yield return new WaitForSeconds(1.0f);

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

    private void TransitionIn()
    {
        transitionAnimator.profile.invert = true;
        transitionAnimator.Play();
    }
    private void TransitionOut()
    {
        transitionAnimator.profile.invert = false;
        transitionAnimator.Play();
    }
}
