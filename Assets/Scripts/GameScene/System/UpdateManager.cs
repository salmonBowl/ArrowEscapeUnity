using System;

public interface IUpdatable
{
    public void Update();
}
public class UpdateManager : SingletonBase<UpdateManager>
{
    public event Action OnUpdate;

    void Update()
    {
        OnUpdate?.Invoke();
    }
}
