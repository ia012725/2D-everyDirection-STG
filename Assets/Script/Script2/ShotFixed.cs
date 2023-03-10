using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotFixed : MonoBehaviour
{
    private void Update()
    {
        //弾の移動
        transform.localPosition += new Vector3(0, 7f, 0) * Time.deltaTime;

        if(transform.position.y > 4.7)
        {
            Destroy(gameObject);
        }
        
    }
}
