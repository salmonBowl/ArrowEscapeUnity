using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField, Header("Backgroundから情報を取得します")]
    Background background;

    [Header("移動範囲の制限のために使います")]
    [SerializeField] float playerWidth;
    [SerializeField] float adhesCorrect_y;

    [Space(20)]

    [SerializeField, Header("横移動のスピードを指定します")]
    float speed;

    [SerializeField, Header("空中での移動速度の割合を0～1で指定します")]
    float ratio_airmove;

    [Space(20)]

    [SerializeField, Header("最大のジャンプ力を指定します")]
    float jumpPower;

    [SerializeField, Header("ジャンプ力に関するクラスです")]
    JumpCharge jumpcharge;

    [SerializeField, Header("")]
    float jumpChargeSpeed;

    [Space(20)]
    [SerializeField] SpriteRenderer particle_illum;

    private readonly float stageWidth;
    private readonly float groundLevel;
    private readonly Vector2 gravityForce;

    public PlayerMove()
    {
        stageWidth = background.Width;
        groundLevel = background.GroundLevel;
        gravityForce = background.GravityForce;
    }

    Vector2 myVelocity = new();

    readonly PlayerInput input = new();


    // プレイヤーの座標に関する処理をまとめた
    public void MoveUpdate()
    {
        // パーティクルを減衰
        float illumdecrease = 0.05f;
        Color pcolor = particle_illum.color;
        particle_illum.color -= new Color(0, 0, 0, illumdecrease + Mathf.Min(0, pcolor.a - illumdecrease));


        input.ReadInput();
        float inputX = input.X;
        bool jumpPressed = input.JumpPressed;

        // 更新前と更新後のPlayerの座標
        Vector2 myPosition = transform.position;
        Vector2 update_position = myPosition;

        // velocityの更新
        if (Grounded(myPosition))
        {
            // 地上では入力がそのまま移動
            myVelocity.x = inputX * speed;
            myVelocity.y = 0;

            // ジャンプに関する処理
            Jump(jumpPressed, true);
        }
        else
        {
            /*
            // 空中では加速度的な移動
            velocity.x = speed_x * Mathf.Clamp(velocity.x + (input_x * 0.01f), -1, 1);
            */
            // 空中でも等速的な移動
            myVelocity.x = inputX * speed;

            myVelocity += gravityForce;

            // 2段ジャンプに関する処理
            Jump(jumpPressed, false);
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
    void Jump(bool jumpPressed, bool grounded)
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
        if (jumpPressed)
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

    // 地面に接地しているかを返す関数
    bool Grounded(Vector2 myPosition)
    {
        // Playerと地面の高さの差
        float dif = myPosition.y - groundLevel;

        // Playerが地面の高さ以下ならfalseを返す
        float adjust = 0.1f;
        return dif - adhesCorrect_y < adjust;
    }
}
