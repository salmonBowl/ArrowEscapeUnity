using UnityEngine;
using UnityEngine.UI;

public class ThankYouForPlaying : MonoBehaviour
{
    [SerializeField] Text textbox;
    [SerializeField] string message;

    [Header("textを徐々に表示する速度、1文字あたりのフレーム数を指定します")]
    [SerializeField] int textDispSpeed;

    void OnEnable() => EventManager.Instance().OnGameCredit += TypingTextPlay;
    void OnDisable() => EventManager.Instance().OnGameCredit -= TypingTextPlay;

    TextTypingDisp textTypingDisp;
    void TypingTextPlay()
    {
        textTypingDisp = new TextTypingDisp(textbox, message, textDispSpeed);
    }
}
