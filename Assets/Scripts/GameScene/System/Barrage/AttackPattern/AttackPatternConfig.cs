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
