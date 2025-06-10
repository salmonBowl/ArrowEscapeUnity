/*
    StageInfo.cs
        ステージの情報をインスペクター上で設定します
 */

using UnityEngine;

public class Stage : MonoBehaviour
{
    // Inspector fields
    [Header("")]

    [Tooltip("Horizontal speed")]
    [SerializeField]
    private float width;

    [SerializeField, Header("地面判定の高さ")]
    private float groundLevel;

    [SerializeField, Header("重力 (大きさ)")]
    private float gravity_volume;

    /// <summary>
    /// backgroundの横幅
    /// </summary>
    internal float Width => width;
    /// <summary>
    /// 地面の高さ
    /// </summary>
    internal float GroundLevel => groundLevel;
    /// <summary>
    /// 重力ベクトル
    /// </summary>
    internal Vector2 GravityForce => new(0, -gravity_volume);

    /// <summary>
    /// falseならPlayボタンが出る
    /// </summary>
    internal bool IsStart { get; set; } = false;
    internal bool IsClear { get; set; } = false;
    internal bool IsPerfectClear { get; set; } = false;

    internal void GameStart() { IsStart = true; }
    internal void Clear() { IsClear = true;}
    internal void Retry() { IsClear = false; }
    internal void PerfectClear() { IsPerfectClear = true; }
}
