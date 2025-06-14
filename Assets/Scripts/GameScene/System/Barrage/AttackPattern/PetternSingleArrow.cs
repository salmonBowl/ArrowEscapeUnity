using System;

public class PatternSingleArrow : AttackPatternBase
{
    readonly ArrowGenerator arrowGenerator;

    public PatternSingleArrow(ArrowGenerator arrowGenerator, Func<bool> randomJudge, CoolTimeManager timeManager)
    {
        Init(randomJudge, timeManager);

        this.arrowGenerator = arrowGenerator;
    }

    public override void Execute()
    {
        if (!timeManager.IsReady(CoolTimeID.Slot3)) return;

        // Update内で確率を引くと実行される
        if (randomJudge())
        {
            // 生成する範囲の調整
            float half_genRange = arrowGenerator.stageWidth / 2;
            arrowGenerator.GeneratePattern01(UnityEngine.Random.Range(-half_genRange, half_genRange), 1, 0);

            timeManager.Reset(CoolTimeID.Slot3, 0f);
        }
    }
}
