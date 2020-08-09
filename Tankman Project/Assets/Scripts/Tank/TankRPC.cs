using UnityEngine;

public class TankRPC : Photon.MonoBehaviour
{
    public PhotonView myPV;
    public TankDeath tankStore;
    public PlayerGO playerGO;

    public GameObject explosion;
    public GameObject hitSoundPrefab;

    public Material deathMat;
    public Material defaultMat;

    public SpriteRenderer body;
    public SpriteRenderer head;



    public void ZebralemScore()
    {
        GetComponent<PhotonView>().RPC("ZebralemScoreRPC",playerGO.myPlayer.pp,50);
    }

    [PunRPC]
    void ZebralemScoreRPC(int SCORE)
    {
        GetComponent<PlayerGO>().myPlayer.score += SCORE;
    }

    public void OnDeathRPC(bool deadOrResurrection)
    {
        photonView.RPC("DeathRPC",PhotonTargets.All, deadOrResurrection);
    }

    [PunRPC]
    void DeathRPC(bool life, PhotonMessageInfo pmi)
    {
        if(life)
        {
            PlayersManager.FindPlayer(pmi.sender).score = PlayersManager.FindPlayer(pmi.sender).score/7;
            Instantiate(explosion, body.transform.position, body.transform.rotation);
            tankStore.stan.SetActive(false);
            body.material = deathMat;
            head.material = deathMat;
            PlayersManager.FindPlayer(pmi.sender).gameObject.GetComponent<TankEvolution>().HullGameObject.tag = TagManager.GetTag(Tag.StaticGameObject);
        }
        else
        {
            tankStore.stan.SetActive(true);
            body.material = defaultMat;
            head.material = defaultMat;
            PlayerSetup ps = PlayersManager.FindPlayer(pmi.sender).gameObject.GetComponent<PlayerSetup>();
            GameObject myColliderObject = GetComponent<TankEvolution>().HullGameObject;
            if (ps.photonView.isMine)
            {
                TagManager.SetGameObjectTag(myColliderObject, Tag.LocalPlayerBody);
                LayerManager.SetGameObjectLayer(myColliderObject, Layer.LocalPlayer);
            }
            else
            {
                TagManager.SetGameObjectTag(myColliderObject, Tag.RemotePlayerBody);
                LayerManager.SetGameObjectLayer(myColliderObject, Layer.RemotePlayer);
            }
        }
    }

    [PunRPC]
    void RpcAddPlayerScoreforKillBot()
    {
        GetComponent<PlayerGO>().myPlayer.score += 500;
    }

    [PunRPC]
    void SetCameraDeathRPC(PhotonMessageInfo pmi)
    {
        Debug.Log("Ustawiam u siebie CAMERE!!!!!!!!!!");
        Debug.Log(PlayersManager.FindPlayer(pmi.sender).gameObject.name);
        tankStore.camDeadTarget = PlayersManager.FindPlayer(pmi.sender).gameObject.GetComponent<TankEvolution>().HullGameObject;    //TO DO: wysłać to przez RPC
    }

    [PunRPC]
    void SetCameraDeathHowBotRPC(int ID)
    {
        Debug.Log("Ustawiam u siebie CAMERE!!!!!!!!!!");
        Debug.Log(PhotonView.Find(ID).gameObject);
        tankStore.camDeadTarget = PhotonView.Find(ID).gameObject;    //TO DO: wysłać to przez RPC
    }

    [PunRPC]
    void PlayAudioHitRPC(PhotonMessageInfo pmi)
    {
        Instantiate(hitSoundPrefab);
    }

    [PunRPC]
    public void SetCamouflage(MoroButton.Camouflage myCamouflage)
    {
        Material newMat = defaultMat;
        switch (myCamouflage)
        {
            case MoroButton.Camouflage.Default:
                newMat = defaultMat;
                break;
            case MoroButton.Camouflage.ERDL:
                newMat = ShopManager.Instance.erdlMat;
                break;
            case MoroButton.Camouflage.Marpat:
                newMat = ShopManager.Instance.marpatMat;
                break;
            case MoroButton.Camouflage.Erbsenmuster:
                newMat = ShopManager.Instance.erbseMat;
                break;
            case MoroButton.Camouflage.Puma:
                newMat = ShopManager.Instance.pumaMat;
                break;
            case MoroButton.Camouflage.Tigerstripe:
                newMat = ShopManager.Instance.tigerstripeMat;
                break;
            case MoroButton.Camouflage.DPM:
                newMat = ShopManager.Instance.dpmMat;
                break;
        }

        GetComponent<TankEvolution>().HullGameObject.GetComponent<SpriteRenderer>().material = newMat;
        GetComponent<TankEvolution>().TurretGameObject.GetComponent<SpriteRenderer>().material = newMat;
        GetComponent<TankEvolution>().TurretCapGameObject.GetComponent<SpriteRenderer>().material = newMat;
    }

    
}
