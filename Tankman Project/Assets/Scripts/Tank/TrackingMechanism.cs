using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingMechanism : MonoBehaviour
{
    [SerializeField]
    protected float rotatingSpeed;
    


    protected void RotatingToTarget(Vector2 targetPos, float hullTurnValue = 0f)
    {

        Vector2 point2Target = (Vector2)transform.position - targetPos;
        point2Target.Normalize();
        float value = Vector3.Cross(point2Target, transform.right).z;
        if (value > 0.01f)
            transform.Rotate(new Vector3(0f, 0f, -rotatingSpeed));
        else if (value < -0.01f)
            transform.Rotate(new Vector3(0f, 0f, rotatingSpeed));
    }
}
