using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class VerticalRollBackGround : MonoBehaviour
{
    //スクロールスピード
    [SerializeField] float m_speed = 1;
   
   void Update()
   {
       //下方向にスクロール
        transform.position -= new Vector3(0,Time.deltaTime * m_speed);
        
        //Yが-11まで来れば、22まで移動する
        if(transform.position.y <= -11f){
            transform.position = new Vector2(0, 22f);
        }
    }
}