/*
    ArrowController.cs
        各Arrowの挙動を処理します
        
        処理
        ・上部で少し待機
        ・直進する
        ・使い終わったら削除
 */

using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float fall_speed;
    public float wait_time;
    public float stoptime_lastwait;
    [HideInInspector] public Vector2 velocity = Vector2.zero;

    Vector2 startpos;
    float elapsed_time = 0;

    void Start()
    {
        startpos = transform.position;
        if (velocity == Vector2.zero)
        {
            velocity = Vector2.down * fall_speed;
        }
    }
    void Update()
    {
        // このオブジェクトの経過時間
        elapsed_time += Time.deltaTime;

        if (wait_time < elapsed_time)
        {
            AfterWaitUpdate();
        }
        else
        {
            float movetime = wait_time - stoptime_lastwait;
            transform.position = startpos + (3f * Mathf.Min(movetime, elapsed_time) / movetime * velocity);
                //初期位置から少しだけ進んだ位置までゆっくり移動→少し停止した後に→Arrowを発射
        }

    }

    // 落下が始まってからのUpdate処理
    void AfterWaitUpdate()
    {
        // 更新前の座標
        Vector2 myPosition = transform.position;

        // 落下処理
        transform.position = myPosition + velocity;

        // 下にスクリーンアウトで
        float offScreen_y = -10;
        if (transform.position.y < offScreen_y)
        {
            Destroy(gameObject);
        }
        // 遠くにいったら
        float far = 20;
        if (far < Vector2.Distance(Vector2.zero, transform.position))
        {
            Destroy(gameObject);
        }
    }
}
