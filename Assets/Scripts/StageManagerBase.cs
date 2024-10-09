using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManagerBase : MonoBehaviour
{
    [SerializeField] GameObject bow;
    [SerializeField] Cinemachine.CinemachineVirtualCamera targetVcam;
    public GameObject vcamTarget;
    public List<Target> targets; // ターゲットをリストで管理

    public bool isGameCleared = false;

    private void Start()
    {
        // 配列からリストへ変換
        targets = new List<Target>(FindObjectsOfType<Target>());
    }

    private void Update()
    {
        // ゲームのリセットおよびタイトルに戻る
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
        }

        // 全てのターゲットが破壊されたらゲームクリア
        if (targets.Count == 0 && !isGameCleared)
        {
            StartCoroutine(GameClear());
        }
    }

    // ゲームクリア処理
    public IEnumerator GameClear()
    {
        bow.gameObject.SetActive(false);

        // vcamの追尾対象を設定
        targetVcam.Follow = vcamTarget.transform;
        targetVcam.LookAt = vcamTarget.transform;

        // カメラ位置を更新
        targetVcam.transform.position = vcamTarget.transform.position + targetVcam.GetCinemachineComponent<Cinemachine.CinemachineTransposer>().m_FollowOffset;
        targetVcam.Priority = 50;

        yield return new WaitForSecondsRealtime(3f);

        if (!isGameCleared)
        {
            isGameCleared = true;
            Debug.Log("Game Cleared! All targets destroyed.");
        }
    }

    // ターゲットが破壊される際にリストから削除
    public void RemoveTarget(Target target)
    {
        if (targets.Contains(target))
        {
            targets.Remove(target);
        }
    }
}
