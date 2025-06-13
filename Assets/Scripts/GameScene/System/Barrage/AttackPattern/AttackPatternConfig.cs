using System;
using System.Collections.Generic;

public enum CoolTimeID // coolTimeにはいくつかのスレッドがあります
{
    Slot0, // PallarelArrow, EmissionArrow
    Slot1, // Beam
    Slot2, // ArrowBom, EmissionArrow
    Slot3  // SingleArrow
}
public enum PatternType
{
    PallarelArrow,
    EmissionArrow,
    Beam,
    ArrowBom,
    SingleArrow,
}

public class AttackPatternConfig
{
    public Func<bool> ExecuteCondition;
    public Func<bool> RandomJudge;
    public CoolTimeID CoolID;

    public PatternType PatternType; // 0:PallarelArrow, 1:EmissionArrow, 2:Beam, 3:ArrowBom, 4:SingleArrow


    // 特殊なパラメーター
    public bool IsCenter; // PallarelArrow専用
    public Func<bool> RandomJudgeMoreover; // Beam専用
}
