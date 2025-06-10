using UnityEngine;
using UnityEngine.UI;

public class RetryGame : MonoBehaviour
{
    [SerializeField] Image panel;
    public float panelalpha;
    public float panelalpha_max = 0.5f;

    void Start()
    {
        panelalpha = 0;
    }

    void Update()
    {
        panelalpha *= 0.9f;
        panel.color = new Color(1, 0, 0, panelalpha);
    }
}
