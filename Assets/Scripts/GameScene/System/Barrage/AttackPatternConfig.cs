using System;

public enum PatternType
{
    PallarelArrow_Center,
    PallarelArrow_Normal,
    EmissionArrow,
    Beam,
    ArrowBom,
    SingleArrow,
}

public class AttackPatternConfig
{
    public PatternType PatternType;
    public Func<bool> RandomJudge;
    public CoolTimeID CoolTimeID;


    // 特殊なパラメーター
    public bool IsCenter; // PallarelArrow専用
    public Func<bool> RandomJudgeMoreover; // Beam専用
}
