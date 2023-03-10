using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonPressed : MonoBehaviour
{
    //ボタンが押されたら、ゲームシーンへ移行する
    public void OnClickStartButton()
    {   
        SceneManager.LoadScene("GameScene");
    }

    //ボタンが押されたら、タイトルシーンへ移行する    
    public void OnClickBackButton()
    {
        SceneManager.LoadScene("TitleScene");
    }

}
