using System;
using UnityEngine;

public class EventManager : SingletonBase<EventManager>
{

    public event Action OnGameplayStart;
    public event Action OnRetry;
    public event Action OnGameClear;
    //public event Action OnPerfectClear;
    public event Action OnGameCredit;

    public void Event(string eventName)
    {
        GamePhase phase = GamePhase.Instance();
        switch (eventName)
        {
            case "GameplayStart":

                phase.CurrentPhase = GamePhase.Status.Game;
                OnGameplayStart?.Invoke();

                break;

            case "Retry":

                phase.CurrentPhase = GamePhase.Status.Game;

                OnRetry?.Invoke();

                break;

            case "GameClear":

                phase.CurrentPhase = GamePhase.Status.GameClear;
                OnGameClear?.Invoke();

                break;

            case "ChangeClearModePerfect":

                phase.CurrentPhase = GamePhase.Status.GamePerfectClear;
                //OnPerfectClear?.Invoke();

                break;

            case "GameCredit":

                phase.CurrentPhase = GamePhase.Status.ThankYouForPlaying;
                OnGameCredit?.Invoke();

                break;

            default:
                Debug.LogError($"Event \"{eventName}\" が無効です");
                break;
        }
    }
}
