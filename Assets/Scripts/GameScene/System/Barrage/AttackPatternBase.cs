using System;
using System.Collections.Generic;

public abstract class AttackPatternBase
{
    protected Func<bool> randomJudge;
    protected CoolTimeManager timeManager;
    protected CoolTimeID coolTimeID;

    protected void Init(Func<bool> randomJudge, CoolTimeManager timeManager, CoolTimeID coolTimeID)
    {
        this.randomJudge = randomJudge;
        this.timeManager = timeManager;
        this.coolTimeID = coolTimeID;
    }
    public abstract void Execute();
}
