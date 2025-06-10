
public class GamePhase : SingletonBase<GamePhase>
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

    // GamePhase.Instance().CurrentPhase == GamePhase.Status.[] と呼び出すのが冗長だったので追加
    public bool IsTitle => CurrentPhase == Status.Title;
    public bool IsGame => CurrentPhase == Status.Game;
    public bool IsGameClear => CurrentPhase == Status.GameClear;
    public bool IsPerfectClear => CurrentPhase == Status.GamePerfectClear;
    public bool IsGameCredit => CurrentPhase == Status.ThankYouForPlaying;
    public bool IsGameEnd =>
        CurrentPhase == Status.GameClear ||
        CurrentPhase == Status.GamePerfectClear ||
        CurrentPhase == Status.ThankYouForPlaying;
}
