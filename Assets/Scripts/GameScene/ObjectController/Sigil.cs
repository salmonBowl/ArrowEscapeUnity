using UnityEngine;

public class Sigil : MonoBehaviour
{
    public Stage stage;
    public GameObject sword;
    public GameObject playButton;

    void Start()
    {
        stage = GameObject.Find("background").GetComponent<Stage>();
        Disp();
    }

    void Update()
    {
        Disp();
    }
    public void Disp()
    {
        if (stage.IsStart)
        {
            // swordを表示
            sword.SetActive(true);
            playButton.SetActive(false);
        }
        else
        {
            // playButtonを表示
            sword.SetActive(false);
            playButton.SetActive(true);
        }
    }
}
