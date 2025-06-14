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

    CoolTimeManager coolTimeManager;

    AttackPatternBase pallarelArrow1, pallarelArrow2, emissionArrow, beam, arrowBom, singleArrow;

    void Start()
    {
        UpdateManager.Instance.OnUpdateWhileTitle += UpdateWhileTitle;
        UpdateManager.Instance.OnUpdateWhileGame += UpdateWhileGame;

        bosshp = iBossHealthStatus.Interface();

        coolTimeManager = new();

        pallarelArrow1 = factory.Create(PatternType.PallarelArrow_Center, coolTimeManager);
        pallarelArrow2 = factory.Create(PatternType.PallarelArrow_Normal, coolTimeManager);
        emissionArrow = factory.Create(PatternType.EmissionArrow, coolTimeManager);
        beam = factory.Create(PatternType.Beam, coolTimeManager);
        arrowBom = factory.Create(PatternType.ArrowBom, coolTimeManager);
        singleArrow = factory.Create(PatternType.SingleArrow, coolTimeManager);
    }

    void UpdateWhileGame()
    {
        coolTimeManager.Tick(Time.deltaTime);

        bool lastPhase = bosshp.HealthPoint < 0.2f;

        if (bosshp.HealthPoint < 1)
        {
            if (!lastPhase)
            {
                // 並んだArrowが降ってくる攻撃
                pallarelArrow1.Execute();
            }
            // プレイヤーに向けたArrowの攻撃
            emissionArrow.Execute();
        }
        if (bosshp.HealthPoint < 0.75f)
        {
            if (!lastPhase)
            {
                // 並んだArrowが降ってくる攻撃2つ目 (全体の密度を上げるため)
                pallarelArrow2.Execute();
            }
            // ビームが打たれる攻撃
            beam.Execute();
        }
        if (bosshp.HealthPoint < 0.4f)
        {
            // Arrow爆弾が投下される
            arrowBom.Execute();
        }
        if (bosshp.HealthPoint < 0.2f) // lastPhase
        {
            pallarelArrow1.Execute();
            pallarelArrow1.Execute();
            pallarelArrow1.Execute();
            // 普通のArrow
            singleArrow.Execute();
        }
    }
    void UpdateWhileTitle()
    {

    }
}
