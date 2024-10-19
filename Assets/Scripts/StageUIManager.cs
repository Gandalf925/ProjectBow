using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TransitionsPlus;

public class StageUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI arrowCountText;
    [SerializeField] private TMP_Text stageNameText;
    [SerializeField] private TMP_Text missionTitleText;
    [SerializeField] private GameObject gameClearPanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Stars")]
    [SerializeField] private Image[] stars; // 星のUI画像の配列

    [Header("Buttons")]
    [SerializeField] private Button retryButton;
    [SerializeField] private Button stageSelectButton;
    [SerializeField] private Button gameClearStageSelectButton;
    [SerializeField] private Button gameOverRetryButton;
    [SerializeField] private Button gameOverStageSelectButton;
    [SerializeField] GameObject blackoutPanel;

    private StageManagerBase stageManager;
    private TransitionAnimator transitionAnimator;
    private bool transitionInCalled = false;


    private void Start()
    {
        blackoutPanel.SetActive(true);
        stageManager = FindObjectOfType<StageManagerBase>();
        transitionAnimator = FindObjectOfType<TransitionAnimator>();

        // ボタンのイベント設定
        retryButton.onClick.AddListener(OnRetry);
        gameOverRetryButton.onClick.AddListener(OnRetry);
        stageSelectButton.onClick.AddListener(OnStageSelect);
        gameClearStageSelectButton.onClick.AddListener(OnStageSelect);
        gameOverStageSelectButton.onClick.AddListener(OnStageSelect);

        // ゲーム開始時はクリア・ゲームオーバーパネルは非表示
        gameClearPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    // 矢の残り数の表示を更新
    public void UpdateArrowCount(int count)
    {
        arrowCountText.text = "x" + count.ToString();
    }

    // ステージ名とミッションタイトルの表示を更新
    public void UpdateStageInfo(string stageName, string missionTitle)
    {
        stageNameText.text = stageName;
        missionTitleText.text = missionTitle;
    }

    public void ShowGameClearPanel(int starRating)
    {
        gameClearPanel.SetActive(true);

        // 星の表示を更新
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].enabled = i < starRating;
        }
    }

    public void HideButtons()
    {
        retryButton.gameObject.SetActive(false);
        stageSelectButton.gameObject.SetActive(false);
    }

    public void ShowButtons()
    {
        retryButton.gameObject.SetActive(true);
        stageSelectButton.gameObject.SetActive(true);
    }

    // ゲームオーバーパネルを表示
    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    // Retryボタンを押したときの処理
    private void OnRetry()
    {
        StartCoroutine(Retry());
    }

    private IEnumerator Retry()
    {
        TransitionOut();
        yield return new WaitForSeconds(0.3f);
        StageLoader.Instance.UnloadStage();
        StageLoader.Instance.ReloadCurrentStage();
    }

    // StageSelectボタンを押したときの処理
    private void OnStageSelect()
    {
        StartCoroutine(MoveStageSelect());
    }

    private IEnumerator MoveStageSelect()
    {
        TransitionOut();
        yield return new WaitForSeconds(0.3f);

        StageLoader.Instance.UnloadStage();
        SceneManager.LoadScene("StageSelect");
    }

    public void TransitionIn()
    {
        // すでに呼び出されていたら処理をスキップ
        if (transitionInCalled) return;

        transitionAnimator.profile.invert = true;
        transitionAnimator.Play();
        blackoutPanel.SetActive(false);

        // 呼び出し済みフラグを立てる
        transitionInCalled = true;
    }

    public void TransitionOut()
    {
        transitionAnimator.profile.invert = false;
        transitionAnimator.Play();

        // フラグをリセット
        transitionInCalled = false;
    }
}
