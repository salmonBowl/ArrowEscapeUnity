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

    void Update()
    {
        bool isVisualFillMode = GamePhase.Instance().IsTitle;
        if (isVisualFillMode)
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

        SetSpriteColorAlpha(playerSprite, 1 * alphaRatio);
        SetSpriteColorAlpha(jumpChargeSprite, chargeAlpha * alphaRatio);
    }
    void SetSpriteColorAlpha(SpriteRenderer sprite, float alpha)
    {
        Color color = sprite.color;
        color.a = alpha;
        sprite.color = color;
    }
}
