/*
    ArrowBom.cs
        攻撃の1つとしてあるArrowBomの挙動を計算します
        
        処理
        ・上部で少し待機
        ・回転しながら落ちる
        ・爆発してArrowを発射
 */

using UnityEngine;
/// <summary>
/// 攻撃の1つとしてあるArrowBomの挙動を計算します
/// </summary>
public class ArrowBom : MonoBehaviour
{
    /// <summary>
    /// 重力
    /// </summary>
    [SerializeField] float gravity;

    [SerializeField] GameObject Arrow;

    [SerializeField] float wait_time;
    [SerializeField] float stoptime_lastwait;
    [SerializeField] float explosion_time;
    Vector2 velocity = Vector2.zero;

    Vector2 startpos;
    float elapsed_time = 0;
    float rotate_speed = 0.5f;

    void Start()
    {
        gravity *= -1;
        startpos = transform.position;

        // 回転の挙動を自然なランダムに
        rotate_speed += Random.Range(-0.1f, 0.1f);
        rotate_speed *= Random.Range(0, 2) == 0 ? 1 : -1;
    }
    void Update()
    {
        // このオブジェクトの経過時間
        elapsed_time += Time.deltaTime;

        if (wait_time < elapsed_time) // 待機後落下
        {
            AfterWaitUpdate();
        }
        else // 待機中
        {
            float movetime = wait_time - stoptime_lastwait;
            transform.position = startpos + (3f * Mathf.Min(movetime, elapsed_time) / movetime * Vector2.down * 0.25f);
            //初期位置から少しだけ進んだ位置までゆっくり移動→少し停止した後に→Arrowを発射
        }

    }

    // 落下が始まってからのUpdate処理
    void AfterWaitUpdate()
    {
        // 重力で落下
        velocity += Vector2.up * gravity;
        transform.position += (Vector3)velocity;

        // 回転しながら落下する挙動
        transform.Rotate(0, 0, rotate_speed);

        // 時間が経つと爆発
        if (explosion_time < elapsed_time)
        {
            Explosion();
            Destroy(gameObject);
        }
    }
    void Explosion()
    {
        int arrowCount = 8;
        for (int i = 0; i < arrowCount; i++)
        {
            // arrowをそれぞれの向きと位置で生成
            float angle = (2 * Mathf.PI / arrowCount * i) + transform.eulerAngles.z;
            Vector2 direction = new(Mathf.Sin(angle), Mathf.Cos(angle));

            ArrowController arrow =
                Instantiate(Arrow, transform.position + ((Vector3)direction * 1.0f), Quaternion.Euler(0, 0, 180 - (angle * Mathf.Rad2Deg)))
                .GetComponent<ArrowController>();
            arrow.wait_time = 0;
            arrow.velocity = direction * 0.25f;
        }
    }
}
