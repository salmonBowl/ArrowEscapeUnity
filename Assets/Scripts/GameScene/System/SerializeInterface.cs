using System;
using UnityEngine;

public class SerializeInterface<TInterface>
{
    [SerializeField]
    private GameObject gameObjectContainInterface;

    private TInterface myInterface;

    public TInterface Interface()
    {
        if (myInterface == null)
        {
            myInterface = gameObjectContainInterface.GetComponent<TInterface>();
            if (myInterface == null)
            {
                Debug.LogError($"\"{gameObjectContainInterface.name}\" に指定のインターフェイスがありません");
            }
        }
        return myInterface;
    }
}
