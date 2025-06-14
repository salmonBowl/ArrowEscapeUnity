using System;

public abstract class AttackPatternBase
{
    protected Func<bool> randomJudge;
    protected CoolTimeManager timeManager;

    protected void Init(Func<bool> randomJudge, CoolTimeManager timeManager)
    {
        this.randomJudge = randomJudge;
        this.timeManager = timeManager;
    }
    public abstract void Execute();
}
