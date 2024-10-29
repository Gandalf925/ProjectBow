using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TransitionsPlus;
using System;

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

    [Header("Timer UI")]
    public TextMeshProUGUI timerText;  // 残り時間を表示するText

    [Header("Wind UI")]
    private WindArea windArea;
    [SerializeField] private GameObject windUI;
    [SerializeField] private TMP_Text windStrengthText;

    [Header("Buttons")]
    [SerializeField] private Button retryButton;
    [SerializeField] private Button stageSelectButton;
    [SerializeField] private Button gameClearStageSelectButton;
    [SerializeField] private Button gameOverRetryButton;
    [SerializeField] private Button gameOverStageSelectButton;
    [SerializeField] GameObject blackoutPanel;
    public Image targetPictureImage;
    public GameObject targetPictureObject;

    private StageManagerBase stageManager;
    private TransitionAnimator transitionAnimator;
    private bool transitionInCalled = false;
    [SerializeField] private Image windVectorImage; // 矢印のUIイメージ

    private void Start()
    {
        blackoutPanel.SetActive(true);
        targetPictureObject.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        windUI.SetActive(false);

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

    public void SetWindUI()
    {
        if (stageManager.stageData.isWindEnabled)
        {
            windUI.SetActive(true);
            windStrengthText = windUI.GetComponentInChildren<TMP_Text>();
            windStrengthText.text = windArea.windStrength.ToString("F1") + "m/s";
            // 矢印の方向を更新
            UpdateWindVectorDirection(windArea.GetWindDirection(), windArea.GetWindStrength());
        }
        else
        {
            windUI.SetActive(false);
        }
    }

    private void UpdateWindVectorDirection(Vector3 windDirection, float windStrength)
    {
        // 矢印の方向を計算
        // 風の方向を基にQuaternionを作成
        Quaternion rotation = Quaternion.LookRotation(windDirection);

        // UIのImageを回転させるためには、Z軸を正しく向ける必要があります。
        // 矢印が上を向くように回転を調整
        windVectorImage.transform.rotation = Quaternion.Euler(0, 0, -rotation.eulerAngles.y); // Y軸の回転を使用

        // 風の強さを考慮して矢印のスケールを調整（オプション）
        float scaleFactor = windStrength / 3f; // 強さに基づいてスケールを調整
        windVectorImage.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
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

    public void ShowTargetPicture(Sprite targetPicture)
    {
        targetPictureImage.sprite = targetPicture;
        targetPictureObject.gameObject.SetActive(true);
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

    public void ShowTimerUI()
    {
        timerText.gameObject.SetActive(true);
    }

    public void HideTimerUI()
    {
        timerText.gameObject.SetActive(false);
    }

    // タイマー表示を更新
    public void UpdateTimer(float timeRemaining)
    {
        TimeSpan time = TimeSpan.FromSeconds(timeRemaining);
        timerText.text = string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
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
        StageLoader.Instance.LoadStage(stageManager.stageData.name, stageManager.stageData);
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
