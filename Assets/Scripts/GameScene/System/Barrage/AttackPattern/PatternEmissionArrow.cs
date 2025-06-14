using System;
using System.Collections.Generic;

public class PatternEmissionArrow : AttackPatternBase
{
    private readonly ArrowGenerator arrowGenerator;

    public PatternEmissionArrow(ArrowGenerator arrowGenerator, Func<bool> randomJudge, CoolTimeManager timeManager, CoolTimeID coolTimeID)
    {
        Init(randomJudge, timeManager, coolTimeID);

        this.arrowGenerator = arrowGenerator;
    }

    public override void Execute()
    {
        if (!timeManager.IsReady(CoolTimeID.Slot0)) return;

        // Update内で確率を引くと実行される
        if (randomJudge())
        {
            // Player方向に7個のArrow
            int quantity = 7;
            float anglerange = 2.1f; // 0～2π
                                     // プレイヤーの向きからランダムな角度に回す時に(-range/2, range/2)で計算する

            // 生成する範囲の調整
            float half_genRange = arrowGenerator.stageWidth / 2;
            arrowGenerator.GeneratePattern02(UnityEngine.Random.Range(-half_genRange, half_genRange), quantity, anglerange);

            timeManager.Reset(CoolTimeID.Slot0, 1.5f);
            timeManager.Reset(CoolTimeID.Slot1, 1.5f);
        }
    }
}
