/*
    Barrage_Process.cs
        ArrowGeneratorで定義した関数を使って攻撃に関するゲームループの処理を行います

        処理
            ・攻撃を生成
            ・
 */

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Barrage_Process : ArrowGenerator
{
    //[SerializeField] StageInfo stageinfo_;
    [SerializeField] IBossHealthStatus bosshp;
    [SerializeField] BeamGenerator beam;

    // 時間経過で減っていく変数。複数個必要になりそうなのでリストに
    List<float> waitTime;

    void Start()
    {
        Application.targetFrameRate = 60;

        // Listの初期化
        waitTime = Enumerable.Repeat(0.0f, 3).ToList();

        stageWidth = stage.Width;
    }
    void OnEnable() => UpdateManager.Instance().OnUpdateWhileGame += UpdateWhileGame;

    void UpdateWhileGame()
    {
        // waitTime[]をカウントダウン
        waitTime = waitTime.Select(x => Mathf.Max(0, x - Time.deltaTime))
                           .ToList();

        if (bosshp.HealthPoint < 1)
        {
            // 並んだArrowが降ってくる攻撃
            Parallel5Arrow(true);
            // Playerに向けたArrowの攻撃
            Emission5Arrow();
        }
        if (bosshp.HealthPoint < 0.75f)
        {
            // 並んだArrowが降ってくる攻撃2つ目 (全体の密度を上げるため)
            Parallel5Arrow(false);
            // ビームが打たれる攻撃
            Beam();
        }
        if (bosshp.HealthPoint < 0.4f)
        {
            // Arrow爆弾が投下される
            ArrowBom();
        }
        if (bosshp.HealthPoint < 0.2f)
        {
            // 普通のArrow
            SingleArrow();
        }
    }

    /*
        ランダムに関する関数
        1/numの確率でtrueを返す
     */
    bool Fixed_Probability(int num)
    {
        int rand_value = Random.Range(0, num);
        if (rand_value == 1)
        {
            return true;
        }
        return false;
    }

    /*
        1/80の確率でパターン1の生成、また1.5秒のクールタイムがある
     */
    void Parallel5Arrow(bool center)
    {
        // 1/80の確率でパターン1の生成
        if (waitTime[0] == 0)
        {
            if (Fixed_Probability(80))
            {
                // 5個のArrowを0.9の間隔で
                int quantity = 5;
                float arrowGap = 0.9f;

                // 生成する範囲の調整
                float halfGenRange = ((center ? 0 : stageWidth) - (arrowGap * quantity)) / 2;

                float genPosX = Random.Range(-halfGenRange, halfGenRange);
                GeneratePattern01(genPosX, quantity, arrowGap);

                waitTime[0] = 1.5f;
            }
        }
    }
    void Beam()
    {
        // 1/80の確率でパターン2の生成
        if (waitTime[1] == 0)
        {
            if (Fixed_Probability(80))
            {
                // ビームの生成
                List<float> beamhight_candidate = new() { -6f, -6f, -3f, 0 };
                float beamhight1 = beamhight_candidate[Random.Range(0, beamhight_candidate.Count)];
                beam.GenerateBeam(beamhight1);

                // 運が悪いともう一本打たれる仕組み
                if (Fixed_Probability(13))
                {
                    // リストからbeam1の高さを削除
                    foreach (float h in beamhight_candidate.ToArray())
                    {
                        if (h == beamhight1)
                        {
                            beamhight_candidate.Remove(h);
                        }
                    }

                    // ビーム2の生成
                    float beamhight2 = beamhight_candidate[Random.Range(0, beamhight_candidate.Count)];
                    beam.GenerateBeam(beamhight2);
                }

                waitTime[1] = 5f;
            }
        }
    }
    void Emission5Arrow()
    {
        // 1/80の確率でパターン2の生成
        if (waitTime[0] == 0)
        {
            if (Fixed_Probability(80))
            {
                // Player方向に5個のArrow
                int quantity = 5;
                float anglerange = 1.5f; // 0～2π
                    // プレイヤーの向きからランダムな角度に回す時に(-range/2, range/2)で計算する

                // 生成する範囲の調整
                float half_genRange = stageWidth / 2;
                GeneratePattern02(Random.Range(-half_genRange, half_genRange), quantity, anglerange);

                waitTime[0] = 1.5f;
                waitTime[1] = 1.5f;
            }
        }
    }
    void ArrowBom()
    {
        // 1/140の確率でパターン2の生成
        if (waitTime[2] == 0)
        {
            if (Fixed_Probability(140))
            {
                // 生成する範囲の調整
                float half_genRange = stageWidth / 2 * 0.7f; // ArrowBomは端で生成されないように
                GeneratePattern03(Random.Range(-half_genRange, half_genRange));

                waitTime[2] = 5f;
            }
        }
    }
    void SingleArrow()
    {
        // 1/140の確率でパターン2の生成
        if (Fixed_Probability(50))
        {
            // 生成する範囲の調整
            float half_genRange = stageWidth / 2;
            GeneratePattern01(Random.Range(-half_genRange, half_genRange), 1, 0);
        }
    }
}
