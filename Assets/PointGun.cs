using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointGun : MonoBehaviour
{
    public float factor = 0.7f;

    void Update()
    {
        var dist = (transform.position.z - Camera.main.transform.position.z) * factor;
        var pos = Input.mousePosition;
        pos.z = dist;
        pos = Camera.main.ScreenToWorldPoint(pos);
        transform.LookAt(pos);
    }
}
