using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField, Header("ステージの情報を取得します")]
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
    PlayerJumpCharge jumpcharge;

    [SerializeField, Header("")]
    float jumpChargeSpeed;

    private float stageWidth;
    private float groundLevel;
    private Vector2 gravityForce;

    Vector2 myVelocity = new();

    [SerializeField] PlayerInput input;

    void Awake()
    {
        stageWidth = background.Width;
        groundLevel = background.GroundLevel;
        gravityForce = background.GravityForce;
    }

    // プレイヤーの座標に関する処理をまとめた
    public void MoveUpdate()
    {
        //Debug.Log("PlayerMove.MoveUpdate()");

        input.ReadInput();
        float inputX = input.X;
        bool jumpTriggered = input.JumpTriggered;

        // 更新前と更新後のPlayerの座標
        Vector2 myPosition = transform.position;
        Vector2 update_position = myPosition;

        //Debug.Log("Grounded : " + Grounded(myPosition));

        if (Grounded(myPosition)) //地上
        {
            // velocityの更新
            myVelocity.x = inputX * speed;
            myVelocity.y = 0;

            //Debug.Log("Player grounded");

            jumpcharge.SetValue(Mathf.Min(jumpcharge.Value + jumpChargeSpeed, 1));

            if (jumpTriggered)
            {
                // 通常のジャンプ
                Jump(true);
            }
        }
        else //空中
        {
            // velocityの更新

            /*
            // 空中では加速度的な移動
            velocity.x = speed_x * Mathf.Clamp(velocity.x + (input_x * 0.01f), -1, 1);
            */
            // 空中でも等速的な移動
            myVelocity.x = inputX * speed;

            myVelocity += gravityForce;


            if (jumpTriggered)
            {
                // 2段ジャンプ
                Jump(false);
            }
        }

        // Playerの移動処理

        update_position += myVelocity * Time.deltaTime;
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
    void Jump(bool grounded)
    {
        // 通常のジャンプ処理
        if (grounded)
        {
            // ジャンプによりjumpChargeを最大0.5だけ消費

            myVelocity += Mathf.Min(jumpcharge.Value, 0.5f) * jumpPower * Vector2.up;
            jumpcharge.SetValue(Mathf.Max(jumpcharge.Value - 0.5f, 0));
        }
        else // 2段ジャンプ
        {
            if (0.4f < jumpcharge.Value)
            {
                myVelocity = 0.5f * jumpPower * Vector2.up;
                jumpcharge.SetValue(0);
            }
        }
    }

    // 地面に接地しているかを返す関数
    bool Grounded(Vector2 myPosition)
    {
        // Playerと地面の高さの差
        float playerHight = myPosition.y - adhesCorrect_y;

        // Playerが地面の高さ以下ならtrueを返す
        float adjust = 0.001f;
        return playerHight - adjust < groundLevel;
    }
}
