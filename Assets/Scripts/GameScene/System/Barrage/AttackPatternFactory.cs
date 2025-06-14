using System;
using UnityEngine;

public enum PatternType
{
    PallarelArrow_Center,
    PallarelArrow_Normal,
    EmissionArrow,
    Beam,
    ArrowBom,
    SingleArrow,
}
public class AttackPatternFactory : MonoBehaviour
{
    [Header("以下の攻撃パターンを使います")]
    [SerializeField] ArrowGenerator arrowGenerator;
    [SerializeField] BeamGenerator beamGenerator;


    public AttackPatternBase Create(PatternType patternType, CoolTimeManager timeManager)
    {
        return patternType switch
        {
            PatternType.PallarelArrow_Center => new PatternPallarelArrow(arrowGenerator, () => Fixed_Probability(80), timeManager, true),
            PatternType.PallarelArrow_Normal => new PatternPallarelArrow(arrowGenerator, () => Fixed_Probability(80), timeManager, false),
            PatternType.EmissionArrow => new PatternEmissionArrow(arrowGenerator, () => Fixed_Probability(80), timeManager),
            PatternType.Beam => new PatternBeam(beamGenerator, () => Fixed_Probability(80), timeManager, () => Fixed_Probability(13)),
            PatternType.ArrowBom => new PatternArrowBom(arrowGenerator, () => Fixed_Probability(140), timeManager),
            PatternType.SingleArrow => new PatternSingleArrow(arrowGenerator, () => Fixed_Probability(50), timeManager),
            _ => throw new ArgumentException("AttackPatternFactoryに定義されていないPatternTypeが渡されました")
        };
    }

    /*
        ランダムに関する関数
        1/numの確率でtrueを返す
     */
    bool Fixed_Probability(int num)
    {
        int rand_value = UnityEngine.Random.Range(0, num);
        if (rand_value == 1)
        {
            return true;
        }
        return false;
    }
}
