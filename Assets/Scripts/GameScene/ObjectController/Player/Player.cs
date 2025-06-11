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
    [Header("他スクリプトからの情報取得用")]
    [SerializeField] Background stage;
    [SerializeField] PlayerHitpoint playerhp;
    [SerializeField] JumpCharge jumpcharge;
    [Space(20)]
    [SerializeField] float playerWidth;
    [SerializeField] float adhesCorrect_y;
    [SerializeField] SpriteRenderer particle_illum;
    [SerializeField] float speed_x;
    [SerializeField] float ratio_airmove;
    [SerializeField] float jumpPower;
    [SerializeField] float jumpChargeSpeed;
    [Space(20)]
    [SerializeField] GameObject sword;
    [SerializeField] float swordCircleCol_radius;
    [SerializeField] float attackInterval;
    [Space(20)]
    ///<summary>
    ///</summary>
    [SerializeField] float invincibleTime;
    [SerializeField] float blinking_alpha;

    readonly float stageWidth;
    readonly float groundLevel;
    readonly Vector2 gravityForce;

    Vector2 myVelocity;

    float swordCoolTime;

    float invincible_timeCount = 0;
    float player_colorAlpha = 1;

    SpriteRenderer playerSpriteRenderer;

    public static event Action OnAttacked;

    Player()
    {
        stageWidth = stage.Width;
        groundLevel = stage.GroundLevel;
        gravityForce = stage.GravityForce;
    }

    void Start()
    {
        swordCoolTime = attackInterval;
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnEnable()
    {
        UpdateManager manager = UpdateManager.Instance();
        manager.OnUpdate += MyUpdate;
    }
    void MyUpdate()
    {
        // パーティクルを減衰
        float illumdecrease = 0.05f;
        Color pcolor = particle_illum.color;
        particle_illum.color -= new Color(0, 0, 0, illumdecrease + Mathf.Min(0, pcolor.a - illumdecrease));

        // jumpChargeに合わせてPlayerの色を黄色に
        playerSpriteRenderer.color = jumpcharge.Value != 0 ? new Color(1, 1, 1 - (0.6f * jumpcharge.Value), player_colorAlpha)
            : new Color(1, 1, 1, player_colorAlpha);
        //jumpcharge.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 0.7f * player_colorAlpha);

        // プレイヤーの座標に関する処理
        Update_PlayerMove();

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

    // プレイヤーの座標に関する処理をまとめた
    void Update_PlayerMove()
    {
        // 更新前と更新後のPlayerの座標
        Vector2 myPosition = transform.position;
        Vector2 update_position = myPosition;

        // カーソルキーの入力
        float input_x = Input_vh(false).x;
        float input_y = Input_vh(true).y;
        Debug.Log($"input_x : {input_x}");

        // velocityの更新
        if (Grounded(myPosition))
        {
            // 地上では入力がそのまま移動
            myVelocity.x = input_x * speed_x;
            myVelocity.y = 0;

            // ジャンプに関する処理
            Jump(input_y, true);
        }
        else
        {
            /*
            // 空中では加速度的な移動
            velocity.x = speed_x * Mathf.Clamp(velocity.x + (input_x * 0.01f), -1, 1);
            */
            // 空中でも等速的な移動
            myVelocity.x = input_x * speed_x;

            myVelocity += gravityForce;

            // 2段ジャンプに関する処理
            Jump(input_y, false);
        }

        // Playerの移動処理

        update_position += myVelocity;
        // 位置補正
        // 画面外に行くならxを補正
        float moverange_half = (stageWidth - playerWidth) / 2;
        update_position.x = Mathf.Clamp(update_position.x, -moverange_half, moverange_half);
        // 地面にめり込むならyを補正
        if (Grounded(update_position))
        {
            update_position.y = groundLevel + adhesCorrect_y;
        }
        transform.position = update_position;
    }

    /*
        Jump(キー入力、地面に接地しているか)
            ジャンプをする関数ではなくUpdateの中で動く、ジャンプに関する色々な処理
            grondedがfalseだと2段ジャンプの挙動になります
     */
    void Jump(float input_y, bool grounded)
    {
        // jumpCharge
        if (grounded)
        {
            float old_jumpCharge = jumpcharge.Value;
            jumpcharge.Value = Mathf.Min(jumpcharge.Value + jumpChargeSpeed, 1);

            // チャージが完了したらエフェクトを出す
            if (old_jumpCharge < 1 && jumpcharge.Value == 1)
            {
                particle_illum.color = new Color(1, 1, 1, 1);
            }
        }

        // ↑が押された時
        if (input_y == 1)
        {
            // 通常のジャンプ処理
            if (grounded)
            {
                // ジャンプによりjumpChargeを最大0.5だけ消費

                myVelocity += Mathf.Min(jumpcharge.Value, 0.5f) * jumpPower * Vector2.up;
                jumpcharge.Value -= Mathf.Min(jumpcharge.Value, 0.5f);
            }
            else // 2段ジャンプ
            {
                if (0.4f < jumpcharge.Value)
                {
                    myVelocity = 0.5f * jumpPower * Vector2.up;
                    jumpcharge.Value = 0;
                }
            }
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

    /*
        カーソルキーの入力を返す関数
            keydownをtrueにするとGetKeyDown,
            falseならGetKeyで入力をとる
        return : Vector2
            x成分には左右キーの入力 (-1 ～ 1)
            y成分には上下キーの入力 (-1 ～ 1)
                を返す
        追加 : WASDとスペースでもよし
    */
    Vector2 Input_vh(bool keydown)
    {
        int vx = 0;
        if ((keydown ?
             Input.GetKeyDown(KeyCode.RightArrow) :
             Input.GetKey(KeyCode.RightArrow))
         || (keydown ?
            Input.GetKeyDown(KeyCode.D) :
            Input.GetKey(KeyCode.D)))
        {
            vx++;
        }
        if ((keydown ?
             Input.GetKeyDown(KeyCode.LeftArrow) :
             Input.GetKey(KeyCode.LeftArrow))
         || (keydown ?
             Input.GetKeyDown(KeyCode.A) :
             Input.GetKey(KeyCode.A)))
        {
            vx--;
        }
        int vy = 0;
        if ((keydown ?
             Input.GetKeyDown(KeyCode.UpArrow) :
             Input.GetKey(KeyCode.UpArrow))
         || (keydown ?
             Input.GetKeyDown(KeyCode.W) :
             Input.GetKey(KeyCode.W))
          || (keydown && Input.GetKeyDown(KeyCode.Space)))
        {
            vy++;
        }
        if ((keydown ?
             Input.GetKeyDown(KeyCode.DownArrow) :
             Input.GetKey(KeyCode.DownArrow))
         || (keydown ?
             Input.GetKeyDown(KeyCode.S) :
             Input.GetKey(KeyCode.S)))
        {
            if (keydown && vy == 1)
            {
                // ↑と↓を同時入力した時は↑が優先される
            }
            else
            {
                vy--;
            }
        }

        return new Vector2(vx, vy);
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

    // 地面に接地しているかを返す関数
    bool Grounded(Vector2 myPosition)
    {
        // Playerと地面の高さの差
        float dif = myPosition.y - groundLevel;

        // Playerが地面の高さ以下ならfalseを返す
        float adjust = 0.1f;
        return dif - adhesCorrect_y < adjust;
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
