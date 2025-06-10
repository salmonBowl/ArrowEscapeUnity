/*
    StageClear.cs
        ステージクリア後に紙吹雪を降らせます

        処理
            ・紙吹雪の量の計算
            ・降らせる
 */

using UnityEngine;

public class StageClear : MonoBehaviour
{
    [SerializeField] Stage stage;
    [SerializeField] GameObject Confetti;
    [SerializeField] GameObject Arrow;
    [SerializeField] PlayerHitpoint playerhp;
    [SerializeField] float waitConfetti; // ステージクリアの余韻のために少し時間を空けて降らせたい
    [SerializeField] float confetti_maxInterval;
    [SerializeField] float confetti_timeToMax;
    [SerializeField] float generateHight;

    float time = 0;
    float frameCount = 0;

    void Start()
    {
        time = -waitConfetti;
    }
    void Update()
    {
        // クリア後
        if (stage.IsClear)
        {
            time += Time.deltaTime;

            // 紙吹雪を降らせる

            // 1つの生成に何フレーム使うか、最初は量が少ない
            float genInterval = confetti_maxInterval * Mathf.Max(confetti_timeToMax / Mathf.Max(0.001f, time), 1);
            frameCount++;

            if (genInterval <= frameCount)
            {
                if (!stage.IsPerfectClear) // ダメージ0でクリアしたか判定
                {
                    if (playerhp.HP == 1)
                    {
                        stage.IsPerfectClear = true;
                    }
                }

                if (stage.IsPerfectClear) // ダメージ0でクリアしたとき
                {
                    Confetti = Arrow; // 紙吹雪の代わりに矢が降る
                }

                // 紙吹雪生成
                float half_genRange = stage.Width / 2;
                GameObject confetti = Instantiate(Confetti, new Vector3(Random.Range(-half_genRange, half_genRange), generateHight, 0), Quaternion.identity);
                
                if (stage.IsPerfectClear)
                {
                    confetti.GetComponent<ArrowController>().wait_time = 0;
                }
                
                frameCount -= genInterval;
            }
        }
        else
        {
            time = -waitConfetti;
        }
    }
}
