/*
    Player.cs
        Playerを動かします

        処理
        ・カーソルキー入力を取得
        ・移動
        ・ジャンプ、2段ジャンプ
        ・重力で落下
        ・jumpChargeに合わせてプレイヤーの色を変える
        ・jumpChargeが溜まったらエフェクトを出す
        ・swordを取ってボスに攻撃
 */

using UnityEngine;
using System;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerHitpoint playerhp;
    [Space(20)]
    [SerializeField] GameObject sword;
    [SerializeField] float swordCircleCol_radius;
    [SerializeField] float attackInterval;
    [Space(20)]
    ///<summary>
    ///</summary>
    [SerializeField] float invincibleTime;
    [SerializeField] float blinking_alpha;

    float swordCoolTime;

    float invincible_timeCount = 0;
    public float player_colorAlpha = 1;

    public static event Action OnAttacked;

    PlayerMove mover;

    void Start()
    {
        swordCoolTime = attackInterval;
        mover = GetComponent<PlayerMove>();
    }
    void OnEnable()
    {
        UpdateManager manager = UpdateManager.Instance();
        manager.OnUpdate += MyUpdate;
    }

    void MyUpdate()
    {
        // プレイヤーの移動に関する処理
        mover.MoveUpdate();

        // 攻撃関係の処理
        swordCoolTime = Mathf.Max(0, swordCoolTime - Time.deltaTime);
        if(swordCoolTime == 0)
        {
            if (GamePhase.Instance().IsGame)
            {
                sword.SetActive(true);
            }
        }
        AttackOnSword();

        // 被弾関係
        // この変数が0になるまで無敵時間が続く
        invincible_timeCount = Mathf.Max(0, invincible_timeCount - Time.deltaTime);
        if (invincible_timeCount == 0)
        {
            StopAllCoroutines();
            player_colorAlpha = 1;
        }
    }

    // swordを取ったらボスに攻撃
    void AttackOnSword()
    {
        if (GetSword())
        {
            // swordに触れた瞬間を処理したい
            if (sword.activeSelf == false)
            {
                
                return;
            }

            if (GamePhase.Instance().IsTitle)
            {
                EventManager.Instance().Event("GameStart");
            }
            else
            {
                OnAttacked?.Invoke();
            }

            sword.SetActive(false);

            swordCoolTime = attackInterval;
        }
    }

    // swordに当たっているかどうかを返す
    bool GetSword()
    {
        Vector2 my = transform.position;
        Vector2 sw = sword.transform.position;
        Vector2 dif = sw - my;
        float radius_player = 1.0f;
        float radius_sword = swordCircleCol_radius;

        float distance = Mathf.Sqrt(dif.x * dif.x + dif.y * dif.y);

        return distance < radius_player + radius_sword;
    }

    // 被弾
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (invincible_timeCount != 0)
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
        invincible_timeCount = invincibleTime;

        StartCoroutine(Blink());
    }

    // 無敵時間の点滅
    IEnumerator Blink()
    {
        while (true)
        {
            player_colorAlpha = blinking_alpha;
            yield return new WaitForSeconds(0.15f);
            player_colorAlpha = 1;
            yield return new WaitForSeconds(0.15f);
        }
    }
}
