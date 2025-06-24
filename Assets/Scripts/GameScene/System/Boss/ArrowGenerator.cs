/*
    ArrowGenerator.cs
        矢の動的生成を行います

        処理
            ・攻撃のパターンごとの生成関数
 */
using UnityEngine;

public class ArrowGenerator : MonoBehaviour
{
    [Header("他スクリプトからの情報取得用")]
    [SerializeField] 
    protected Background background;
    [Space(20)]
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject arrowBom;
    [SerializeField] GameObject player;
    [SerializeField] float generateHeight;

    [HideInInspector] public float stageWidth;

    void Start()
    {
        stageWidth = background.Width;
    }

    // x座標の指定のみでArrowを生成する関数
    GameObject GenerateArrow(float generate_x, Quaternion angle)
    {
        Vector2 generatePos = new(generate_x, generateHeight);
        return Instantiate(arrow, generatePos, angle);
    }
    GameObject GenerateArrowBom(float generate_x)
    {
        Vector2 generatePos = new(generate_x, generateHeight);
        return Instantiate(arrowBom, generatePos, Quaternion.identity);
    }



    /*
        Arrowの生成パターン1つ目
        いくつかのArrowを横に並べて生成します
            generate_x : いくつかのArrowの中心
            quantity : 並べる個数
            arrowGap : Arrowごとの間隔
     */
    public void GeneratePattern01(float generate_x, int quantity, float arrowGap)
    {
        for (int i = 0; i < quantity; i++)
        {
            // 生成するx座標を求めて生成
            float pos_x = generate_x + arrowGap * (i - (quantity / 2f));
            GenerateArrow(pos_x, Quaternion.identity);
        }
    }
    public void GeneratePattern02(float generate_x, int quantity, float angle_range)
    {
        for (int i = 0; i < quantity; i++)
        {
            // Arrowの向きを計算

            Vector3 myPos = new(generate_x, generateHeight);
            var vec_MyToTarget = player.transform.position - myPos;
            /*var look = Quaternion.Euler(0, 0, 0 + Mathf.Rad2Deg * Random.Range(-angle_range / 2, angle_range / 2))
                     * Quaternion.Euler(-90, 0, 0)
                * Quaternion.LookRotation(vec_MyToTarget, Vector3.up);
            */
            var look_euler_z = (Mathf.Atan2(-vec_MyToTarget.x, -vec_MyToTarget.y) + Random.Range(-angle_range / 2, angle_range / 2)) * Mathf.Rad2Deg;
            var look = Quaternion.Euler(0, 0, look_euler_z);

            // arrow生成
            GameObject arrow = GenerateArrow(generate_x, look);

            // arrowのvelocityを計算

            float arrow_speed = 0.25f;
            Vector2 arrow_velocity = look * Vector3.down * arrow_speed;
            arrow.GetComponent<Arrow>().velocity = arrow_velocity;
        }
    }
    public void GeneratePattern03(float generate_x)
    {
        GenerateArrowBom(generate_x);
    }
    public void GeneratePattern04(float generate_y)
    {
        // 右向きのArrow

        Vector3 myPos = new(stageWidth / 2, generate_y);

        var radRight = Mathf.PI / 2;
        var look = Quaternion.Euler(0, 0, radRight);

        // arrow生成
        GameObject arrowInstance = Instantiate(arrow, myPos, look);

        // arrowのvelocityを計算

        float arrow_speed = 0.25f;
        Vector2 arrow_velocity = look * Vector3.down * arrow_speed;
        arrowInstance.GetComponent<Arrow>().velocity = arrow_velocity;
    }
}
