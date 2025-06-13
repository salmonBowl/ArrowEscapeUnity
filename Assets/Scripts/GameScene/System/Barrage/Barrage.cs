/*
    Barrage_Process.cs
        ArrowGeneratorで定義した関数を使って攻撃に関するゲームループの処理を行います

        処理
            ・攻撃を生成
            ・
 */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Barrage : MonoBehaviour
{
    [Header("依存関係")]

    [SerializeField] AttackPatternFactory factory;

    IBossHealthStatus bosshp; [SerializeField] SerializeIBossHealthStatus iBossHealthStatus; [Serializable] public class SerializeIBossHealthStatus : SerializeInterface<IBossHealthStatus> { }

    [Space(20)]

    List<AttackPatternBase> patterns;

    // 時間経過で減っていく変数。複数個必要になりそうなのでリストに
    List<float> waitTimes;

    void Start()
    {
        bosshp = iBossHealthStatus.Interface();

        patterns = new()
        {
            factory.Create(PatternType.PallarelArrow_Center),
            factory.Create(PatternType.PallarelArrow_Normal),
            factory.Create(PatternType.EmissionArrow),
            factory.Create(PatternType.Beam),
            factory.Create(PatternType.ArrowBom),
            factory.Create(PatternType.SingleArrow),
        };

        Application.targetFrameRate = 60;

        // Listの初期化
        waitTimes = Enumerable.Repeat(0.0f, 4).ToList();

        UpdateManager.Instance.OnUpdateWhileGame += UpdateWhileGame;
    }

    void UpdateWhileGame()
    {
        // waitTimes[]をカウントダウン
        waitTimes = waitTimes.Select(x => Mathf.Max(0, x - Time.deltaTime))
                           .ToList();

        foreach (var pattern in patterns)
        {
            pattern.Execute(waitTimes);
        }
        if (bosshp.HealthPoint < 1)
        {
            // 並んだArrowが降ってくる攻撃
            patterns[0].Execute(waitTimes);
            // プレイヤーに向けたArrowの攻撃
            patterns[2].Execute(waitTimes);
        }
        if (bosshp.HealthPoint < 0.75f)
        {
            // 並んだArrowが降ってくる攻撃2つ目 (全体の密度を上げるため)
            patterns[1].Execute(waitTimes);
            // ビームが打たれる攻撃
            patterns[3].Execute(waitTimes);
        }
        if (bosshp.HealthPoint < 0.4f)
        {
            // Arrow爆弾が投下される
            patterns[4].Execute(waitTimes);
        }
        if (bosshp.HealthPoint < 0.2f)
        {
            // 普通のArrow
            patterns[5].Execute(waitTimes);
        }
    }
}
