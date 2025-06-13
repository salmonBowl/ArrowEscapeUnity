using System;
using System.Collections.Generic;

public abstract class AttackPatternBase
{
    protected AttackPatternConfig config;

    protected Func<bool> executeCondition;
    protected Func<bool> randomJudge;
    protected CoolTimeID coolTimeID;

    public AttackPatternBase(AttackPatternConfig config)
    {
        this.config = config;
    }

    protected void Init(Func<bool> executeCondition, Func<bool> randomJudge, CoolTimeID coolTimeID)
    {
        this.executeCondition = executeCondition;
        this.randomJudge = randomJudge;
        this.coolTimeID = coolTimeID;
    }
    public abstract void Execute(List<float> waitTimes);
}
