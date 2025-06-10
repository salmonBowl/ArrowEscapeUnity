using UnityEngine;

public class KeyGuide : MonoBehaviour
{
    [SerializeField] float when_display;
    [SerializeField] float display_time;

    float localtime = 0;

    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        localtime += Time.deltaTime;
        if (when_display < localtime)
        {
            gameObject.SetActive(true);
        }
    }
}
