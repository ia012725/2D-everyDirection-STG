using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{   
    //インスタンス管理
    public static Fade m_instance_f;

    void Awake()
    {
        //他クラスからの参照用
        m_instance_f = this;
    }

    //画面をフェードインさせるコールチン
    public IEnumerator FadeIn()
    {
        
        //色を変えるゲームオブジェクトからImageコンポーネントを取得
        Image fade = GetComponent<Image>();

        //フェード元の色を設定(黒)
        fade.color = new Color((0.0f / 255.0f), (0.0f / 255.0f), (0.0f / 0.0f), (255.0f / 255.0f));

        //フェードインにかかる時間(秒)
        const float fade_time = 2f;

        //ループ回数(0はエラー)
        const int loop_count = 10;

        //ウェイト時間算出
        float wait_time = fade_time / loop_count;

        //色の間隔を算出
        float alpha_interval = 255.0f / loop_count;

        //色を徐々に変えるループ
        for (float alpha = 255.0f; alpha >= 0.0f; alpha -= alpha_interval)
        {
            //待ち時間
            yield return new WaitForSeconds(wait_time);

            //Alpha値を少しずつ下げる
            Color new_color = fade.color;
            new_color.a = alpha / 255.0f;
            fade.color = new_color;
        }

        //コルーチンを終了させる
        yield break;
    }

    //画面をフェードインさせるコールチン
    public IEnumerator FadeOut()
    {
        //色を変えるゲームオブジェクトからImageコンポーネントを取得
        Image fade = GetComponent<Image>();

        //フェード後の色を設定(黒)
        fade.color = new Color((0.0f / 255.0f), (0.0f / 255.0f), (0.0f / 0.0f), (0.0f / 255.0f));

        //フェードインにかかる時間(秒)
        const float fade_time = 2.0f;

        //ループ回数(0はエラー)
        const int loop_count = 10;

        //ウェイト時間算出
        float wait_time = fade_time / loop_count;

        //色の間隔を算出
        float alpha_interval = 255.0f / loop_count;

        //色を徐々に変えるループ
        for (float alpha = 0.0f; alpha <= 255.0f; alpha += alpha_interval)
        {
            //待ち時間
            yield return new WaitForSeconds(wait_time);

            //Alpha値を少しずつ上げる
            Color new_color = fade.color;
            new_color.a = alpha / 255.0f;
            fade.color = new_color;
        }

        //コルーチンを終了させる
        yield break;
    }
}
