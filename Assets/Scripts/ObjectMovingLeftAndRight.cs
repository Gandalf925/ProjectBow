using System.Collections;
using UnityEngine;

public class MovingBarrel : MonoBehaviour
{
    public float speed = 2.0f; // 移動速度
    public float distance = 20.0f; // 左右の移動範囲

    private Vector3 startPosition;
    private int direction = 1; // 1は右、-1は左

    private void Start()
    {
        startPosition = transform.position;
        StartCoroutine(MoveBarrel());
    }

    private IEnumerator MoveBarrel()
    {
        while (true)
        {
            // 移動する距離を計算
            float moveDistance = direction * speed * Time.deltaTime;
            transform.Translate(moveDistance, 0, 0);

            // 距離の範囲を超えた場合、移動方向を反転
            if (Vector3.Distance(startPosition, transform.position) >= distance - 0.01f)
            {
                direction *= -1; // 移動方向を反転
                yield return new WaitForSeconds(0.2f); // 1秒間の停止時間
            }

            yield return null;
        }
    }
}
