using System;
using UnityEngine;

public class AttackPatternFactory : MonoBehaviour
{
    [Header("以下の攻撃パターンを使います")]
    [SerializeField] ArrowGenerator arrowGenerator;
    [SerializeField] BeamGenerator beamGenerator;


    public AttackPatternBase Create(PatternType patternType)
    {
        return patternType switch
        {
            PatternType.PallarelArrow_Center => new PatternPallarelArrow(arrowGenerator, () => Fixed_Probability(80), CoolTimeID.Slot0, true),
            PatternType.PallarelArrow_Normal => new PatternPallarelArrow(arrowGenerator, () => Fixed_Probability(80), CoolTimeID.Slot0, false),
            PatternType.EmissionArrow => new PatternEmissionArrow(arrowGenerator, () => Fixed_Probability(80), CoolTimeID.Slot0),
            PatternType.Beam => new PatternBeam(beamGenerator, () => Fixed_Probability(80), CoolTimeID.Slot1, () => Fixed_Probability(13)),
            PatternType.ArrowBom => new PatternArrowBom(arrowGenerator, () => Fixed_Probability(140), CoolTimeID.Slot2),
            PatternType.SingleArrow => new PatternSingleArrow(arrowGenerator, () => Fixed_Probability(50), CoolTimeID.Slot3),
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
