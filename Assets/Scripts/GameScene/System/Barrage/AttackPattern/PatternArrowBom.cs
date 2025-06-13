using System;
using System.Collections.Generic;

public class PatternArrowBom : AttackPatternBase
{
    readonly ArrowGenerator arrowGenerator;

    public PatternArrowBom(ArrowGenerator arrowGenerator, Func<bool> executeCondition, Func<bool> randomJudge, int waitTimeIndex)
    {
        Init(executeCondition, randomJudge, waitTimeIndex);

        this.arrowGenerator = arrowGenerator;
    }

    public override void Execute(List<float> waitTimes)
    {
        if (executeCondition() == false) return;

        if (waitTimes[waitTimeIndex] != 0) return;

        // Update内で確率を引くと実行される
        if (randomJudge())
        {
            // 生成する範囲の調整
            float half_genRange = arrowGenerator.stageWidth / 2 * 0.7f; // ArrowBomは端で生成されないように
            arrowGenerator.GeneratePattern03(UnityEngine.Random.Range(-half_genRange, half_genRange));

            waitTimes[2] = 5f;
        }
    }
}
