/*
    BeamController.cs
        生成されたビームの挙動を処理します
        
        処理
        ・打たれる場所にエフェクトで予告
        ・発射
        ・減衰
        ・ビーム本体の描画
        ・Colliderを変形
 */

using UnityEngine;

public class BeamController : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] ParticleSystem particle;
    [SerializeField] BoxCollider2D mycollider;

    float stageWidth;

    [HideInInspector] public float beam_hight;
    [HideInInspector] public float graceTime;
    float localTime; //このオブジェクトが生成されてからの時間経過


    void Start()
    {
        stageWidth = GameObject.Find("background").GetComponent<Background>().Width;

        localTime = 0;
        mycollider.enabled = false;
    }
    void Update()
    {
        localTime += Time.deltaTime;
        // localtimeの値によって状態0→状態1→...と分ける

        float time_notice = graceTime;
        float time_shoot = graceTime + 0f; // 今はUpdate_Shoot()を1フレームも行わない
        float time_after = graceTime + 1.20f;

        if (localTime < time_notice) //撃たれる前のエフェクト
        {
            Update_Notice(localTime);
        }
        else if (localTime < time_shoot) //撃たれた瞬間
        {
            Update_Shoot(localTime - time_notice);
        }
        else if (localTime < time_after) //撃たれた後のエフェクト
        {
            Update_AfterEffect(localTime - time_shoot);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /*
        状態0
            ビームが打たれるまでの猶予時間にエフェクトが発生するフェーズ
            localTime : 状態1が始まってから経った時間
     */
    void Update_Notice(float localTime)
    {
        // パーティクルを動かす
        if (!particle.isPlaying) particle.Play();
        particle.gameObject.transform.position = new Vector2(0, beam_hight);

        // ビームのエフェクト

        float lineWidth = 1.2f;
        float linealpha = (Mathf.Pow(4.5f, localTime * 0.4f) - 1) * 0.3f;
        RenderLine(beam_hight, lineWidth, linealpha, false);
    }

    /*
        状態1
            猶予時間が終わってビームが打たれるフェーズ
            localTime : 状態1が始まってから経った時間
     */
    void Update_Shoot(float localTime)
    {
        // パーティクルの停止
        if (particle.isPlaying) particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        // Playerとの衝突をtrue
        mycollider.enabled = true;

        float lineWidth = 3f + localTime;
        RenderLine(beam_hight, lineWidth, 1f, true);
    }

    /*
        状態2
            ビームが打たれ終わって残りのエフェクトが出るフェーズ
     */
    void Update_AfterEffect(float localTime)
    {
        // パーティクルの停止
        if (particle.isPlaying) particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        // 減衰フェーズでは最初の一部だけPlayerと衝突判定を持つ
        float collisionLifeTime = 0.3f;
        mycollider.enabled = localTime < collisionLifeTime;

        // ビームを描画

        float contraction_speed = 1.7f;
        float lineWidth = 2.8f * Mathf.Max(0, Mathf.Cos(localTime * contraction_speed - 0.05f));
        float linealpha = Mathf.Min(lineWidth, 0.8f);
        RenderLine(beam_hight, lineWidth, linealpha, false);
    }

    /*
        lineRenderer(ビーム本体)の描画を行います
        height : ビームが打たれる高さ
        linewidth : 太さ
        alpha : 透明度
        bool isWhite : ビームの色を白にするか赤にするか
     */
    void RenderLine(float hight, float linewidth, float alpha, bool isWhite)
    {
        lineRenderer.startWidth = linewidth;
        lineRenderer.endWidth = linewidth;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, new Vector2(-stageWidth / 2, hight));
        lineRenderer.SetPosition(1, new Vector2(stageWidth / 2, hight));
        lineRenderer.GetComponent<Renderer>().material.color = new Color(1, isWhite? 1 : 0, isWhite? 1 : 0, alpha);

        // LineRendererのついでにコライダーを計算
        mycollider.size = new Vector2(stageWidth, linewidth);
        mycollider.offset = new Vector2(0, hight);
    }
}
