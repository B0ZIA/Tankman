using UnityEngine;


/// <summary>
/// (Detektor zbierania itemów który posiada zdalny gracz w kopii gry servera)
/// Sprawdza czy collider2D przypisany do tego samego obiektu przypadkiem nie dotknął
/// jakiegoś itemu, jeśli tak to zmienia jego pozycję przez RPC
/// </summary>
public class ItemDetector : Photon.MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D tank;
    [SerializeField]
    private TankEvolution tankEvolution;



    void Start()
    {
        if(!PhotonNetwork.isMasterClient)
        {
            Destroy(this);
            return;
        }
    }

    void Update()
    {
        GetComponent<Rigidbody2D>().position = tank.position;
        GetComponent<Rigidbody2D>().rotation = tank.rotation;
    }


    //Zbieranie Score 
    void OnTriggerEnter2D(Collider2D coll)
    {
        CheckCollision(coll);
    }

    void CheckCollision(Collider2D coll)
    {
        Player myPlayer = tankEvolution.GetComponent<PlayerGO>().myPlayer;
        PhotonView myPV = tankEvolution.GetComponent<TankRPC>().myPV;

        Tag tag = TagsManager.FindTagEnum(coll.gameObject.tag);

        switch (tag)
        {
            case Tag.PlayerSpawn:
                if (myPlayer.Dynamit < 3)
                {
                    myPlayer.Dynamit += 1;
                    //myPV.RPC("SetItemPositionRPC", PhotonTargets.AllBuffered, coll.gameObject.GetComponent<PhotonView>().viewID, ItemManager.RandomPos());
                }
                break;
            case Tag.RepairDevice:
                if (myPlayer.Naprawiarka < 3)
                {
                    myPlayer.Naprawiarka += 1;
                    //myPV.RPC("SetItemPositionRPC", PhotonTargets.AllBuffered, coll.gameObject.GetComponent<PhotonView>().viewID, ItemManager.RandomPos());
                }
                break;
            case Tag.Resources:
                if (myPlayer.Zasoby < 3)
                {
                    myPlayer.Zasoby += 1;
                }
                break;
            case Tag.Coin:
                coll.GetComponent<Gold>().GiveRevard(myPlayer);
                //myPV.RPC("SetItemPositionRPC", PhotonTargets.AllBuffered, coll.gameObject.GetComponent<PhotonView>().viewID, ItemManager.RandomPos());
                break;
            case Tag.Score:
                if (myPlayer.score < HUDManager.tempGranicaWbicjaLewla)
                {
                    myPlayer.score += 50;
                    //myPV.RPC("SetItemPositionRPC", PhotonTargets.AllBuffered, coll.gameObject.GetComponent<PhotonView>().viewID, ItemManager.RandomPos());
                }
                break;
        }
    }
}
