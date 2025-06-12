using UnityEngine;

public class PlayerJumpCharge : MonoBehaviour
{
    /// <summary>
    /// プレイヤーのジャンプ力をチャージします。0.0f～1.0f
    /// </summary>
    public float Value { get; private set; }

    void Start()
    {
        Value = 0;
    }
    public void SetValue(float value)
    {
        // チャージが完了したらエフェクトを出す
        if (Value < 1 && value == 1)
        {
            PlayerEventManager.Instance().Event("ChargeFulled");
        }

        Value = value;
    }
}
