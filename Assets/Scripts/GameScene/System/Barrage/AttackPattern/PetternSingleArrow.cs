using System;
using System.Collections.Generic;

public class PatternSingleArrow : AttackPatternBase
{
    readonly ArrowGenerator arrowGenerator;

    public PatternSingleArrow(ArrowGenerator arrowGenerator, Func<bool> executeCondition, Func<bool> randomJudge, CoolTimeID coolTimeID)
    {
        Init(executeCondition, randomJudge, coolTimeID);

        this.arrowGenerator = arrowGenerator;
    }

    public override void Execute(List<float> waitTimes)
    {
        if (executeCondition() == false) return;

        if (waitTimes[(int)coolTimeID] != 0) return;

        // Update内で確率を引くと実行される
        if (randomJudge())
        {
            // 生成する範囲の調整
            float half_genRange = arrowGenerator.stageWidth / 2;
            arrowGenerator.GeneratePattern01(UnityEngine.Random.Range(-half_genRange, half_genRange), 1, 0);

            waitTimes[(int)coolTimeID] = 0;
        }
    }
}
