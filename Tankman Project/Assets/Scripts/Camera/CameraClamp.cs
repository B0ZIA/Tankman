using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClamp : MonoBehaviour
{
    public Transform target;
    const float MAP_POS_MIN_X = -14f;
    const float MAP_POS_MIN_Y = -18f;
    const float MAP_POS_MAX_X = 14.15f;
    const float MAP_POS_MAX_Y = 18;


    void Update ()
    {
        float x = Mathf.Clamp(target.position.x, MAP_POS_MIN_X, MAP_POS_MAX_X);
        float y = Mathf.Clamp(target.position.y, MAP_POS_MIN_Y, MAP_POS_MAX_Y);
        float z = transform.position.z;

        transform.position = new Vector3(x, y, z);
    }
}
