using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hud : MonoBehaviour
{
    public Image m_hpGauge;         //HPゲージ
    public Image m_expGauge;        //経験値ゲージ

    public float m_standardLevel;   //ボス画面に遷移するレベルの設定

    /*レベルの表示を更新する*/
    public Text m_levelText;    //レベルのテキスト

    /*ゲームオーバーを表示*/
    public GameObject m_gameOverText;   //ゲームオーバーのテキスト

    private void Update()
    {
        //プレイヤーを取得する
        var player = Player.m_instance;
        var fade = Fade.m_instance_f;

        //HPゲージの表示を更新する
        var hp = player.m_hp;
        var hpMax = player.m_hpMax;
        m_hpGauge.fillAmount = (float)hp / hpMax;

        //経験値ゲージの表示を更新する
        var exp = player.m_exp;
        var prevNeedExp = player.m_prevNeedExp;
        var needExp = player.m_needExp;
        m_expGauge.fillAmount = 
            (float)(exp - prevNeedExp) / (needExp - prevNeedExp);

        //レベルのテキスト表示を更新する
        m_levelText.text = player.m_level.ToString();
    
        //プレイヤーが非表示ならゲームオーバー表示する
        m_gameOverText.SetActive(!player.gameObject.activeSelf);

        if(m_gameOverText.activeSelf == true)
        {
            //もしスペースボタンが押されたらシーンの再読み込み
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("GameScene");
            }
        }
        
        if(player.m_level == m_standardLevel)
        {
            StartCoroutine(fade.FadeOut());
            SceneManager.LoadScene("FixedGameScene");
        }
        
    } 

}

