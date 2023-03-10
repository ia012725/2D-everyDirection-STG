using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFixed : MonoBehaviour
{
    //public float m_spped;

    //弾発射用の変数宣言
    public ShotFixed m_shotPrefab;          //弾のプレハブ
    
    //発射位置(オブジェクト)の位置を取得
    public GameObject FirePoint;

    public float m_shotSpeed;          //弾の移動の速さ

    public float m_ShotAngleRange;     //複製の弾を発射する時の角度
    public float m_shotTimer;          //弾の発射タイミングを管理
    public int m_shotCount;            //弾の発射数
    
    public float m_shotInterval;       //弾の発射間隔(秒)
    public int m_hpMax;     //HPの最大値
    public int m_hp;        //HP

    public int m_level;

    public Explosion m_explosionPrefab;     //爆破エフェクトのプレハブ

    //プレイヤーのインスタンスを管理
    public static PlayerFixed m_instance_p;

    /*SE用関数*/
    public AudioClip m_damageClip;      //ダメージを受けたときに再生するSE

    private void Awake()
    {
        var player = Player.m_instance;

        m_hp = player.m_hp;         //HP
        m_hpMax = player.m_hpMax;   //HP最大値
        m_level = player.m_level;

        //他クラスからプレイヤーを参照できるようにする
        m_instance_p = this;

        //弾発射コルーチン
        StartCoroutine("Shot");
    }

    void Update()
    {
        //ゲームを 60 FPS 固定にする
        Application.targetFrameRate = 60;
        
        Move();
        Shot();

    }

    void Move()
    {   
        //矢印キーの入力情報を取得する
        var x = Input.GetAxis( "Horizontal" );
        var y = Input.GetAxis( "Vertical" );

        //現在位置に、x, yの値をプラスする
        Vector3 nextPosition = transform.position + new Vector3(x, y, 0) * Time.deltaTime * 4;
        
        //現在地を反映させる
        transform.position = nextPosition;
        
        //プレイヤーが画面外に出ないよう位置を制限
        transform.localPosition = UtilsFixed.ClampPosition(transform.localPosition);    

    }

    IEnumerator Shot()
    {
        while(true)
        {
            //弾の生成
            Instantiate(m_shotPrefab, FirePoint.transform.position, transform.localRotation);

            yield return new WaitForSeconds(m_shotInterval);

        }
    }

    //ダメージを受ける関数
    public void Damage(int damege)
    {
        // ダメージを受けた時の SE を再生する
        var audioSource = FindObjectOfType<AudioSource>();
        audioSource.PlayOneShot( m_damageClip );

        //HPを減らす
        m_hp -= damege;

        //HPがまだある場合、ここで処理を終える
        if(0 < m_hp)
        {
            return;
        }

        //プレイヤーが死亡したら非表示
        //ここでgameover演出
        gameObject.SetActive(false);
    }


        //他のオブジェクトととの衝突判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //弾1と衝突した場合
        if(collision.name.Contains("BossBullet1"))
        {
            //爆発エフェクトを生成
            Instantiate(
                m_explosionPrefab,
                collision.transform.localPosition,
                Quaternion.identity
            );

            //弾を削除する
            Destroy(collision.gameObject);

            //HPを減らす
            Damage(EnemyFixed.m_instance_e.m_damage);

            return;
        }
        
        //弾2と衝突した場合
        if(collision.name.Contains("BossBullet2"))
        {
            //爆発エフェクトを生成
            Instantiate(
                m_explosionPrefab,
                collision.transform.localPosition,
                Quaternion.identity
            );
            
            //弾を削除する
            Destroy(collision.gameObject);

            //HPを減らす
            Damage(EnemyFixed.m_instance_e.m_damage);

            return;
        }
    }

}
