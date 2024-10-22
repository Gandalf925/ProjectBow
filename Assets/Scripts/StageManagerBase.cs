using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class StageManagerBase : MonoBehaviour
{
    private StageUIManager stageUIManager;
    public StageData stageData; // StageDataを格納するための変数
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

    // クリア条件に関連するパラメータ
    public ClearConditionType clearConditionType;
    public GameObject specificTarget;
    public string specificPartName;
    private float timeLimit;
    private float startTime;
    bool isHitSpecificPart = false;
    bool isNonWeakPointHit = false;
    private float targetVcamDuration;
    private Vector3 targetVcamOffset;
    private bool isTimerRunning = false; // タイマーが動作中かどうかのフラグ
    public bool isHitCorrectTarget = false;
    internal bool isHitNotCorrectTarget = false;
    Sprite correctTargetPicture;


    // 初期化処理：ステージデータをセット
    public void Initialize(StageData data)
    {
        stageData = data;
        bowCount = data.maxArrowCount;
        stageName = data.name;
        missionTitle = data.missionTitle;
        initialBowCount = bowCount;
        threeStarThreshold = data.threeStarThreshold;
        twoStarThreshold = data.twoStarThreshold;
        pointLight.intensity = data.pointLightIntensity;
        targetVcamDuration = data.targetVcamDuration;
        targetVcamOffset = data.targetCameraOffset;

        SetupCameras();

        clearConditionType = data.clearConditionType;

        if (data.clearConditionType == ClearConditionType.HitAllTargets)
        {
            if (data.timeLimit > 0) SetupTimer();
        }

        if (clearConditionType == ClearConditionType.HitSpecificPart)
        {
            specificPartName = data.specificPartName;
        }

        if (clearConditionType == ClearConditionType.WeakPointOnly)
        {
            specificPartName = data.specificPartName;
        }

        stageUIManager.UpdateArrowCount(bowCount);
        stageUIManager.UpdateStageInfo(stageName, missionTitle);

        if (data.clearConditionType == ClearConditionType.HitCorrectTarget)
        {
            correctTargetPicture = stageData.targetPicture[UnityEngine.Random.Range(0, stageData.targetPicture.Count)];
            stageUIManager.ShowTargetPicture(correctTargetPicture);
            SetupTimer();
        }

        StartCoroutine(SetupStageTargets());
    }

    private void SetupTimer()
    {
        stageUIManager.ShowTimerUI();
        startTime = Time.time;  // タイマー開始
        timeLimit = stageData.timeLimit;
        isTimerRunning = true; // タイマーを動作中に設定
    }

    private void SetupCameras()
    {
        mainVirtualCamera = FindObjectOfType<CameraController>().GetComponent<CinemachineVirtualCamera>();
        targetVcam.Priority = 0;
        mainVirtualCamera.Priority = 10;
    }

    private void Start()
    {
        stageUIManager = FindObjectOfType<StageUIManager>();
    }

    private IEnumerator SetupStageTargets()
    {
        yield return new WaitForSeconds(1f);

        if (stageData.clearConditionType == ClearConditionType.HitCorrectTarget)
        {
            // ステージ内のTargetSetShelfを探す
            TargetSetShelf targetSetShelf = FindObjectOfType<TargetSetShelf>();

            if (targetSetShelf == null)
            {
                Debug.LogError("TargetSetShelf not found in the scene!");
                yield break;
            }

            // targetSetShelfの子オブジェクトからtargetPositionsを取得
            List<Transform> targetPositions = new List<Transform>();
            foreach (Transform child in targetSetShelf.transform)
            {
                targetPositions.Add(child);
            }

            // availablePositionsにtargetPositionsを代入
            List<Transform> availablePositions = new List<Transform>(targetPositions);

            // ターゲットプレハブをランダムに配置
            for (int i = 0; i < stageData.targetPrefabs.Count; i++)
            {
                // ランダムな位置にターゲットを配置
                int randomIndex = UnityEngine.Random.Range(0, availablePositions.Count);
                int randomRotation = UnityEngine.Random.Range(0, 359);
                GameObject instantiatedTarget = Instantiate(stageData.targetPrefabs[i], availablePositions[randomIndex].position, Quaternion.Euler(0, randomRotation, 0));
                availablePositions.RemoveAt(randomIndex);

                // ターゲットの名前と画像の名前が一致するものに "Target" タグを付ける
                if (instantiatedTarget.name == correctTargetPicture.name)
                {
                    instantiatedTarget.tag = "Target";
                    instantiatedTarget.AddComponent<CorrectTarget>();
                }
                else
                {
                    instantiatedTarget.tag = "Out";  // 一致しないものには別のタグを付ける
                    instantiatedTarget.AddComponent<NotCorrectTarget>();
                }
            }
        }
        else
        {

            GameObject[] targetObjects = GameObject.FindGameObjectsWithTag("Target");
            targets = new List<MonoBehaviour>();
            foreach (var targetObject in targetObjects)
            {
                MonoBehaviour targetComponent = targetObject.GetComponent<MonoBehaviour>();
                if (targetComponent != null)
                {
                    targets.Add(targetComponent);
                }
            }
        }
        isStageSetupComplete = true;
        stageUIManager.TransitionIn();
    }

    private void Update()
    {
        if (!isStageSetupComplete) return;

        HideUIButtons(isAiming);

        if (isTimerRunning)
        {
            UpdateTimer(); // タイマーが動作中の時だけタイマーを更新
        }

        CheckClearCondition();
        CheckGameOverCondition();
    }

    private void UpdateTimer()
    {
        float timeRemaining = Mathf.Max(0, timeLimit - (Time.time - startTime));
        stageUIManager.UpdateTimer(timeRemaining);

    }

    private void CheckClearCondition()
    {
        switch (clearConditionType)
        {
            case ClearConditionType.HitAllTargets:
                if (AllTargetsCleared()) TriggerGameClear();
                break;

            case ClearConditionType.HitSpecificPart:
                if (CheckHitSpecificPart()) TriggerGameClear();
                break;

            case ClearConditionType.HitCorrectTarget:
                if (isHitCorrectTarget) TriggerGameClear();
                break;

            case ClearConditionType.WeakPointOnly:
                // 弱点のみを狙うチェック
                if (CheckHitSpecificPart()) TriggerGameClear();
                break;
        }
    }

    private void CheckGameOverCondition()
    {
        switch (clearConditionType)
        {
            case ClearConditionType.HitAllTargets:
                if (CheckBowCountZero() && !AllTargetsCleared()) TriggerGameOver(); // 矢が尽きてもターゲットが残っている
                if (isTimerRunning && Time.time - startTime > timeLimit) TriggerGameOver();
                break;

            case ClearConditionType.HitSpecificPart:
                if (CheckBowCountZero()) TriggerGameOver(); // 矢が尽きてもターゲットが残っている
                if (isTimerRunning && Time.time - startTime > timeLimit) TriggerGameOver();
                break;

            case ClearConditionType.HitCorrectTarget:
                if (isHitNotCorrectTarget) TriggerGameOver();
                if (isTimerRunning && Time.time - startTime > timeLimit) TriggerGameOver();
                break;

            case ClearConditionType.WeakPointOnly:
                if (CheckBowCountZero() || CheckNonWeakPointHit()) TriggerGameOver();
                if (isTimerRunning && Time.time - startTime > timeLimit) TriggerGameOver();
                break;
        }
    }

    private bool CheckNonWeakPointHit()
    {
        // 弱点以外にヒットしたかの判定
        if (isNonWeakPointHit) return true;
        return false;
    }

    public void HitNonWeakPoint() => isNonWeakPointHit = true;

    private bool AllTargetsCleared()
    {
        return targets.Count == 0;
    }

    private void TriggerGameClear()
    {
        if (!isGameCleared)
        {
            isGameCleared = true;
            if (clearConditionType == ClearConditionType.HitCorrectTarget)
            {
                stageUIManager.HideTimerUI();
                isTimerRunning = false; // タイマーを停止
            }

            StartCoroutine(GameClear());
        }
    }

    private void TriggerGameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            if (clearConditionType == ClearConditionType.HitCorrectTarget)
            {
                stageUIManager.HideTimerUI();
                isTimerRunning = false; // タイマーを停止
            }
            stageUIManager.ShowGameOverPanel();
        }
    }

    private bool CheckBowCountZero()
    {
        return bowCount <= 0 && (hitArrowCount + outArrowCount) == initialBowCount;
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

    private IEnumerator GameClear()
    {
        isGameEnded = true;
        if (bow != null)
        {
            bow.gameObject.SetActive(false);
        }

        SetupClearCamera();
        yield return new WaitForSecondsRealtime(targetVcamDuration);

        stageUIManager.ShowGameClearPanel(CalculateStarRating());

        ResetStageSettings();
    }

    private void SetupClearCamera()
    {
        if (vcamTarget != null)
        {
            targetVcam.Priority = 50;
            targetVcam.Follow = vcamTarget.transform;
            targetVcam.LookAt = vcamTarget.transform;
            targetVcam.transform.position = vcamTarget.transform.position;
            var offset = targetVcam.GetCinemachineComponent<CinemachineTransposer>();
            if (targetVcamOffset != Vector3.zero)
            {
                offset.m_FollowOffset = targetVcamOffset;
            }
        }
    }

    private void ResetStageSettings()
    {
        Time.timeScale = 1f;
        isGameCleared = isGameOver = isGameEnded = isStageSetupComplete = false;
        isAiming = false;
        isNonWeakPointHit = false;
        isHitSpecificPart = false;
        isHitCorrectTarget = false;
        isHitNotCorrectTarget = false;
        startTime = 0;
        timeLimit = 0;
        outArrowCount = hitArrowCount = 0;

        // Reset camera priorities
        targetVcam.Priority = 0;
        mainVirtualCamera.Priority = 10;
        var offset = targetVcam.GetCinemachineComponent<CinemachineTransposer>();
        if (targetVcamOffset != Vector3.zero)
        {
            offset.m_FollowOffset = new Vector3(2, 5, 5);
        }
    }

    public void RemoveTarget(MonoBehaviour target)
    {
        if (targets.Contains(target))
            targets.Remove(target);
    }

    private bool CheckHitSpecificPart()
    {
        if (isHitSpecificPart) return true;
        return false;
    }

    public void HitSpecificPart() => isHitSpecificPart = true;
}
