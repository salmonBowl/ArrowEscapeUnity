/*
    Confetti_prefab.cs
        ゲームクリア後に紙吹雪を出します

        処理
        ・落ちる
        ・使い終わったら削除
 */
using UnityEngine;

public class Confetti : MonoBehaviour
{
    [SerializeField] SpriteRenderer my;
    [SerializeField] Color[] colors;
    [SerializeField] float fall_speed;
    float rotate_speed = 1.3f;

    void Start()
    {
        transform.Rotate(0, 0, Random.Range(0.0f, 180.0f));

        // 回転速度をランダムにする
        rotate_speed += Random.Range(-0.7f, 0.7f);
        rotate_speed *= Random.Range(0, 2) == 0 ? 1 : -1;

        //色を決定
        my.color = colors[Random.Range(0, colors.Length)];
    }

    void Update()
    {
        transform.position += Vector3.down * fall_speed;
        transform.Rotate(0, 0, rotate_speed);

        // 下にスクリーンアウトで削除
        float offScreen_y = -10;
        if (transform.position.y < offScreen_y)
        {
            Destroy(gameObject);
        }
    }
}
