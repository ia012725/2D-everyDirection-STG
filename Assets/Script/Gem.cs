using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public int m_exp;               //取得できる経験値
    public float m_brake = 0.9f;    //散らばるときの減速量
                                    //(数値が小さいほどすぐに減速する)

    private Vector3 m_direction;    //散らばるときの進行方向
    private float m_speed;          //散らばるときの速さ

    /*プレイヤーを追尾*/
    public float m_followAccel = 0.01f; //プレイヤーを追尾するときの加速度
                                        //数値が大きいほどすぐ加速する
    private bool m_isFollow;            //プレイヤーを追尾する場合true
    private float m_followSpeed;        //プレイヤーを追尾する速さ

    /*SE用*/
    public AudioClip m_goldClip;    //宝石を取得した時に再生するSE
    
    //gemのインスタンスを管理
    public static Gem m_instance_gem;

    private void Awake()
    {
        m_instance_gem = this;
    }
    
    private void Update()
    {   
        /*プレイヤーを追尾する場合*/

        //プレイヤーの現在位置を取得する
        var playerPos = Player.m_instance.transform.localPosition;
        
        //プレイヤーと宝石の距離を計算する
        var distance = Vector3.Distance(playerPos, transform.localPosition);
        
        //プレイヤーと宝石の距離が近づいた場合
        if(distance < Player.m_instance.m_magnetDistance)
        {
            //プレイヤーを追尾するモードに入る
            m_isFollow = true;
        }

        //プレイヤーを追尾するモードかつ
        //プレイヤーがまだ死んでいない場合
        if(m_isFollow && Player.m_instance.gameObject.activeSelf)
        {
            //プレイヤーの現在位置へ向かうベクトルを生成する
            var direction = playerPos - transform.localPosition;
            direction.Normalize();

            //宝石をプレイヤーが存在する方向に移動する
            transform.localPosition += direction * m_followSpeed;

            //加速しながら近づく
            m_followSpeed += m_followAccel  * Time.deltaTime * 4;
            return;
        }

        //散らばる速度を計算
        var velocity = m_direction * m_speed;

        //散らばる
        transform.localPosition += velocity;

        //だんだん減速する
        m_speed *= m_brake;

        //宝石が画面買いに出ないように位置を制限
        transform.localPosition =
            Utils.ClampPosition(transform.localPosition);

    }

    //宝石が出現するときに初期化する関数
    public void Init(int score, float speedMin, float speedMax)
    {
        //宝石がどの方向に散らばるかランダムに決定
        var angle = Random.Range(0, 360);

        //進行方向をラジアン値に変換する
        var f = angle * Mathf.Deg2Rad;

        //進行方向のベクトルを生成する
        m_direction = new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0);

        //宝石の散らばる速度をランダムに決定する
        m_speed = Mathf.Lerp(speedMin, speedMax, Random.value);

        //8秒後に宝石を削除する
        Destroy(gameObject, 8);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //衝突したオブジェクトがプレイヤーでない場合は無視する
        if(!collision.name.Contains("Player")){
            return;
        }

        Destroy(gameObject);

        //プレイヤーの経験値を増やす
        var player = collision.GetComponent<Player>();
        player.AddExp(m_exp);

        //宝石を取得したときのSEを再生する
        var audioSource = FindObjectOfType<AudioSource>();
        audioSource.PlayOneShot(m_goldClip);
    }
}
