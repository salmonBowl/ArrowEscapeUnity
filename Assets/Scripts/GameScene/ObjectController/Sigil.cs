using UnityEngine;

public class Sigil : MonoBehaviour
{
    [SerializeField]
    GameObject sword;

    [SerializeField]
    GameObject playButton;

    void Start()
    {
        EventManager.Instance().OnGameplayStart += ChangeDispToSword;

        // タイトルではplayButtonを表示
        sword.SetActive(false);
        playButton.SetActive(true);
    }

    void ChangeDispToSword()
    {
        // swordを表示
        sword.SetActive(true);
        playButton.SetActive(false);
    }
}
