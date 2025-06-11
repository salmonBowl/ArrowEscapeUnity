using UnityEngine;
using UnityEngine.UI;

public class TextTypingDisp : IUpdatable
{
    private readonly Text textbox;
    private readonly string message;

    private readonly int charsPerFrame;
    private int currentIndex;
    private int frameCounter;

    private bool isPlayFinished = false;

    public TextTypingDisp(Text textbox, string message)
    {
        textbox.text = string.Empty;
        this.textbox = textbox;
        this.message = message;

        charsPerFrame = 1;
        currentIndex = 0;
        frameCounter = 0;
    }
    public TextTypingDisp(Text textbox, string message, int charsPerFrame) : this(textbox, message)
    {
        this.charsPerFrame = charsPerFrame;
    }
    public TextTypingDisp(Text textbox, string message, int charsPerFrame, int waitFrame) : this(textbox, message, charsPerFrame)
    {
        frameCounter = -waitFrame;
    }

    public void Update()
    {
        if (isPlayFinished) return;

        if (IsFrameFilled())
        {
            TextIncrease();
        }
    }

    bool IsFrameFilled()
    {
        // counterが閾値に満たないとき
        if (++frameCounter < charsPerFrame)
            return false;

        frameCounter = 0;
        return true;
    }

    void TextIncrease()
    {
        string slicedMessage = message[..Mathf.Min(currentIndex, message.Length)];
        currentIndex++; // currentIndexは初期値が0のため後ろに書いています

        textbox.text = slicedMessage;
        isPlayFinished = slicedMessage == message;
    }
}
