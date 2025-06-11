/*
    BossHP.cs
        ボス(またはステージ?)のHPを管理します

        処理
        ・HPゲージの描画
 */

using UnityEngine;
using UnityEngine.UI;

public class BossHealthPoint : MonoBehaviour, IBossHealthStatus
{
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

        UpdateManager.Instance().OnUpdateIfNotTitle += UpdateIfNotTitle;

        Player.OnAttacked += Attack;
        EventManager.Instance().OnRetry += OnRetryGame;
    }

    void UpdateIfNotTitle()
    {
        // HPゲージの描画
        gauge_fill.fillAmount = CalculateFillAmount(gauge_fill.fillAmount, hp);

        // 開始時に背景を表示
        gauge_backGround.fillAmount = CalculateFillAmount(gauge_backGround.fillAmount, 1);
    }

    float CalculateFillAmount(float currentAmount, float nextAmount)
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
            EventManager.Instance().Event("GameClear");
        }
    }
    void DecreaseHP(float damage)
    {
        hp = Mathf.Max(0, hp - damage);
    }

    void OnRetryGame()
    {
        if (GamePhase.Instance().IsGameCredit == false)
        {
            hp = 1;
        }
    }
}
