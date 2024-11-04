using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Dictionary<string, bool> stageClearStatus = new Dictionary<string, bool>();
    private StageSelectManager stageSelectManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadStageSelectManager();
            LoadStageClearStatus(); // 進捗状況を読み込み
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadStageSelectManager()
    {
        stageSelectManager = FindObjectOfType<StageSelectManager>();

        if (stageSelectManager != null)
        {
            InitializeStageClearStatus(stageSelectManager.EasyStages);
            InitializeStageClearStatus(stageSelectManager.NormalStages);
            InitializeStageClearStatus(stageSelectManager.HardStages);
            InitializeStageClearStatus(stageSelectManager.InsaneStages);
        }
        else
        {
            Debug.LogError("StageSelectManagerがシーンに存在しません。");
        }
    }

    // 各難易度リストからクリア状況を初期化
    private void InitializeStageClearStatus(List<StageData> stages)
    {
        foreach (var stageData in stages)
        {
            if (!stageClearStatus.ContainsKey(stageData.name))
            {
                stageClearStatus[stageData.name] = false; // 初期は全ステージ未クリア
            }
        }
    }

    // ステージのクリア状況を確認
    public bool IsStageCleared(string stageName)
    {
        return stageClearStatus.ContainsKey(stageName) && stageClearStatus[stageName];
    }

    // ステージクリア時にクリア状況を更新し保存
    public void MarkStageAsCleared(string stageName)
    {
        if (stageClearStatus.ContainsKey(stageName))
        {
            stageClearStatus[stageName] = true;
            SaveProgress();
        }
    }

    // 進捗状況の保存
    private void SaveProgress()
    {
        // セーブ処理 (EasySaveなど)
        ES3.Save("stageClearStatus", stageClearStatus);
    }

    private void LoadStageClearStatus()
    {
        if (ES3.KeyExists("stageClearStatus"))
        {
            stageClearStatus = ES3.Load<Dictionary<string, bool>>("stageClearStatus");
        }
        else
        {
            Debug.Log("クリア状況のデータが存在しません。新規作成します。");
        }
    }
}
