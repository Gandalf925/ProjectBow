using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI arrowCountText;
    [SerializeField] private GameObject gameClearPanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Stars")]
    [SerializeField] private Image[] stars; // 星のUI画像の配列

    [Header("Buttons")]
    [SerializeField] private Button retryButton;
    [SerializeField] private Button gameClearStageSelectButton;
    [SerializeField] private Button gameOverStageSelectButton;

    private StageManagerBase stageManager;

    private void Start()
    {
        stageManager = FindObjectOfType<StageManagerBase>();

        // ボタンのイベント設定
        retryButton.onClick.AddListener(OnRetry);
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

    public void ShowGameClearPanel(int starRating)
    {
        gameClearPanel.SetActive(true);

        // 星の表示を更新
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].enabled = i < starRating;
        }
    }

    // ゲームオーバーパネルを表示
    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    // Retryボタンを押したときの処理
    private void OnRetry()
    {
        StageLoader.Instance.UnloadStage();
        StageLoader.Instance.LoadCurrentStage();
    }

    // StageSelectボタンを押したときの処理
    private void OnStageSelect()
    {
        StageLoader.Instance.UnloadStage();
        SceneManager.LoadScene("StageSelect");
    }
}
