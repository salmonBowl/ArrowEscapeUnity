using System;
using System.Collections;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    // 色々な条件つきのUpdate
    
    public event Action OnUpdate;

    public event Action OnUpdateWhileTitle;
    public event Action OnUpdateWhileGame;
    public event Action OnUpdateWhileGameClear;
    public event Action OnUpdateWhilePerfectClear;
    public event Action OnUpdateWhileGameCredit;

    public event Action OnUpdateIfNotTitle;


    void Update()
    {
        OnUpdate?.Invoke();

        switch (GamePhase.Instance().CurrentPhase)
        {
            case GamePhase.Status.Title:
                OnUpdateWhileTitle?.Invoke();
                break;
            case GamePhase.Status.Game:
                OnUpdateWhileGame?.Invoke();
                break;
            case GamePhase.Status.GameClear:
                OnUpdateWhileGameClear?.Invoke();
                break;
            case GamePhase.Status.GamePerfectClear:
                OnUpdateWhilePerfectClear?.Invoke();
                break;
            case GamePhase.Status.ThankYouForPlaying:
                OnUpdateWhileGameCredit?.Invoke();
                break;
        }

        if (!GamePhase.Instance().IsTitle)
        {
            OnUpdateIfNotTitle?.Invoke();
        }
    }

    public static UpdateManager Instance { get; private set; }
    UpdateManager()
    {
        Instance = this;
    }
}
