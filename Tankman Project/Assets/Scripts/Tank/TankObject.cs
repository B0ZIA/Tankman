using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Cały czołg, powinien znajdować się w tej części czołgu gdzie jest jego collider.
/// Odpowiada za łatwiejsze odwoływanie się do reszty podzespołów czołgu.
/// </summary>
public class TankObject : MonoBehaviour
{
    [SerializeField]
    private Collider2D collider2D;
    [SerializeField]
    private PhotonView photonView;
    [SerializeField]
    private GameObject playerGO;
    [SerializeField]
    private GameObject hull;
    [SerializeField]
    private GameObject mainTurret;
    [SerializeField]
    private Player player;

    public void Start()
    {
        Debug.Log("Ustawiam player gracza");
        //player = playerGO.GetComponent<PlayerGO>().myPlayer;
        //Debug.Log(player.nick);

    }

    /// <summary>Jeśli to nie bot to można się odwołacz do tego czyli gracza.</summary>
    public Player Player { get { if (playerGO.GetComponent<PlayerGO>().myPlayer != null) return playerGO.GetComponent<PlayerGO>().myPlayer; else { Debug.Log("Niema takiego gracza!"); return null; } } }
    /// <summary>Collider który posiada każdy czołg.</summary>
    public Collider2D Collider2D { get { return collider2D; } }
    /// <summary>PhotonView czołgu czyli też gracza</summary>
    public PhotonView PhotonView { get { return photonView; } }
    /// <summary>Kadłub czołgu, zazwyczaj posida collider, podwozie i cień i texture</summary>
    public GameObject Hull { get { return hull; } }
    /// <summary>...gracza posiada tankStore, evolution, photonview, i te najważniejsze </summary>
    public GameObject PlayerGO { get { return playerGO; } }
    /// <summary>Główna wieża czołgu, posiada klasy Shoot i TrackingMechanism(zazwyczaj dziedziczone z innych)</summary>
    public GameObject MainTurret { get { return mainTurret; } }
}
