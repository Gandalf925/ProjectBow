using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManagerBase : MonoBehaviour
{
    private StageUIManager stageUIManager;
    private string stageName;
    [SerializeField] private BowController bow;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera targetVcam;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera mainVirtualCamera;
    public GameObject vcamTarget;

    public List<MonoBehaviour> targets;
    [SerializeField] Light pointLight;

    public int bowCount;
    private int initialBowCount;
    private int outArrowCount = 0;
    private int hitArrowCount = 0;

    public bool isGameCleared = false;
    private bool isGameOver = false;
    public bool isGameEnded = false;
    private bool isStageSetupComplete = false;

    private int threeStarThreshold;
    private int twoStarThreshold;

    // ステージデータを受け取るメソッド
    public void Initialize(StageData stageData)
    {
        bowCount = stageData.maxArrowCount;
        initialBowCount = bowCount;
        stageName = stageData.name;
        threeStarThreshold = stageData.threeStarThreshold;
        twoStarThreshold = stageData.twoStarThreshold;
        pointLight.intensity = stageData.pointLightIntensity;

        // カメラの優先順位をリセット
        mainVirtualCamera = FindObjectOfType<CameraController>().GetComponent<Cinemachine.CinemachineVirtualCamera>();

        bow = FindObjectOfType<BowController>();
        bow.transform.SetParent(Camera.main.transform);
        bow.transform.localPosition = new Vector3(0f, -0.175f, 1.5f);
        bow.transform.localRotation = Quaternion.Euler(0, 0, 77.5810089f);
        bow.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

        targetVcam.Priority = 0; // TargetVcamの優先度を低く設定
        mainVirtualCamera.Priority = 10; // MainVirtualCameraの優先度を高く設定
    }

    private void Start()
    {
        stageUIManager = FindObjectOfType<StageUIManager>();
        stageUIManager.UpdateArrowCount(bowCount);

        StartCoroutine(SetupStage());
    }

    private IEnumerator SetupStage()
    {
        yield return new WaitForSeconds(0.5f);
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

        isStageSetupComplete = true;
    }

    private void Update()
    {
        if (!isStageSetupComplete) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            StageLoader.Instance.UnloadStage();
            StageLoader.Instance.LoadCurrentStage();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
        }

        if (targets.Count == 0 && !isGameCleared)
        {
            StartCoroutine(GameClear());
        }

        if (bowCount <= 0 && initialBowCount == (outArrowCount + hitArrowCount) && targets.Count > 0 && !isGameOver)
        {
            GameOver();
        }
    }

    public void OnArrowShot()
    {
        bowCount--;
        stageUIManager.UpdateArrowCount(bowCount);
    }

    public void CountOutArrow()
    {
        outArrowCount++;
    }

    public void CountHitArrow()
    {
        hitArrowCount++;
    }

    private int CalculateStarRating()
    {
        int missedShots = outArrowCount;
        bool allTargetsHit = targets.Count == 0;

        if (!allTargetsHit)
        {
            return 0;
        }

        if (missedShots <= threeStarThreshold)
        {
            return 3;
        }
        else if (missedShots <= twoStarThreshold)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameEnded = true;
            isGameOver = true;
            stageUIManager.ShowGameOverPanel();
        }
    }

    public IEnumerator GameClear()
    {
        isGameEnded = true;
        bow.gameObject.SetActive(false);

        // クリア時のカメラ設定
        targetVcam.gameObject.SetActive(true);

        if (vcamTarget != null)
        {
            targetVcam.Follow = vcamTarget.transform;
            targetVcam.Priority = 50; // TargetVcamの優先度を上げる
            targetVcam.Follow = vcamTarget.transform;
            targetVcam.LookAt = vcamTarget.transform;
            targetVcam.transform.position = vcamTarget.transform.position + targetVcam.GetCinemachineComponent<Cinemachine.CinemachineTransposer>().m_FollowOffset;
        }

        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 0f;

        if (!isGameCleared)
        {
            isGameCleared = true;
            int starRating = CalculateStarRating();
            stageUIManager.ShowGameClearPanel(starRating);
        }

        ResetStageSettings();
    }

    public void RemoveTarget(MonoBehaviour target)
    {
        if (targets.Contains(target))
        {
            targets.Remove(target);
        }
    }

    public void ResetStageSettings()
    {
        Time.timeScale = 1f;
        isGameCleared = false;
        isGameOver = false;
        isGameEnded = false;
        isStageSetupComplete = false;

        // StageClear後のカメラ優先順位リセット
        targetVcam.Priority = 0; // TargetVcamの優先度を下げる
        mainVirtualCamera.Priority = 10; // MainVirtualCameraの優先度を上げる
    }
}
