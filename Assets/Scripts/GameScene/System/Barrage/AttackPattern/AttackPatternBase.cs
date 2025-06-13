using System;
using System.Collections.Generic;

public abstract class AttackPatternBase
{
    protected Func<bool> executeCondition;
    protected Func<bool> randomJudge;
    protected int waitTimeIndex;

    protected void Init(Func<bool> executeCondition, Func<bool> randomJudge, int waitTimeIndex)
    {
        this.executeCondition = executeCondition;
        this.randomJudge = randomJudge;
        this.waitTimeIndex = waitTimeIndex;
    }
    public abstract void Execute(List<float> waitTimes);
}
