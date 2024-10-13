using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManagerBase : MonoBehaviour
{
    StageUIManager stageUIManager;
    [SerializeField] GameObject bow;
    BowController bowController;
    [SerializeField] Cinemachine.CinemachineVirtualCamera targetVcam;
    public GameObject vcamTarget;
    public List<Target> targets;

    private int initialBowCount;
    public int bowCount = 5;
    private int outArrowCount = 0; // 「Out」エリアに当たった矢のカウント
    private int hitArrowCount = 0; // ターゲットに命中した矢のカウント

    public bool isGameCleared = false;
    private bool isGameOver = false;
    private bool isLastArrowFired = false;

    public bool isGameEnded = false;

    private void Start()
    {
        bowController = bow.GetComponent<BowController>();
        stageUIManager = FindObjectOfType<StageUIManager>();
        initialBowCount = bowCount;
        stageUIManager.UpdateArrowCount(bowCount);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
        }

        if (targets.Count == 0 && !isGameCleared)
        {
            StartCoroutine(GameClear());
        }

        // 矢がなくなり、全ターゲットに命中していない場合にゲームオーバー
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

    // 「Out」に当たった矢をカウント
    public void CountOutArrow()
    {
        outArrowCount++;
    }

    // ターゲットに当たった矢をカウント
    public void CountHitArrow()
    {
        hitArrowCount++;
    }

    private int CalculateStarRating()
    {
        int missedShots = outArrowCount; // 外した矢の本数
        bool allTargetsHit = targets.Count == 0;

        if (!allTargetsHit)
        {
            return 0;
        }

        if (missedShots <= 1)
        {
            return 3;
        }
        else if (missedShots <= 3)
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
            Debug.Log("Game Over! Out of bounds.");
            stageUIManager.ShowGameOverPanel();
        }
    }

    public IEnumerator GameClear()
    {
        isGameEnded = true;

        bow.gameObject.SetActive(false);

        targetVcam.Follow = vcamTarget.transform;
        targetVcam.LookAt = vcamTarget.transform;
        targetVcam.transform.position = vcamTarget.transform.position + targetVcam.GetCinemachineComponent<Cinemachine.CinemachineTransposer>().m_FollowOffset;
        targetVcam.Priority = 50;

        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 0f;

        if (!isGameCleared)
        {
            isGameCleared = true;
            int starRating = CalculateStarRating();
            stageUIManager.ShowGameClearPanel(starRating);
        }
    }

    public bool IsLastArrowFired()
    {
        return isLastArrowFired;
    }

    public void RemoveTarget(Target target)
    {
        if (targets.Contains(target))
        {
            targets.Remove(target);
        }
    }
}
