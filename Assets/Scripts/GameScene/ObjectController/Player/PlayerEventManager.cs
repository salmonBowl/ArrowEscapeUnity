using System;
using UnityEngine;

public class PlayerEventManager : SingletonBase<PlayerEventManager>
{

    public event Action OnAttacked;
    public event Action OnChargeFulled;

    public void Event(string eventName)
    {
        switch (eventName)
        {
            case "Attacked":

                OnAttacked?.Invoke();

                break;

            case "ChargeFulled":

                OnChargeFulled?.Invoke();

                break;

            default:
                Debug.LogError($"Event \"{eventName}\" が無効です");
                break;
        }
    }
}
