using System.Collections.Generic;
using UnityEngine;

public class PatternPallarelArrow : IAttackPattern
{
    private readonly ArrowGenerator arrowGenerator;
    private readonly bool center;
    private readonly List<float> waitTime;
    private readonly System.Func<bool> randomJudge;

    public PatternPallarelArrow(bool center, ArrowGenerator arrowGenerator, List<float> waitTime, System.Func<bool> randomJudge)
    {
        this.center = center;
        this.arrowGenerator = arrowGenerator;
        this.waitTime = waitTime;
        this.randomJudge = randomJudge;
    }

    /*
        1/80の確率でパターン1の生成、また1.5秒のクールタイムがある
     */
    public void Execute()
    {
        // 1/80の確率でパターン1の生成
        if (waitTime[0] == 0)
        {
            if (randomjudge())
            {
                // 5個のArrowを0.9の間隔で
                int quantity = 5;
                float arrowGap = 0.9f;

                // 生成する範囲の調整
                float halfGenRange = ((center ? 0 : arrowGenerator.stageWidth) - (arrowGap * quantity)) / 2;

                float genPosX = UnityEngine.Random.Range(-halfGenRange, halfGenRange);
                arrowGenerator.GeneratePattern01(genPosX, quantity, arrowGap);

                waitTime[0] = 1.5f;
            }
        }
    }
}
