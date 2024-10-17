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

    [Header("UI References")]
    TransitionAnimator transitionAnimator;
    [SerializeField] private GameObject stageButtonPrefab; // ステージボタンのプレハブ
    [SerializeField] private Transform container; // GridLayoutのコンテナ

    private void Start()
    {
        transitionAnimator = FindObjectOfType<TransitionAnimator>();
        LoadStageButtons(easyStages); // 初期表示としてEasyのステージを表示
        TransitionIn();
    }

    public void LoadStageButtons(List<StageData> stageList)
    {
        // 既存のボタンを削除
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        // ステージデータに基づいてボタンを生成
        foreach (var stageData in stageList)
        {
            GameObject buttonObj = Instantiate(stageButtonPrefab, container);
            Button stageButton = buttonObj.GetComponent<Button>();

            // ボタンの画像とクリックイベントを設定
            Image buttonImage = buttonObj.GetComponent<Image>();
            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
            buttonText.text = stageData.name;

            stageButton.onClick.AddListener(() => StartCoroutine(OnStageSelected(stageData)));
        }
    }

    private IEnumerator OnStageSelected(StageData stageData)
    {
        // 選択したステージデータをもとにステージをロードする処理を実行
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