using UnityEngine;
using MyFirstLiblary;

public class Engine : Photon.MonoBehaviour, IMove, ITurn
{
    private Rigidbody2D myRB;
    public virtual float MoveSpeed { get; set; }
    public virtual float TurnSpeed { get; set; }



    public virtual void Setup()
    {
        myRB = GetComponent<Rigidbody2D>();
    }

    public virtual void Move(float moveSpeed, float inputValue = 1)
    {
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
        Vector2 movement = -transform.right * inputValue * moveSpeed * Time.deltaTime;
        // Apply this movement to the rigidbody's position.
        myRB.MovePosition(myRB.position + movement);
    }

    public virtual void Turn(float turnSpeed, float inputValue = 1)
    {
        // Determine the number of degrees to be turned based on the input, speed and time between frames.
        float turnValue = -inputValue * turnSpeed;
        // Apply this rotation to the rigidbody's rotation.
        myRB.MoveRotation(myRB.rotation + turnValue * Time.fixedDeltaTime);
    }

    public void TurnToTarget(Vector2 targetPos, float TurnSpeed)
    {
        Vector2 point2Target = (Vector2)transform.position - targetPos;
        point2Target.Normalize();
        float value = Vector3.Cross(point2Target, transform.right).z;
        if (value > 0.01f)
            myRB.angularVelocity = -TurnSpeed;
        else if (value < -0.01f)
            myRB.angularVelocity = TurnSpeed;
        else
            myRB.angularVelocity = 0;
    }
}