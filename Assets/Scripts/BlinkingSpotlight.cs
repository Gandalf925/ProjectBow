using System.Collections;
using UnityEngine;

public class BlinkingSpotlight : MonoBehaviour
{
    [SerializeField] private Light spotLight;   // 点灯させるスポットライト
    [SerializeField] private float lightOnDuration = 2f;  // 点灯時間
    [SerializeField] private float lightOffDuration = 2f; // 消灯時間

    private bool isLightOn = true; // 現在ライトが点灯しているかどうか

    private void Start()
    {
        if (spotLight == null)
        {
            spotLight = GetComponent<Light>();
        }

        StartCoroutine(ToggleLight());
    }

    // ライトの点灯・消灯を切り替えるコルーチン
    private IEnumerator ToggleLight()
    {
        while (true)
        {
            spotLight.enabled = isLightOn;
            if (isLightOn)
            {
                yield return new WaitForSeconds(lightOnDuration); // 点灯時間待機
            }
            else
            {
                yield return new WaitForSeconds(lightOffDuration); // 消灯時間待機
            }

            isLightOn = !isLightOn; // 点灯・消灯を反転
        }
    }
}