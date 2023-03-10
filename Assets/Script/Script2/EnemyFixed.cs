using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFixed : MonoBehaviour
{
    public float m_speed;           //移動する速さ
    public int m_hp;                //HP 
    public int m_hpMax;             //HP最大値
    public int m_exp;               //この敵から得られる経験値
    public int m_damage;            //この敵がプレイヤーに与えるダメージ
    private Vector3 m_direction;    //進行方向

    /*SE再生用*/
    public AudioClip m_deathClip;   //敵を倒した時に再生するSE
    public AudioClip m_hit;         //弾が当たった時に再生するSE

    public Explosion m_explosionPrefab;     //爆破エフェクトのプレハブ

    //TODO:敵が出現する際のエフェクトを追加する
    //public GameObject appearEffect;
    
    public EnemyShotFixed m_enemyShotPrefab1;   //BossShot1の弾
    public EnemyShotFixed m_enemyShotPrefab2;   //BossShot2の弾

    //敵のインスタンスを管理
    public static EnemyFixed m_instance_e;

    //追加
    public int m_apperFlag;
    
    //敵の行動パターン
    public enum ActionPattern
    {
        Appear,     //出現
        Wait,       //待機
    }
    
    //現在の行動パターン
    public ActionPattern m_currentAction;

    private void Start()
    {
        //HPを初期化
        m_hp = m_hpMax;

        //他クラスからプレイヤーを参照できるようにする
        m_instance_e = this;

        //ボス戦に入る前に暗転(フェードイン)する
        var fade = Fade.m_instance_f;
        StartCoroutine(fade.FadeIn());

        //攻撃開始コルーチン
        StartCoroutine(ShotProcess());

    }

    private void Update()
    {
        
        //ボスの行動パターンを切り替える
        switch(m_currentAction)
        {
            case ActionPattern.Appear:
            UpdateApperAction();
            break;

            case ActionPattern.Wait:
            break;
        }
    }
    
    private void UpdateApperAction()
    {
        Move(Vector2.down);  

        //特定の位置まで移動したら待機パターンに切り替える
        if(transform.position.y < 2f)
        { 
            m_apperFlag = 1;
            m_currentAction = ActionPattern.Wait;
        }
    }

    private void Move(Vector3 moveDirection)
    {
        //敵の座標を取得
        var pos = transform.position;

        //移動速度と方向から移動値を現在地に加える
        pos += moveDirection * m_speed * Time.deltaTime;
        
        //敵の位置を更新
        transform.position = pos;

    }
    
    //WaveNShotM・WaveNShotMCurve用の弾プレハブ(全方位攻撃)
    private void Shot1(float angle, float speed)
    {
        var pos = transform.position;
        var rot = transform.rotation;

        EnemyShotFixed shot = Instantiate(m_enemyShotPrefab1, pos, rot);
        shot.Init(angle, speed);
    }

    //WaveNPlayerAimShot用の弾プレハブ(ホーミング攻撃)
    void Shot2(float angle, float speed)
    {
        var pos = transform.position;
        var rot = transform.rotation;

        EnemyShotFixed shot = Instantiate(m_enemyShotPrefab2, pos, rot);
        shot.Init(angle, speed);
    }

    //出現中の移動
    IEnumerator MoveProcess()
    {        
        while(m_apperFlag == 1)
        {
            UpdateApperAction();
            yield break;      //1フレーム待つ
        }
    }

    IEnumerator ShotProcess()
    {
        while(true)
        {
            //行動パターン:waitになったら、撃ち始める
            yield return StartCoroutine(MoveProcess());
            yield return new  WaitForSeconds(2.7f); 

            yield return WaveNShotM(4, 8);
            yield return new WaitForSeconds(0.7f);
            yield return WaveNShotMCurve(4, 16);
            yield return new WaitForSeconds(0.7f);
            yield return WaveNPlayerAimShot(4, 6);
            yield return new WaitForSeconds(0.7f);

        }
    }

     IEnumerator WaveNShotM(int n, int m)
    {
        //4回8方向に撃つ
        for (int w = 0; w < n; w++)
        {
            yield return new WaitForSeconds(0.3f);
            ShotN(m, 2);
        }
    }

    IEnumerator WaveNShotMCurve(int n, int m)
    {
        //4回8方向に撃つ
        for (int w = 0; w < n; w++)
        {
            yield return new WaitForSeconds(0.3f);
            yield return ShotNCurve(m, 2);
        }
    }

    
    IEnumerator WaveNPlayerAimShot(int n, int m)
    {
        //4回8方向に撃つ
        for (int w = 0; w < n; w++)
        {
            yield return new WaitForSeconds(1f);
            PlayerAimShot(m, 3);
        }
    }

    void ShotN(int count, float speed)
    {
        int bulletCount = count;
        for(int i = 0; i < bulletCount; i++)
        {
            float angle = i * (2 * Mathf.PI / bulletCount); //2PI = 360°
            Shot1(angle, speed);
        }
    }

    IEnumerator ShotNCurve(int count, int speed)
    {
        int bulletCount = count;
        for(int i = 0; i < bulletCount; i++)
        {
            float angle = i * (2 * Mathf.PI / bulletCount);
            Shot1(angle - Mathf.PI / 2f, speed);
            Shot1(-angle - Mathf.PI / 2f, speed);
            yield return new WaitForSeconds(0.1f);
        }
    }

    
    //Playerを狙うようにする
    //Playerの位置を把握・どの角度に撃てばよいかを計算
    void PlayerAimShot(int count, float speed)
    {
        //先にPlayerが倒されていたら、何もしない
        if(PlayerFixed.m_instance_p != null)
        {
            //自分から見たPlayerの位置を計算する
            Vector3 diffPosition = PlayerFixed.m_instance_p.transform.position - transform.position;
            
            //自分から見たPlayerの角度を出す
            //傾きから角度→アークタンジェント利用
            float angleP = Mathf.Atan2(diffPosition.y, diffPosition.x);

            int bulletCount = count;
            for(int i = 0; i < bulletCount; i++)
            {
                float angle
                    = (i - bulletCount / 2f) * ((Mathf.PI / 2f) / bulletCount);
                    Shot2(angleP + angle, speed);            //ここを変更
            }
        }
    }


    //他のオブジェクトととの衝突判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //敵出現中は当たり判定をなくす
        if(m_currentAction == ActionPattern.Appear)
        {
            return;
        }
        
        //プレイヤーと衝突した場合
        if(collision.name.Contains("PlayerFixed"))
        {
            //プレイヤーにダメージを与える
            var playerfixed = collision.GetComponent<PlayerFixed>();
            playerfixed.Damage(m_damage);
            return;
        }

        //弾と衝突した場合
        if(collision.name.Contains("ShotFixed"))
        {
              
            //爆発エフェクトを生成
            Instantiate(
                m_explosionPrefab,
                collision.transform.localPosition,
                Quaternion.identity
            );

            //弾が当たった時のSEを再生する
            var audioSource = FindObjectOfType<AudioSource>();
            audioSource.PlayOneShot(m_hit);

            //弾を削除する
            Destroy(collision.gameObject);

            //敵のHPを減らす
            m_hp--;

            //敵のHPがまだ残っている場合はここで処理を終える
            if(0 < m_hp)
            {
                return;
            }

            //敵を倒した時のSEを再生する
            //audioSource = FindObjectOfType<AudioSource>();
            audioSource.PlayOneShot(m_deathClip);

            //敵を削除する
            gameObject.SetActive(false);

            //TODO:エフェクトの種類を変える
            //敵が死ぬときにエフェクトを付ける
        }
    }
}