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
            RenderAlpha(1, 1);
        }
        else
        {
            SetFillAmount(1);
            RenderAlpha(jumpCharge.Value, blink.AlphaRatio);
        }
    }

    void SetFillAmount(float value)
    {
        jumpChargeSprite.size = new Vector2(jumpChargeSprite.size.x, playerSpriteHeight * value);
        jumpChargeSprite.transform.localPosition = new Vector2(
            jumpChargeSprite.transform.localPosition.x,
            playerSpriteHeight / 2 * (value - 1)
            );
    }
    void RenderAlpha(float jumpCharge, float alphaRatio)
    {
        float chargeAlpha = jumpCharge == 0 ? 0 : (jumpChargeMaxAlpha * jumpCharge);

        playerSprite.color = SpriteSetAlpha(playerSprite, 1 * alphaRatio);
        jumpChargeSprite.color = SpriteSetAlpha(jumpChargeSprite, chargeAlpha * alphaRatio);
    }
    Color SpriteSetAlpha(SpriteRenderer sprite, float alpha)
    {
        Color color = sprite.color;
        color.a = alpha;
        return color;
    }

    // Update内でパーティクルを減衰
    void AttenuateParticle()
    {
        float illumdecrease = 0.05f;
        Color particleColor = particle_illum.color;
        particleColor.a = Mathf.Max(particleColor.a - illumdecrease, 0);
        particle_illum.color = particleColor;
    }
    void DispParticle()
    {
        particle_illum.color = new Color(1, 1, 1, 1);
    }
}
