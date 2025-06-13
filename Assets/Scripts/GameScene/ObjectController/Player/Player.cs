/*
    Player.cs
        Playerを動かします

        処理
        ・jumpChargeに合わせてプレイヤーの色を変える
        ・jumpChargeが溜まったらエフェクトを出す
        ・swordを取ってボスに攻撃
 */

using UnityEngine;
using System;

[RequireComponent(typeof(PlayerMove))]
public class Player : MonoBehaviour
{
    [Header("依存関係")]
    [SerializeField] PlayerMove mover;

    [Space(20)]

    [SerializeField] GameObject sword;
    [SerializeField] float swordCircleCol_radius;
    [SerializeField] float attackInterval;

    float swordCoolTime;

    void Start()
    {
        swordCoolTime = attackInterval;

        UpdateManager.Instance.OnUpdate += MyUpdate;
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
        if (GetSword())
        {
            AttackOnSword();
        }
    }

    // swordを取ったらボスに攻撃
    void AttackOnSword()
    {
        // swordに触れた瞬間を処理したい
        if (sword.activeSelf == false)
        {

            return;
        }

        if (GamePhase.Instance().IsTitle)
        {
            EventManager.Instance().Event("GameplayStart");
        }
        else
        {
            PlayerEventManager.Instance().Event("Attacked");
        }

        sword.SetActive(false);

        swordCoolTime = attackInterval;
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
}
