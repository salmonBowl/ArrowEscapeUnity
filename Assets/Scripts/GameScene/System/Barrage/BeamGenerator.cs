using UnityEngine;

public class BeamGenerator : MonoBehaviour
{
    [SerializeField] GameObject Beam;
    [SerializeField] float graceTime;

    public void GenerateBeam(float hight)
    {
        Beam beam = Instantiate(Beam).GetComponent<Beam>();
        beam.beam_hight = hight;
        beam.graceTime = graceTime;
    }
}
