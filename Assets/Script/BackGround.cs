using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public Transform m_player;      //プレイヤー
    public Vector2 m_limit;         //背景の移動範囲

    void Start()
    {
        
    }

    private void Update()
    {
        //プレイヤーの現在地取得
        var pos = m_player.localPosition;

        //地面の端の位置を取得
        var limit = Utils.m_moveLimit;

        //プレイヤーが画面のどの位置に存在するのか(0から1の範囲の値)
        var tx = 1 - Mathf.InverseLerp(-limit.x, limit.x, pos.x);
        var ty = 1 - Mathf.InverseLerp(-limit.y, limit.y, pos.y);

        //プレイヤーの現在位置から背景の表示位置を算出する
        var x = Mathf.Lerp(-m_limit.x, m_limit.x, tx);
        var y = Mathf.Lerp(-m_limit.y, m_limit.y, ty);

        //背景の表示位置を更新する
        transform.localPosition = new Vector3(x, y, 0);
        
    }
}
