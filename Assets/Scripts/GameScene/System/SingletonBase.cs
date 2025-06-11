/*
    SingletonBase.cs
 */
using UnityEngine;

/// <summary>
/// Singletonパターンを実装ための基底クラスです
/// このクラスを継承するとInstance()が使えるようになります
/// </summary>
public abstract class SingletonBase<T> where T : class, new()
{
    public static T Instance()
    {
        instance ??= new T();
        return instance;
    }

    private static T instance;
}
