using System;
using System.Collections.Generic;

public class PatternPallarelArrow : AttackPatternBase
{
    private readonly ArrowGenerator arrowGenerator;
    private readonly bool center;

    public PatternPallarelArrow(ArrowGenerator arrowGenerator, Func<bool> randomJudge, CoolTimeID coolTimeID, bool center)
    {
        Init(randomJudge, coolTimeID);

        this.arrowGenerator = arrowGenerator;
        this.center = center;
    }

    /*
        1/80の確率でパターン1の生成、また1.5秒のクールタイムがある
     */
    public override void Execute(List<float> waitTimes)
    {
        if (waitTimes[0] != 0) return;

        // Update内で確率を引くと実行される
        if (randomJudge())
        {
            // 5個のArrowを1.5の間隔で
            int quantity = 5;
            float arrowGap = 3.8f;

            // 生成する範囲の調整
            float halfGenRange = ((center ? 0 : arrowGenerator.stageWidth) - (arrowGap * quantity)) / 2;

            float genPosX = UnityEngine.Random.Range(-halfGenRange, halfGenRange);
            arrowGenerator.GeneratePattern01(genPosX, quantity, arrowGap);

            waitTimes[0] = 1.5f;
        }
    }
}
