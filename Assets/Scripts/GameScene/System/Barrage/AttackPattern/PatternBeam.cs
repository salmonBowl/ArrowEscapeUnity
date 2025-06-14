using System;
using System.Collections.Generic;

public class PatternBeam : AttackPatternBase
{
    readonly BeamGenerator beamGenerator;
    readonly Func<bool> randomJudgeMoreover;

    public PatternBeam(BeamGenerator beamGenerator, Func<bool> randomJudge, CoolTimeManager timeManager, CoolTimeID coolTimeID, Func<bool> randomJudgeMoreover)
    {
        Init(randomJudge, timeManager, coolTimeID);

        this.beamGenerator = beamGenerator;
        this.randomJudgeMoreover = randomJudgeMoreover;
    }

    public override void Execute()
    {
        if (!timeManager.IsReady(CoolTimeID.Slot1)) return;

        // Update内で確率を引くと実行される
        if (randomJudge())
        {
            // ビームの生成
            List<float> beamhight_candidate = new() { -6f, -6f, -3f, -3f, 0 };
            float beamhight1 = beamhight_candidate[UnityEngine.Random.Range(0, beamhight_candidate.Count)];
            beamGenerator.GenerateBeam(beamhight1);

            // 運が悪いともう一本打たれる仕組み
            if (randomJudgeMoreover())
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

            timeManager.Reset(CoolTimeID.Slot1, 5f);
        }
    }
}
