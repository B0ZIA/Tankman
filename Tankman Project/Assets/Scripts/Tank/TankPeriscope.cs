using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Peryskop czołgu, widzi graczy w pobliżu
/// </summary>
public class TankPeriscope : MonoBehaviour
{
    public List<Player> PlayersInNear { get; private set; }

    [SerializeField]
    private GameOver gameOver;
    [SerializeField]
    private Shake cameraShake;



    void OnTriggerEnter2D(Collider2D collision)
    {
        CheckPlayerInNear(collision);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        CheckPlayerOutNear(collision);
    }

    void CheckPlayerInNear(Collider2D collision)
    {
        if (collision.tag == Tag.REMOTEPLAYERBODY)
        {
            gameOver.onPlayerDead += collision.GetComponent<TankObject>().PlayerGO.GetComponent<GameOver>().tankPeriscope.CameraShake;
            //PlayersInNear.Add(collision.GetComponent<Keeper>().keep.GetComponent<PlayerGO>().myPlayer);
        }
    }

    void CheckPlayerOutNear(Collider2D collision)
    {
        if (collision.tag == Tag.REMOTEPLAYERBODY)
        {
            gameOver.onPlayerDead -= collision.GetComponent<TankObject>().PlayerGO.GetComponent<GameOver>().tankPeriscope.CameraShake;
            //PlayersInNear.Remove(collision.GetComponent<Keeper>().keep.GetComponent<PlayerGO>().myPlayer);
        }
    }

    void CameraShake()
    {
        Debug.Log("Potrząsam kamerą!");
        gameOver.ShakeCamera();
    }
}
