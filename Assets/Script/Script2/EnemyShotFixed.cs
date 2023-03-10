using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotFixed : MonoBehaviour
{
    float m_dx;
    float m_dy;

    public void Init(float angle, float speed)
    {
        m_dx = Mathf.Cos(angle) * speed;
        m_dy = Mathf.Sin(angle) * speed;
    }


    void Update()
    {
        
        //弾の移動
        transform.position += new Vector3(m_dx, m_dy, 0) * Time.deltaTime;
        


        
        //まっすぐの弾が発射する
        //transform.position += new Vector3(0, -3f, 0) * Time.deltaTime;

        if(transform.position.y < -4.7)
        {
            Destroy(gameObject);
        }
        


        /*
        //弾が画面外に出たら、削除する
        if(transform.localPosition = UtilsFixed.ClampPosition(transform.localPosition))
        {
            Destroy(gameObject);
        }
        */
    }


}