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
    [Header("以下の攻撃パターンを使います")]
    [SerializeField] ArrowGenerator arrowGenerator;
    [SerializeField] BeamGenerator beamGenerator;

    [Space(20)]

    [Header("依存関係")]

    IBossHealthStatus bosshp;
    [SerializeField] SerializeIBossHealthStatus iBossHealthStatus; [Serializable] public class SerializeIBossHealthStatus : SerializeInterface<IBossHealthStatus> { }

    [Space(20)]

    List<AttackPatternBase> patterns;

    // 時間経過で減っていく変数。複数個必要になりそうなのでリストに
    List<float> waitTimes;

    void Start()
    {
        bosshp = iBossHealthStatus.Interface();

        patterns = new()
        {
            new PatternPallarelArrow(arrowGenerator, () => bosshp.HealthPoint < 1, () => Fixed_Probability(80), 0, true),
            new PatternPallarelArrow(arrowGenerator, () => bosshp.HealthPoint < 0.75f, () => Fixed_Probability(80), 0, false),
            new PatternEmissionArrow(arrowGenerator, () => bosshp.HealthPoint < 1, () => Fixed_Probability(80), 0),
            new PatternBeam(beamGenerator, () => bosshp.HealthPoint < 0.75f, () => Fixed_Probability(80), CoolTimeID.Slot1, () => Fixed_Probability(13)),
            new PatternArrowBom(arrowGenerator, () => bosshp.HealthPoint < 0.4f, () => Fixed_Probability(140), CoolTimeID.Slot2),
            new PatternSingleArrow(arrowGenerator, () => bosshp.HealthPoint < 0.2f, () => Fixed_Probability(50), CoolTimeID.Slot3),
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
}
