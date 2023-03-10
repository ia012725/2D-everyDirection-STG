using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilsFixed : MonoBehaviour
{
    public static Vector2 m_moveLimit = new Vector2(4.6f, 3.4f);

    public static Vector3 ClampPosition(Vector3 position)
    {
        return new Vector3
        (
            Mathf.Clamp(position.x, -m_moveLimit.x, m_moveLimit.x),
            Mathf.Clamp(position.y, -m_moveLimit.y, m_moveLimit.y),
            0
        );
    }
}
