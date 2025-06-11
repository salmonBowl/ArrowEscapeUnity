using System;

public class UpdateManager : SingletonBase<UpdateManager>
{
    // 色々な条件つきのUpdate
    
    public event Action OnUpdate;

    public event Action OnUpdateWhileGame;
    public event Action OnUpdateIfNotTitle;

    void Update()
    {
        OnUpdate?.Invoke();

        if (GamePhase.Instance().IsGame)
        {
            OnUpdateWhileGame?.Invoke();
        }
        if (!GamePhase.Instance().IsTitle)
        {
            OnUpdateIfNotTitle?.Invoke();
        }
    }
}
