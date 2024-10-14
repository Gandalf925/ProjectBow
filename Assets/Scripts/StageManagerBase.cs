using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class StageManagerBase : MonoBehaviour
{
    private StageUIManager stageUIManager;
    public string stageName;
    public string missionTitle;

    [Header("Components")]
    [SerializeField] private BowController bow;
    [SerializeField] private CinemachineVirtualCamera targetVcam;
    [SerializeField] private CinemachineVirtualCamera mainVirtualCamera;
    [SerializeField] private Light pointLight;

    [Header("Game Settings")]
    public GameObject vcamTarget;
    public List<MonoBehaviour> targets;

    // Game stats
    private int initialBowCount;
    private int outArrowCount = 0;
    private int hitArrowCount = 0;

    public int bowCount;
    public bool isAiming = false;
    public bool isGameCleared = false;
    public bool isGameEnded = false;
    private bool isGameOver = false;
    private bool isStageSetupComplete = false;

    // Star rating thresholds
    private int threeStarThreshold;
    private int twoStarThreshold;

    // 初期化処理：ステージデータをセット
    public void Initialize(StageData stageData)
    {
        bowCount = stageData.maxArrowCount;
        stageName = stageData.name;
        missionTitle = stageData.missionTitle;
        initialBowCount = bowCount;
        threeStarThreshold = stageData.threeStarThreshold;
        twoStarThreshold = stageData.twoStarThreshold;
        pointLight.intensity = stageData.pointLightIntensity;


        SetupCameras();
        SetupBow();

        stageUIManager.UpdateArrowCount(bowCount);
        stageUIManager.UpdateStageInfo(stageName, missionTitle);
    }

    private void SetupCameras()
    {
        mainVirtualCamera = FindObjectOfType<CameraController>().GetComponent<CinemachineVirtualCamera>();
        targetVcam.Priority = 0;
        mainVirtualCamera.Priority = 10;
    }

    private void SetupBow()
    {
        bow = FindObjectOfType<BowController>();
        bow.transform.SetParent(Camera.main.transform);
        bow.transform.localPosition = new Vector3(0f, -0.175f, 1.5f);
        bow.transform.localRotation = Quaternion.Euler(0, 0, 77.58f);
        bow.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    }

    private void Start()
    {
        stageUIManager = FindObjectOfType<StageUIManager>();
        StartCoroutine(SetupStageTargets());
    }

    private IEnumerator SetupStageTargets()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag("Target");
        targets = new List<MonoBehaviour>();
        foreach (var targetObject in targetObjects)
        {
            MonoBehaviour targetComponent = targetObject.GetComponent<MonoBehaviour>();
            if (targetComponent != null)
                targets.Add(targetComponent);
        }
        isStageSetupComplete = true;
        stageUIManager.TransitionIn();
    }

    private void Update()
    {
        if (!isStageSetupComplete) return;

        HandleGameInputs();
        HideUIButtons(isAiming);

        if (AllTargetsCleared() && !isGameCleared)
        {
            StartCoroutine(GameClear());
        }

        if (ShouldTriggerGameOver())
        {
            GameOver();
        }
    }

    private void HandleGameInputs()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StageLoader.Instance.ReloadCurrentStage();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
        }
    }

    private void HideUIButtons(bool isAiming)
    {
        if (isAiming || isGameEnded)
        {
            stageUIManager.HideButtons();
        }
        else
        {
            stageUIManager.ShowButtons();
        }

    }

    private bool AllTargetsCleared()
    {
        return targets.Count == 0;
    }

    private bool ShouldTriggerGameOver()
    {
        return bowCount <= 0 && initialBowCount == (outArrowCount + hitArrowCount) && targets.Count > 0 && !isGameOver;
    }

    public void OnArrowShot()
    {
        bowCount--;
        stageUIManager.UpdateArrowCount(bowCount);
    }

    public void CountOutArrow() => outArrowCount++;

    public void CountHitArrow() => hitArrowCount++;

    private int CalculateStarRating()
    {
        if (targets.Count > 0) return 0; // All targets must be hit for any rating

        int missedShots = outArrowCount;
        if (missedShots <= threeStarThreshold) return 3;
        if (missedShots <= twoStarThreshold) return 2;
        return 1;
    }

    private void GameOver()
    {
        if (!isGameOver)
        {
            isGameEnded = true;
            isGameOver = true;
            stageUIManager.ShowGameOverPanel();
        }
    }

    private IEnumerator GameClear()
    {
        isGameEnded = true;
        bow.gameObject.SetActive(false);

        SetupClearCamera();
        yield return new WaitForSecondsRealtime(3f);

        if (!isGameCleared)
        {
            isGameCleared = true;
            stageUIManager.ShowGameClearPanel(CalculateStarRating());
        }

        ResetStageSettings();
    }

    private void SetupClearCamera()
    {
        if (vcamTarget != null)
        {
            targetVcam.Priority = 50;
            targetVcam.Follow = vcamTarget.transform;
            targetVcam.LookAt = vcamTarget.transform;
            targetVcam.transform.position = vcamTarget.transform.position + targetVcam.GetCinemachineComponent<Cinemachine.CinemachineTransposer>().m_FollowOffset;
        }
    }

    private void ResetStageSettings()
    {
        Time.timeScale = 1f;
        isGameCleared = isGameOver = isGameEnded = isStageSetupComplete = false;

        // Reset camera priorities
        targetVcam.Priority = 0;
        mainVirtualCamera.Priority = 10;
    }

    public void RemoveTarget(MonoBehaviour target)
    {
        if (targets.Contains(target))
            targets.Remove(target);
    }

    internal string GetStageName()
    {
        return stageName;
    }

    internal string GetStageMissionTitle()
    {
        return missionTitle;
    }
}
