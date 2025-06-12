using System.Collections;
using UnityEngine;

public class PlayerVisualBlinking : MonoBehaviour
{
    [SerializeField] float lowestAlpha = 0.4f;
    [SerializeField] float blinkInterval = 0.15f;

    public float AlphaRatio { get; private set; } = 1;

    public void StartBlinking()
    {
        StartCoroutine(Blink());
    }
    public void StopBlinking()
    {
        StopAllCoroutines();
        AlphaRatio = 1;
    }

    // 無敵時間の点滅
    IEnumerator Blink()
    {
        while (true)
        {
            SetAlpha(lowestAlpha);
            yield return new WaitForSeconds(blinkInterval);
            SetAlpha(1);
            yield return new WaitForSeconds(blinkInterval);
        }
    }
    public void SetAlpha(float alpha)
    {
        //Debug.Log($"PlayerVisualBlinking.SetAlpha({alpha})");

        AlphaRatio = alpha;
    }
}
