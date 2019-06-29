using UnityEngine;


/// <summary>
/// (Detektor zbierania itemów który posiada zdalny gracz w kopii gry servera)
/// Sprawdza czy collider2D przypisany do tego samego obiektu przypadkiem nie dotknął
/// jakiegoś itemu, jeśli tak to zmienia jego pozycję przez RPC
/// </summary>
public class ItemDetector : Photon.MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D Rig2;
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
        GetComponent<Rigidbody2D>().position = Rig2.position;
        GetComponent<Rigidbody2D>().rotation = Rig2.rotation;
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

        switch (coll.gameObject.tag)
        {
            case Tag.DYNAMIT:
                if (myPlayer.Dynamit < 3)
                {
                    myPlayer.Dynamit += 1;
                    myPV.RPC("SetItemPositionRPC", PhotonTargets.AllBuffered, coll.gameObject.GetComponent<PhotonView>().viewID, FoodSpawn.SetVector());
                }
                break;
            case Tag.NAPRAWIARKA:
                if (myPlayer.Naprawiarka < 3)
                {
                    myPlayer.Naprawiarka += 1;
                    myPV.RPC("SetItemPositionRPC", PhotonTargets.AllBuffered, coll.gameObject.GetComponent<PhotonView>().viewID, FoodSpawn.SetVector());
                }
                break;
            case Tag.ZASOBY:
                if (myPlayer.Zasoby < 3)
                {
                    myPlayer.Zasoby += 1;
                    myPV.RPC("SetItemPositionRPC", PhotonTargets.AllBuffered, coll.gameObject.GetComponent<PhotonView>().viewID, FoodSpawn.SetVector());
                }
                break;
            case Tag.COIN:
                myPlayer.coin += 1;
                myPV.RPC("SetItemPositionRPC", PhotonTargets.AllBuffered, coll.gameObject.GetComponent<PhotonView>().viewID, FoodSpawn.SetVector());
                break;
            case Tag.SCORE:
                if (myPlayer.score < HUDManager.tempGranicaWbicjaLewla)
                {
                    myPlayer.score += 50;
                    myPV.RPC("SetItemPositionRPC", PhotonTargets.AllBuffered, coll.gameObject.GetComponent<PhotonView>().viewID, FoodSpawn.SetVector());
                }
                break;
        }
    }
}
