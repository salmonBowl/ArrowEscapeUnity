using UnityEngine;
using UnityEngine.UI;

public class ThankYouForPlaying : MonoBehaviour
{
    [SerializeField] Text textbox;
    [SerializeField] string text;
    /// <summary>
    /// textを徐々に表示する速度を指定します
    /// 1文字あたりのフレーム数
    /// </summary>
    [SerializeField] int textDispSpeed;
    int text_charCount;
    int text_nowIndex = -1;
    int frameCountUp = -90;


    void Start()
    {
        text_charCount = text.Length;
    }

    void Update()
    {
        if (GamePhase.Instance().IsPerfectClear)
        {
            TextIncrease();   
        }
    }
    void TextIncrease()
    {
        frameCountUp++;
        if (textDispSpeed <= frameCountUp)
        {
            frameCountUp = 0;
            text_nowIndex++;
            if (0 < text_charCount - text_nowIndex)
            {
                textbox.text += text[text_nowIndex];
            }
        }
    }
}
