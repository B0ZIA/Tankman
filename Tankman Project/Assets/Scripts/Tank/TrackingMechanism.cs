/*
 * ########################################################
 * #                    TurretRotate                      #
 * #                  by Jakub Glowczyk                   #
 * #      Obracanie wieży czołgu z uwzględnieniem         #
 * #              obracającego się kadłuba                #
 * ########################################################
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotating the tank tower
/// </summary>
public class TrackingMechanism : MonoBehaviour
{
    protected float rotatingSpeed;
    public virtual float RotatingSpeed
    {
        get { return rotatingSpeed; }
        set { rotatingSpeed = value; }
    }

    /// <summary>
    /// Rotating this GameObject to target. Takes into account the rotation of the tank hull
    /// </summary>
    /// <param name="targetPos">target position (you can use Input.mousePosition)</param>
    /// <param name="hullTurnValue">if it is tank turret get current value of turn tank hull (default=0)</param>
    protected void RotatingToTarget(Vector2 targetPos, float hullTurnValue = 0f)
    {
        Rigidbody2D myRB = GetComponent<Rigidbody2D>();
        Vector2 point2Target = (Vector2)transform.position - targetPos;
        point2Target.Normalize();
        float value = Vector3.Cross(point2Target, transform.right).z;
        //Debug.Log(value);
        if (value > 0.01f)
            myRB.angularVelocity = -RotatingSpeed + hullTurnValue;
        else if (value < -0.01f)
            myRB.angularVelocity = RotatingSpeed + hullTurnValue;
        else
            myRB.angularVelocity = 0;
    }
}
