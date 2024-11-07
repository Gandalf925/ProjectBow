using System.Collections;
using System.Collections.Generic;
using TMPro;
using TransitionsPlus;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectManager : MonoBehaviour
{
    [Header("Stage Data Lists")]
    [SerializeField] private List<StageData> easyStages;
    [SerializeField] private List<StageData> normalStages;
    [SerializeField] private List<StageData> hardStages;
    [SerializeField] private List<StageData> insaneStages;

    public List<StageData> EasyStages => easyStages;
    public List<StageData> NormalStages => normalStages;
    public List<StageData> HardStages => hardStages;
    public List<StageData> InsaneStages => insaneStages;

    [Header("UI References")]
    private TransitionAnimator transitionAnimator;
    [SerializeField] private GameObject stageButtonPrefab;
    [SerializeField] private Transform container;

    private void Start()
    {
        transitionAnimator = FindObjectOfType<TransitionAnimator>();
        LoadStageButtons(easyStages);
        TransitionIn();
    }

    private void OnEnable()
    {
        // シーンに戻ってきた際にステージボタンを再ロード
        LoadStageButtons(easyStages); // 必要に応じて他の難易度も更新
    }

    public void LoadStageButtons(List<StageData> stageList)
    {
        // 既存のボタンを削除
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        // ステージデータに基づいてボタンを生成
        for (int i = 0; i < stageList.Count; i++)
        {
            GameObject buttonObj = Instantiate(stageButtonPrefab, container);
            Button stageButton = buttonObj.GetComponent<Button>();
            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
            buttonText.text = stageList[i].name;

            // ステージがクリア済みかどうかの確認
            bool isStageUnlocked = i == 0 || GameManager.Instance.IsStageCleared(stageList[i - 1].name);

            // アンロックされている場合、ボタンのクリックイベントを設定
            stageButton.interactable = isStageUnlocked;
            if (isStageUnlocked)
            {
                int stageIndex = i; // ローカル変数にiをコピーしてキャプチャ
                stageButton.onClick.AddListener(() => StartCoroutine(OnStageSelected(stageList[stageIndex])));
            }
        }
    }

    private IEnumerator OnStageSelected(StageData stageData)
    {
        TransitionOut();
        yield return new WaitForSeconds(0.3f);
        StageLoader.Instance.LoadStage("StageBase", stageData);
    }

    public void LoadEasyStages() => LoadStageButtons(easyStages);
    public void LoadNormalStages() => LoadStageButtons(normalStages);
    public void LoadHardStages() => LoadStageButtons(hardStages);
    public void LoadInsaneStages() => LoadStageButtons(insaneStages);

    public void TransitionIn()
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
