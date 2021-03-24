using UnityEngine;

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
        Vector2 movement = -transform.right * inputValue * moveSpeed * Time.deltaTime;
        myRB.MovePosition(myRB.position + movement);
    }

    public virtual void Turn(float turnSpeed, float inputValue = 1)
    {
        float turnValue = -inputValue * turnSpeed;
        transform.Rotate(new Vector3(0f, 0f, turnValue));
    }

    public void TurnToTarget(Vector2 targetPos, float TurnSpeed)
    {
        Vector2 point2Target = (Vector2)transform.position - targetPos;
        point2Target.Normalize();
        float value = Vector3.Cross(point2Target, transform.right).z;
        if (value > 0.01f)
            transform.Rotate(new Vector3(0f, 0f, -TurnSpeed));
        else if (value < -0.01f)
            transform.Rotate(new Vector3(0f, 0f, TurnSpeed));
    }
}