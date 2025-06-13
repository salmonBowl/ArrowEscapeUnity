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

    // 時間経過で減っていく変数。複数個必要になりそうなので配列、Linqで操作するのでリストに
    List<float> waitTimes;

    AttackPatternBase pallarelArrow1, pallarelArrow2, emissionArrow, beam, arrowBom, singleArrow;

    void Start()
    {
        bosshp = iBossHealthStatus.Interface();

        pallarelArrow1 = factory.Create(PatternType.PallarelArrow_Center);
        pallarelArrow2 = factory.Create(PatternType.PallarelArrow_Normal);
        emissionArrow = factory.Create(PatternType.EmissionArrow);
        beam = factory.Create(PatternType.Beam);
        arrowBom = factory.Create(PatternType.ArrowBom);
        singleArrow = factory.Create(PatternType.SingleArrow);

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

        if (bosshp.HealthPoint < 1)
        {
            // 並んだArrowが降ってくる攻撃
            pallarelArrow1.Execute(waitTimes);
            // プレイヤーに向けたArrowの攻撃
            emissionArrow.Execute(waitTimes);
        }
        if (bosshp.HealthPoint < 0.75f)
        {
            // 並んだArrowが降ってくる攻撃2つ目 (全体の密度を上げるため)
            pallarelArrow2.Execute(waitTimes);
            // ビームが打たれる攻撃
            beam.Execute(waitTimes);
        }
        if (bosshp.HealthPoint < 0.4f)
        {
            // Arrow爆弾が投下される
            arrowBom.Execute(waitTimes);
        }
        if (bosshp.HealthPoint < 0.2f)
        {
            // 普通のArrow
            singleArrow.Execute(waitTimes);
        }
    }
}
