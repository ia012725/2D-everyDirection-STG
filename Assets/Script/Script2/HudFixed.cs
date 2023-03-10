using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HudFixed : MonoBehaviour
{
    public Image m_hpGauge;
    public Image m_expGauge;
    public Image m_bossGauge;

    /*レベルの表示を更新する*/
    public Text m_levelText;    //レベルのテキスト

    /*ゲームオーバーを表示*/
    public GameObject m_gameOverText;   //ゲームオーバーのテキスト


    public GameObject m_clearText;    //クリアテキスト


    private void Update()
    {   
        //インスタンスを取得
        var player = PlayerFixed.m_instance_p;  //プレイヤーを取得する
        var enemy = EnemyFixed.m_instance_e;    //敵を取得

        //HPゲージの表示を更新する
        var hp_p = player.m_hp;
        var hpMax_p = player.m_hpMax;
        m_hpGauge.fillAmount = (float)hp_p / hpMax_p;

        //HPゲージの表示を更新する
        var hp_e = enemy.m_hp;
        var hpMax_e = enemy.m_hpMax;
        m_bossGauge.fillAmount = (float)hp_e / hpMax_e;

        /*
        //TODO：ボス戦が終わった後もゲームが続くようにするときに実装
        //経験値ゲージの表示を更新する
        var exp = player.m_exp;
        var prevNeedExp = player.m_prevNeedExp;
        var needExp = player.m_needExp;
        m_expGauge.fillAmount = 
            (float)(exp - prevNeedExp) / (needExp - prevNeedExp);
        */

        //レベルのテキスト表示を更新する
        m_levelText.text = player.m_level.ToString();

        //プレイヤーが非表示ならゲームオーバー表示する
        m_gameOverText.SetActive(!player.gameObject.activeSelf);
        if(m_gameOverText.activeSelf == true)
        {
            //もしスペースボタンが押されたらシーンの再読み込み
            //リトライ用
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("GameScene");
            }
        }

        //敵が非表示ならゲームオーバー表示する
        m_clearText.SetActive(!enemy.gameObject.activeSelf);
 
    }    

}


