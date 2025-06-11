using UnityEngine;

public class JumpCharge : MonoBehaviour
{
    /// <summary>
    /// プレイヤーのジャンプ力をチャージします。0.0f～1.0f
    /// </summary>
    public float Value;
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] SpriteRenderer mySprite;
    [SerializeField] float spriteHeight;
    [SerializeField] Player player;

    void Start()
    {
        Value = 0;
    }

    void Update()
    {
        SetFillAmount(Value);
        //jumpChargeに合わせて黄色フィルターの大きさを変える
        float player_colorAlpha = player.player_colorAlpha;
        playerSprite.color = Value != 0 ? new Color(1, 1, 1 - (0.4f * Value), player_colorAlpha)
            : new Color(1, 1, 1, player_colorAlpha);
    }
    void SetFillAmount(float value)
    {
        mySprite.size = new Vector2(mySprite.size.x, spriteHeight * value);
        transform.localPosition -= Vector3.up * (transform.localPosition.y - (spriteHeight * (value - 1) * 0.5f));
    }
}
