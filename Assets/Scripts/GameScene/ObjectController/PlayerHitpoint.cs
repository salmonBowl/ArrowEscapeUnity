using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHitpoint : MonoBehaviour
{
    [HideInInspector] public float HP = 1.0f;
    [SerializeField] Stage stage;
    [SerializeField] BossHealthPoint bosshp;
    [SerializeField] RetryGame retrypanel;
    [Space(20)]
    [SerializeField] Slider hpGauge;
    [SerializeField] float[] damage_value;

    void OnEnable() => EventManager.Instance().OnRetry += OnRetryGame;

    void Update()
    {
        float gaugeSmoothness = 0.9f; // 0～1.0f

        // HPゲージの描画
        hpGauge.value = (hpGauge.value * gaugeSmoothness) + (HP * (1 - gaugeSmoothness));

        // リトライ時にダメージを受けないように
        if (bosshp.HealthPoint == 1)
        {
            HP = 1;
        }
        // PerfectClear後最初に戻る時にもダメージを受けないように
        if (GamePhase.Instance().IsGameCredit)
        {
            HP = 1;
        }
    }

    /*
        ダメージを受ける関数
            Playerから呼び出し
            damageNum : 攻撃の種類
     */
    public void Damage(int damageNum)
    {
        HP = Mathf.Max(0, HP - damage_value[damageNum]);

        // 死んでしまうと最初から
        if (HP == 0)
        {
            Debug.Log("リトライしました");
            StartCoroutine(WaitReset());
        }
    }

    IEnumerator WaitReset()
    {
        yield return new WaitForSeconds(0.08f);

        EventManager.Instance().Event("Retry");

        StopAllCoroutines();
    }

    // リトライされたとき
    void OnRetryGame()
    {
        HP = 1;
        retrypanel.panelalpha = retrypanel.panelalpha_max;
    }
}
