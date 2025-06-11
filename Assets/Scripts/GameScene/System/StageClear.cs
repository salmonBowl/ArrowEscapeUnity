/*
    StageClear.cs
        ステージクリア後に紙吹雪を降らせます

        処理
            ・紙吹雪の量の計算
            ・降らせる
 */

using UnityEngine;

public class StageClear : MonoBehaviour
{
    [SerializeField] Background background;
    [SerializeField] GameObject Confetti;
    [SerializeField] GameObject Arrow;
    [SerializeField] PlayerHitpoint playerhp;
    [SerializeField] float waitConfetti; // ステージクリアの余韻のために少し時間を空けて降らせたい
    [SerializeField] float confetti_maxInterval;
    [SerializeField] float confetti_timeToMax;
    [SerializeField] float generateHight;

    float time = 0;
    float frameCount = 0;

    void Start()
    {
        UpdateManager manager = UpdateManager.Instance();
        manager.OnUpdateWhileGameClear += UpdateWhileGameClearOrPerfectClear;
        manager.OnUpdateWhilePerfectClear += UpdateWhileGameClearOrPerfectClear;

        time = -waitConfetti;
    }

    // クリア後
    void UpdateWhileGameClearOrPerfectClear()
    {
        time += Time.deltaTime;

        // 紙吹雪を降らせる

        // 1つの生成に何フレーム使うか、最初は量が少ない
        float genInterval = confetti_maxInterval * Mathf.Max(confetti_timeToMax / Mathf.Max(0.001f, time), 1);

        frameCount++;
        if (genInterval <= frameCount)
        {
            frameCount -= genInterval;

            // ダメージ0でクリアしたか判定
            if (playerhp.HP == 1)
            {
                EventManager.Instance().Event("ChangeClearModePerfect");
            }

            // 生成場所を計算
            float halfGenRange = background.Width / 2;
            float genPosX = Random.Range(-halfGenRange, halfGenRange);

            GameObject scatterTarget = null;

            GamePhase phase = GamePhase.Instance();
            if (phase.IsGameClear)
                scatterTarget = Confetti;
            else if (phase.IsPerfectClear) // ダメージ0でクリアしたとき
                scatterTarget = Arrow; // 代わりにArrowが降る
            else
                Debug.LogError("GamePhaseが \"GameClear\" でも \"PerfectClear\" でもない状態でConfettiクラスのUpdateが呼ばれています");

            // 紙吹雪生成
            GameObject confetti = Instantiate(scatterTarget, new Vector3(genPosX, generateHight, 0), Quaternion.identity);

            if (phase.IsPerfectClear)
            {
                confetti.GetComponent<ArrowController>().wait_time = 0;
            }
        }
    }
}
