using System;

public class AttackPatternFactory
{
    public static AttackPatternBase Create(AttackPatternConfig config, ArrowGenerator arrowGenerator, BeamGenerator beamGenerator)
    {
        return config.PatternType switch
        {
            PatternType.PallarelArrow => new PatternPallarelArrow(arrowGenerator, config.ExecuteCondition, config.RandomJudge, config.CoolID, config.IsCenter),
            PatternType.EmissionArrow => new PatternEmissionArrow(arrowGenerator, config.ExecuteCondition, config.RandomJudge, config.CoolID),
            PatternType.Beam => new PatternBeam(beamGenerator, config.ExecuteCondition, config.RandomJudge, config.CoolID, config.RandomJudgeMoreover),
            PatternType.ArrowBom => new PatternArrowBom(arrowGenerator, config.ExecuteCondition, config.RandomJudge, config.CoolID),
            PatternType.SingleArrow => new PatternSingleArrow(arrowGenerator, config.ExecuteCondition, config.RandomJudge, config.CoolID),
            _ => throw new ArgumentException("AttackPatternFactoryに定義されていないPatternTypeが渡されました")
        };
    }
}
