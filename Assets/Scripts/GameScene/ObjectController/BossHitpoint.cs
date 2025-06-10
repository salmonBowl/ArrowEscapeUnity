/*
    BossHP.cs
        ボス(またはステージ?)のHPを管理します

        処理
        ・HPゲージの描画
 */

using UnityEngine;
using UnityEngine.UI;

public interface IBossHealthStatus
{
    float HealthPoint { get; }
}

public class BossHealthpoint : MonoBehaviour, IBossHealthStatus
{
    [SerializeField]
    private Stage stage;

    [Space(20)]

    [SerializeField]
    private Image gauge_fill;
    [SerializeField]
    private Image gauge_backGround;

    [SerializeField, Header("一回の攻撃で与えられるダメージ")]
    float damageAmount = 0.09f;

    float hp = 1;

    // IBossHealthStatusの変数
    public float HealthPoint => hp;

    void Start()
    {
        gauge_fill.fillAmount = 0;
        gauge_backGround.fillAmount = 0;

        Player.OnAttacked += Attack;
    }

    void Update()
    {
        if (stage.IsStart)
        {
            // HPゲージの描画
            gauge_fill.fillAmount = CalFillAmount(gauge_fill.fillAmount, hp);

            // 開始時に背景を表示
            gauge_backGround.fillAmount = CalFillAmount(gauge_backGround.fillAmount, 1);
        }
    }
    float CalFillAmount(float currentAmount, float nextAmount)
    {
        float smoothness = 0.9f; // 0.0f～1.0f

        return currentAmount * smoothness +
                  nextAmount * (1 - smoothness);
    }

    public void Attack()
    {
        DecreaseHP(damageAmount);

        if (hp == 0)
        {
            stage.Clear();
        }
    }
    void DecreaseHP(float damage)
    {
        hp = Mathf.Max(0, hp - damage);
    }
}
