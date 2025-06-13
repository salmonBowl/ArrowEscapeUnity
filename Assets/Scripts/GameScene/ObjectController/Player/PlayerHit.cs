using System;
using UnityEngine;

[RequireComponent(typeof(PlayerVisualBlinking))]
public class PlayerHit : MonoBehaviour
{
    [Header("依存関係")]
    [SerializeField] PlayerHealthpoint playerhp;
    [SerializeField] PlayerVisualBlinking blink;

    [Space(20)]

    ///<summary>
    ///プレイヤーの無敵時間を設定します
    ///</summary>
    [SerializeField] float setInvincibleSec;

    public float InvincibleRemaindSec { get; private set; } = 0;


    void Update()
    {
        InvincibleRemaindSec -= Time.deltaTime;
        if ((InvincibleRemaindSec = Mathf.Max(InvincibleRemaindSec, 0)) <= 0)
        {
            InvincibleRemaindSec = 0;

            blink.StopBlinking();
        }
    }

    // 被弾
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (InvincibleRemaindSec != 0)
        {
            return;
        }
        // 無敵時間がない時

        // collisionのtagがこの中にあれば下の処理を実行
        string colTag = collision.gameObject.tag;
        string[] tags = { "Arrow", "Beam", "Bom" };

        int index = Array.IndexOf(tags, colTag);

        if (index == -1)
        {
            return;
        }
        if (colTag == tags[index])
        {
            playerhp.Damage(index);
        }

        // 被弾して無敵時間ができる
        InvincibleRemaindSec = setInvincibleSec;

        blink.StartBlinking();
    }
}
