using System.Collections;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] SpriteRenderer jumpChargeSprite;
    [SerializeField] float spriteHeight;
    [SerializeField] Player player;
    [SerializeField] PlayerJumpCharge jumpCharge;

    [Space(20)]
    [SerializeField] SpriteRenderer particle_illum;

    void Start()
    {
        PlayerEventManager.Instance().OnChargeFulled += DispParticleIllim;
    }

    void Update()
    {
        // パーティクルを減衰
        float illumdecrease = 0.05f;
        Color pcolor = particle_illum.color;
        particle_illum.color += new Color(0, 0, 0, Mathf.Max(illumdecrease - pcolor.a, 0) - illumdecrease);

        SetFillAmount(jumpCharge.Value);
        //jumpChargeに合わせて黄色フィルターの大きさを変える
        float player_colorAlpha = player.player_colorAlpha;
        playerSprite.color = jumpCharge.Value != 0 ? new Color(1, 1, 1 - (0.4f * jumpCharge.Value), player_colorAlpha)
            : new Color(1, 1, 1, player_colorAlpha);
    }
    void SetFillAmount(float value)
    {
        jumpChargeSprite.size = new Vector2(jumpChargeSprite.size.x, spriteHeight * value);
        transform.localPosition -= Vector3.up * (transform.localPosition.y - (spriteHeight * (value - 1) * 0.5f));
    }
    void DispParticleIllim()
    {
        particle_illum.color = new Color(1, 1, 1, 1);
    }
}
