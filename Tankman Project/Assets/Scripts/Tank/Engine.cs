using UnityEngine;
using MyFirstLiblary;

/// <summary>
/// Zwykły silnik który może obiektem obracać, go przesuwać i wydawać dzwięk 
/// </summary>
public class Engine : Photon.MonoBehaviour, IMove, ITurn
{
    private Rigidbody2D myRB;
    public virtual float MoveSpeed { get; set; }
    public virtual float TurnSpeed { get; set; }


    public virtual void Setup()
    {
        myRB = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Prousza obiekt według podanej prędkości
    /// </summary>
    /// <param name="moveSpeed">Prędkość z jaką ma się poruszać obiekt</param>
    /// <param name="inputValue">(od 0 do 1, domyślnie 1) Jeśli chcemy żeby obiekt miał "rozpęd" (np. Input.GetAxis("Vertical1"))</param>
    public virtual void Move(float moveSpeed, float inputValue = 1)
    {
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
        Vector2 movement = -transform.right * inputValue * moveSpeed * Time.deltaTime;
        // Apply this movement to the rigidbody's position.
        myRB.MovePosition(myRB.position + movement);
    }

    /// <summary>
    /// Obraca obiekt według podanej prędkości (na '-' jeśli w lewo, na '+' jeśli w prawo)
    /// </summary>
    /// <param name="turnSpeed">Prędkość z jaką ma się obracać obiekt</param>
    /// <param name="inputValue">(od 0 do 1, domyślnie 1) Jeśli chcemy żeby obiekt miał "rozpęd" (np. Input.GetAxis("Horizontal1"))</param>
    public virtual void TurnForValue(float turnSpeed, float inputValue)
    {
        // Determine the number of degrees to be turned based on the input, speed and time between frames.
        float turnValue = -inputValue * turnSpeed;
        // Apply this rotation to the rigidbody's rotation.
        myRB.MoveRotation(myRB.rotation + turnValue * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Obraca obiekt w stronę podanego obiektu z podaną prędkością 
    /// </summary>
    /// <param name="targetPos">Pozycja obiektu w którego stronę ma się obrócić </param>
    /// <param name="TurnSpeed">Prędkość poruszania</param>
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