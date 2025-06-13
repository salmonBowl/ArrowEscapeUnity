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

public interface IAttackGeneratable { }

public class Barrage : MonoBehaviour
{
    [Header("以下の攻撃パターンを使います")]
    [SerializeField] ArrowGenerator arrowGenerator;
    [SerializeField] BeamGenerator beamGenerator;

    [Space(20)]

    [SerializeField, Header("依存関係")]
    IBossHealthStatus bosshp; readonly SerializeIBossHealthStatus iBossHelthStatus; [Serializable] private class SerializeIBossHealthStatus : SerializeInterface<IBossHealthStatus> { }

    [Space(20)]

    List<AttackPatternBase> patterns;

    // 時間経過で減っていく変数。複数個必要になりそうなのでリストに
    List<float> waitTimes;

    void Start()
    {
        bosshp = iBossHelthStatus.Interface();

        patterns = new()
        {
            new PatternPallarelArrow(arrowGenerator, () => bosshp.HealthPoint < 1, () => Fixed_Probability(80), 0, true),
            new PatternPallarelArrow(arrowGenerator, () => bosshp.HealthPoint < 0.75f, () => Fixed_Probability(80), 0, false),
            new PatternEmissionArrow(arrowGenerator, () => bosshp.HealthPoint < 1, () => Fixed_Probability(80), 0),
        };

        Application.targetFrameRate = 60;

        // Listの初期化
        waitTimes = Enumerable.Repeat(0.0f, 3).ToList();

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

        if (bosshp.HealthPoint < 0.75f)
        {
            // ビームが打たれる攻撃
            Beam();
        }
        if (bosshp.HealthPoint < 0.4f)
        {
            // Arrow爆弾が投下される
            ArrowBom();
        }
        if (bosshp.HealthPoint < 0.2f)
        {
            // 普通のArrow
            SingleArrow();
        }
    }

    /*
        ランダムに関する関数
        1/numの確率でtrueを返す
     */
    bool Fixed_Probability(int num)
    {
        int rand_value = UnityEngine.Random.Range(0, num);
        if (rand_value == 1)
        {
            return true;
        }
        return false;
    }

    void Beam()
    {
        // 1/80の確率でパターン2の生成
        if (waitTimes[1] == 0)
        {
            if (Fixed_Probability(80))
            {
                // ビームの生成
                List<float> beamhight_candidate = new() { -6f, -6f, -3f, -3f, 0 };
                float beamhight1 = beamhight_candidate[UnityEngine.Random.Range(0, beamhight_candidate.Count)];
                beamGenerator.GenerateBeam(beamhight1);

                // 運が悪いともう一本打たれる仕組み
                if (Fixed_Probability(13))
                {
                    // リストからbeam1の高さを削除
                    foreach (float h in beamhight_candidate.ToArray())
                    {
                        if (h == beamhight1)
                        {
                            beamhight_candidate.Remove(h);
                        }
                    }

                    // ビーム2の生成
                    float beamhight2 = beamhight_candidate[UnityEngine.Random.Range(0, beamhight_candidate.Count)];
                    beamGenerator.GenerateBeam(beamhight2);
                }

                waitTimes[1] = 5f;
            }
        }
    }
    void ArrowBom()
    {
        // 1/140の確率でパターン2の生成
        if (waitTimes[2] == 0)
        {
            if (Fixed_Probability(140))
            {
                // 生成する範囲の調整
                float half_genRange = arrowGenerator.stageWidth / 2 * 0.7f; // ArrowBomは端で生成されないように
                arrowGenerator.GeneratePattern03(UnityEngine.Random.Range(-half_genRange, half_genRange));

                waitTimes[2] = 5f;
            }
        }
    }
    void SingleArrow()
    {
        // 1/140の確率でパターン2の生成
        if (Fixed_Probability(50))
        {
            // 生成する範囲の調整
            float half_genRange = arrowGenerator.stageWidth / 2;
            arrowGenerator.GeneratePattern01(UnityEngine.Random.Range(-half_genRange, half_genRange), 1, 0);
        }
    }
}
