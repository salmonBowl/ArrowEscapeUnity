using UnityEngine;

public class PlayerJumpChargeCompleted : MonoBehaviour
{
    [SerializeField] SpriteRenderer particle_illum;

    void Start() => PlayerEventManager.Instance().OnChargeFulled += DispParticle;

    void Update()
    {
        AttenuateParticle();
    }

    // Update内でパーティクルを減衰
    void AttenuateParticle()
    {
        float illumdecrease = 0.05f;
        Color particleColor = particle_illum.color;
        particleColor.a = Mathf.Max(particleColor.a - illumdecrease, 0);
        particle_illum.color = particleColor;
    }
    void DispParticle()
    {
        particle_illum.color = new Color(1, 1, 1, 1);
    }
}
