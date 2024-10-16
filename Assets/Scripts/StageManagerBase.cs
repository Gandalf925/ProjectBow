using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class StageManagerBase : MonoBehaviour
{
    private StageUIManager stageUIManager;
    private StageData stageData; // StageDataを格納するための変数
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
    private List<GameObject> orderedTargets;
    private int currentOrderedTargetIndex = 0;
    private float startTime;
    bool isHitSpecificPart = false;
    bool isNonWeakPointHit = false;
    private float targetVcamDuration;

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

        SetupCameras();
        SetupBow();

        clearConditionType = data.clearConditionType;

        if (clearConditionType == ClearConditionType.HitSpecificPart)
        {
            specificPartName = data.specificPartName;
        }

        if (clearConditionType == ClearConditionType.TimeLimit)
        {
            timeLimit = data.timeLimit;
        }

        if (clearConditionType == ClearConditionType.WeakPointOnly)
        {
            specificPartName = data.specificPartName;
        }

        stageUIManager.UpdateArrowCount(bowCount);
        stageUIManager.UpdateStageInfo(stageName, missionTitle);

        if (clearConditionType == ClearConditionType.TimeLimit)
            startTime = Time.time;
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
        yield return new WaitForSeconds(1f);
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

        HideUIButtons(isAiming);

        CheckClearCondition();
        CheckGameOverCondition();
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

            case ClearConditionType.TimeLimit:
                if (Time.time - startTime <= timeLimit && AllTargetsCleared()) TriggerGameClear();
                break;

            case ClearConditionType.WeakPointOnly:
                // 弱点のみを狙うチェック
                if (CheckHitSpecificPart()) TriggerGameClear();
                break;
        }
    }

    private bool CheckHitInOrder()
    {
        if (currentOrderedTargetIndex >= orderedTargets.Count)
            return true; // 全てのターゲットが順序通りにヒット済みの場合はクリア

        // 現在のターゲットが次の順序であるか確認
        GameObject currentTarget = orderedTargets[currentOrderedTargetIndex];
        if (targets.Contains(currentTarget.GetComponent<MonoBehaviour>())) // まだターゲットがリストに残っている場合
        {
            return false; // 正しい順序で当たっていないため、クリア条件は達成されていない
        }

        // 正しい順序でターゲットに当たった場合、次のターゲットに進む
        currentOrderedTargetIndex++;
        return currentOrderedTargetIndex >= orderedTargets.Count; // すべてのターゲットが順番通りにヒットされたかを返す
    }

    private void CheckGameOverCondition()
    {
        switch (clearConditionType)
        {
            case ClearConditionType.HitAllTargets:
                if (CheckBowCount() && !AllTargetsCleared()) TriggerGameOver(); // 矢が尽きてもターゲットが残っている
                break;

            case ClearConditionType.HitSpecificPart:
                if (CheckBowCount()) TriggerGameOver(); // 矢が尽きてもターゲットが残っている
                break;

            case ClearConditionType.TimeLimit:
                if (CheckBowCount() || (Time.time - startTime > timeLimit && !AllTargetsCleared())) TriggerGameOver(); // 時間切れでターゲットが残っている
                break;

            case ClearConditionType.WeakPointOnly:
                if (CheckBowCount() || CheckNonWeakPointHit()) TriggerGameOver();
                break;
        }
    }

    private bool IncorrectOrderHit()
    {
        // 順番通りにターゲットがヒットされたかを判定
        orderedTargets[currentOrderedTargetIndex].TryGetComponent(out MonoBehaviour target);
        return false; // 例: 実際の判定結果を返す
    }

    private bool CheckNonWeakPointHit()
    {
        // 弱点以外にヒットしたかの判定
        if (isNonWeakPointHit)
        {
            return true; // 例: 実際の判定結果を返す
        }
        return false;
    }

    public void HitNonWeakPoint()
    {
        isNonWeakPointHit = true;
    }


    private bool AllTargetsCleared()
    {
        return targets.Count == 0;
    }

    private void TriggerGameClear()
    {
        if (!isGameCleared)
        {
            isGameCleared = true;
            StartCoroutine(GameClear());
        }
    }

    private void TriggerGameOver()
    {
        if (!isGameOver)
        {
            isGameEnded = true;
            isGameOver = true;
            stageUIManager.ShowGameOverPanel();
        }
    }

    private bool CheckBowCount()
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
            targetVcam.transform.position = vcamTarget.transform.position + targetVcam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        }
    }

    private void ResetStageSettings()
    {
        Time.timeScale = 1f;
        isGameCleared = isGameOver = isGameEnded = isStageSetupComplete = false;
        isAiming = false;
        isNonWeakPointHit = false;
        isHitSpecificPart = false;
        currentOrderedTargetIndex = 0;
        outArrowCount = hitArrowCount = 0;

        // Reset camera priorities
        targetVcam.Priority = 0;
        mainVirtualCamera.Priority = 10;
    }

    public void RemoveTarget(MonoBehaviour target)
    {
        if (targets.Contains(target))
            targets.Remove(target);
    }

    private bool CheckHitSpecificPart()
    {
        if (isHitSpecificPart)
        {
            return true;
        }
        return false;
    }

    public void HitSpecificPart()
    {
        isHitSpecificPart = true;
    }
}
