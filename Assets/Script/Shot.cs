using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    private Vector3 m_velocity;

    void Update()
    {
        //弾の移動
        transform.localPosition += m_velocity * Time.deltaTime * 5;
        
    }

    public void Init(float angle, float speed)
    {
        var direction = Utils.GetDirection(angle);

        //速度を求める(発射角度*速さ)
        m_velocity = direction * speed;

        //弾が進行方向を向くようにする
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

        //2秒後に球を削除する
        Destroy(gameObject, 2);
    }
}
