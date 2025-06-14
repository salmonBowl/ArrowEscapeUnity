using System;
using System.Collections.Generic;

public class PatternArrowBom : AttackPatternBase
{
    readonly ArrowGenerator arrowGenerator;

    public PatternArrowBom(ArrowGenerator arrowGenerator, Func<bool> randomJudge, CoolTimeManager timeManager, CoolTimeID coolTimeID)
    {
        Init(randomJudge, timeManager, coolTimeID);

        this.arrowGenerator = arrowGenerator;
    }

    public override void Execute()
    {
        if (!timeManager.IsReady(CoolTimeID.Slot2)) return;

        // Update内で確率を引くと実行される
        if (randomJudge())
        {
            // 生成する範囲の調整
            float half_genRange = arrowGenerator.stageWidth / 2 * 0.7f; // ArrowBomは端で生成されないように
            arrowGenerator.GeneratePattern03(UnityEngine.Random.Range(-half_genRange, half_genRange));

            timeManager.Reset(CoolTimeID.Slot2, 5f);
        }
    }
}
