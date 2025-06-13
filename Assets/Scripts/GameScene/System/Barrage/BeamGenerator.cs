using UnityEngine;

public class BeamGenerator : MonoBehaviour
{
    [SerializeField] GameObject Beam;
    [SerializeField] float graceTime;

    public void GenerateBeam(float hight)
    {
        BeamController beam = Instantiate(Beam).GetComponent<BeamController>();
        beam.beam_hight = hight;
        beam.graceTime = graceTime;
    }
}
