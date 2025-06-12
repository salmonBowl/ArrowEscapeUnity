using System.Collections;
using UnityEngine;

public class PlayerVisualBlinking : MonoBehaviour
{
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] SpriteRenderer jumpChargeSprite;

    [Space(20)]

    [SerializeField] float blinkAlpha = 0.4f;
    [SerializeField] float blinkInterval = 0.15f;

    private float player_colorAlpha = 1;

    public void StartBlinking()
    {
        StartCoroutine(Blink());
    }
    public void StopBlinking()
    {
        StopAllCoroutines();
        player_colorAlpha = 1;
    }

    public void SetAlpha(float alpha)
    {
        Color playerColor = playerSprite.color;
        Color chargeColor = jumpChargeSprite.color;
        playerColor.a = alpha;
        chargeColor.a = alpha;
        playerSprite.color = playerColor;
        jumpChargeSprite.color = chargeColor;
    }
    // 無敵時間の点滅
    IEnumerator Blink()
    {
        while (true)
        {
            SetAlpha(blinkAlpha);
            yield return new WaitForSeconds(blinkInterval);
            SetAlpha(0);
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
