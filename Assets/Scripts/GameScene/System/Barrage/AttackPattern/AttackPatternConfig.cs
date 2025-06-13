using System;
public enum CoolTimeID // coolTimeにはいくつかのスレッドがあります
{
    Slot0, // PallarelArrow, EmissionArrow
    Slot1, // Beam
    Slot2, // ArrowBom, EmissionArrow
    Slot3  // SingleArrow
}
public class AttackPatternConfig
{
    public float TriggerHPThreshold;
    public float Probability;
    public float Cooldown;
    public CoolTimeID CoolSlot;

    public AttackType PatternType;
    public bool IsCenter; // PatternPallarelArrow専用
}
