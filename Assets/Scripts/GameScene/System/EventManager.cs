using System;

public class EventManager : SingletonBase<EventManager>
{
    public enum Status
    {
        Title,
        Game,
        GameClear,
        GamePerfectClear,
        ThankYouForPlaying
    }
    public Status CurrentPhase = Status.Title;

    public event Action OnGameplayStart;
    public event Action OnRetry;
    public event Action OnGameClear;
    public event Action OnPerfectClear;
    public event Action OnGameCredit;
}
