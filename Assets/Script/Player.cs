using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float m_speeed;

    //弾発射用の変数宣言
    public Shot m_shotPrefab;          //弾のプレハブ
    public float m_shotSpeed;          //弾の移動の速さ
    public float m_ShotAngleRange;     //複製の弾を発射する時の角度
    public float m_shotTimer;          //弾の発射タイミングを管理
    public int m_shotCount;            //弾の発射数
    public float m_shotInterval;       //弾の発射間隔(秒)
    public int m_hpMax;     //HPの最大値
    public int m_hp;        //HP

    //プレイヤーのインスタンスを管理
    public static Player m_instance;


    /*宝石がプレイヤーを追尾*/
    public float m_magnetDistance;      //宝石を引き付ける距離

    /*プレイヤーの経験値をあげる*/
    public int m_nextExpBase;       //次のレベルまでに必要な経験値の基本値
    public int m_nextEpInterval;    //次のレベルまでに必要な経験値の増加量
    public int m_level;             //レベル
    public int m_exp;               //経験値
    public int m_prevNeedExp;       //前のレベルに必要だった経験値
    public int m_needExp;           //次のレベルに必要な経験値
    
    /*SE用関数*/
    public AudioClip m_levelUpClip;     //レベルアップしたときに再生するSE
    public AudioClip m_damageClip;      //ダメージを受けたときに再生するSE

    /*
    *プレイヤーのレベルが上がったら、以下パラメータを成長させる
    *
    *弾の発射数
    *弾の発射間隔
    *宝石を引き付ける距離
    *
    */
    public int m_levelMax;                  //レベルの最大値
    public int m_shotCountFrom;             //弾の発射数(レベルが最小値のとき)
    public int m_ShotCountTo;               //弾の発射数(レベルが最大値のとき)
    public float m_shotIntervalFrom;        //弾の発射間隔(秒)(レベルが最小値のとき)
    public float m_shotIntervalTo;          //弾の発射間隔(秒)(レベルが最大値のとき)
    public float m_magnetDistanceFrom;      //宝石を引き付ける距離(レベルが最小値のとき)
    public float m_magnetDistanceto;        //宝石を引き付ける距離(レベルが最大値のとき)

    private void Awake()
    {
        //他クラスからプレイヤーを参照できるようにする
        m_instance = this;

        m_hp = m_hpMax;     //HP

        m_level = 1;                //レベル
        m_needExp = GetNeedExp(1);  //次のレベルに必要な経験値

        /*パラメータ成長用*/
        m_shotCount = m_shotCountFrom;              //弾の発射数
        m_shotInterval = m_shotIntervalFrom;        //弾の発射間隔
        m_magnetDistance = m_magnetDistanceFrom;    //宝石を引き付ける距離
    }

    void Update()
    {
        //60FPSに固定
        Application.targetFrameRate = 60;

        //キー入力値を取得
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        //プレイヤーの移動
        var velocity = new Vector3(h, v) * m_speeed;
        transform.localPosition += velocity * Time.deltaTime * 4;

        //プレイヤーが画面外に出ないように制限
        transform.localPosition = Utils.ClampPosition(transform.localPosition);

        //プレイヤーのスクリーン座標を計算
        var screenPos = Camera.main.WorldToScreenPoint(transform.position);
        
        //プレイヤーから見たマウスカーソルの方向を計算
        var direction = Input.mousePosition -screenPos;
        
        //マウスカーソルが存在する方向の角度を取得
        var angle = Utils.GetAngle(Vector3.zero, direction);
        
        //プレイヤーがマウスカーソルの方向を見るようにする
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

        //タイマーの更新
        m_shotTimer += Time.deltaTime;

        //弾の発射タイミングではない場合、ここで処理を終える
        if(m_shotTimer < m_shotInterval)
        {
            return;
        }

        //タイマーをリセットする
        m_shotTimer = 0;

        //弾を発射する
        ShootNWay(angle, m_ShotAngleRange, m_shotSpeed, m_shotCount);
    }

    //弾を発射する関数
    private void ShootNWay(
        float angleBase, float angleRange, float speed, int count
    )
    {
        var pos = transform.localPosition;  //プレイヤーの位置
        var rot = transform.localRotation;  //プレイヤーの向き

        //弾を複数発射する場合
        if(1 < count)
        {
            for(int i = 0; i< count; ++i)
            {
                //弾の発射角度を計算
                var angle =
                    angleBase + angleRange * ( (float)i / (count - 1) - 0.5f);

                //弾の生成
                var shot = Instantiate(m_shotPrefab, pos, rot);
                
                //弾を発射する方向と速さを設定
                shot.Init(angle, speed);
            }
        }
        //弾を一つだけ発射する場合
        else if(count == 1)
        {
            //弾の生成
            var shot = Instantiate(m_shotPrefab, pos, rot);
            //弾を発車する方向と速さを設定
            shot.Init(angleBase, speed);
        }
    }

    //ダメージを受ける関数
    public void Damage(int damage)
    {   
        //HPを減らす
        m_hp -= damage;

        //HPがまだある場合、ここで処理を終える
        if(0 < m_hp)
        {
            return;
        }

        //プレイヤーを非表示にする
        gameObject.SetActive(false);
    }

    //経験値を増やす関数(宝石を取得した際に呼ばれる)
    public void AddExp(int exp)
    {
        //経験値を増やす
        m_exp += exp;

        //まだレベルアップに必要な経験値が足りていない場合
        //ここで処理を終える
        if(m_exp < m_needExp)
        {
            return;
        }

        //レベルアップする
        m_level++;

        //今回のレベルアップに必要だった経験値を記憶しておく
        m_prevNeedExp = m_needExp;

        //次のレベルアップに必要な経験値を計算する
        m_needExp = GetNeedExp(m_level);

        /*
        *レベルアップしたときにボムを発動する
        */
        var angleBase = 0;
        var angleRange = 360;
        var count = 28;

        ShootNWay(angleBase, angleRange, 1f, count);
        ShootNWay(angleBase, angleRange, 2f, count);
        ShootNWay(angleBase, angleRange, 3f, count);

        //レベルアップしたときのSEを再生する
        var audioSource = FindObjectOfType<AudioSource>();
        audioSource.PlayOneShot(m_levelUpClip);

        /*パラメータ成長用*/
        //レベルアップしたので、各種パラメータを更新する
        var t = (float)(m_level - 1) / (m_levelMax - 1);
        
        //弾の発射数
        m_shotCount = Mathf.RoundToInt(
            Mathf.Lerp(m_shotCountFrom, m_ShotCountTo, t)
        );
        
        //弾の発射間隔
        m_shotInterval = Mathf.Lerp(
            m_shotIntervalFrom, m_shotIntervalTo, t
        );
        
        //宝石を引き付ける距離
        m_magnetDistance = Mathf.Lerp(
            m_magnetDistanceFrom, m_magnetDistanceto, t
        );
    }

    //指定されたレベルに必要な経験値を計算する関数
    private int GetNeedExp(int level)
    {
        /*
        *レベルが上がるほど必要な経験値を増やす
        *(ex)
        *m_nextExpBase = 5, m_nextExpInterval = 10の時
        *レベル1: 5 + 10 * 0 = 5;
        *レベル2: 5 + 10 * 1 = 15;
        *レベル3: 5 + 10 * 4 = 45;
        *となる
        */
        return m_nextExpBase +
            m_nextEpInterval * ((level - 1) * (level - 1));
    }
}
