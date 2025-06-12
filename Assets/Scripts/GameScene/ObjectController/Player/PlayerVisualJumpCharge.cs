using UnityEngine;

public class PlayerVisualJumpCharge : MonoBehaviour
{
    [SerializeField] PlayerJumpCharge jumpCharge;
    [SerializeField] PlayerVisualBlinking blink;

    [Space(20)]

    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] float playerSpriteHeight;
    [SerializeField] SpriteRenderer jumpChargeSprite;
    [SerializeField] float jumpChargeMaxAlpha;

    [Space(20)]

    [SerializeField] SpriteRenderer particle_illum;

    void Start() => PlayerEventManager.Instance().OnChargeFulled += DispParticle;

    void Update()
    {
        AttenuateParticle();

        if (GamePhase.Instance().IsTitle)
        {
            SetFillAmount(jumpCharge.Value);
            RenderAlpha(1, blink.AlphaRatio);
        }
        else
        {
            SetFillAmount(1);
            RenderAlpha(jumpCharge.Value, blink.AlphaRatio);
        }
    }
    /// <summary>
    /// チャージが完了したエフェクトを表示します
    /// </summary>

    void SetFillAmount(float value)
    {
        jumpChargeSprite.size = new Vector2(jumpChargeSprite.size.x, playerSpriteHeight * value);
        jumpChargeSprite.transform.localPosition -= Vector3.up * (transform.localPosition.y - (playerSpriteHeight * (value - 1) * 0.5f));
    }
    void RenderAlpha(float jumpCharge, float alphaRatio)
    {
        float chargeAlpha = jumpCharge != 0 ? 0 : (jumpChargeMaxAlpha * jumpCharge);

        Color playerColor = playerSprite.color;
        Color chargeColor = jumpChargeSprite.color;

        playerColor.a = alphaRatio * 1;
        chargeColor.a = alphaRatio * chargeAlpha;

        playerSprite.color = playerColor;
        jumpChargeSprite.color = chargeColor;
    }

    void DispParticle()
    {
        particle_illum.color = new Color(1, 1, 1, 1);
    }


    // Update内でパーティクルを減衰
    void AttenuateParticle()
    {
        float illumdecrease = 0.05f;
        Color pcolor = particle_illum.color;
        particle_illum.color += new Color(0, 0, 0, Mathf.Max(illumdecrease - pcolor.a, 0) - illumdecrease);

    }
}
