using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BOTHealt : Photon.MonoBehaviour
{
    public BOTSetup botSetup;
    public float healtPoint = 10000;
    public float maxHealtPoint;
    public GameObject lastShooter;
    public Slider healtbar;
    public PhotonView myPV;
    public GameObject BulletTrailPrefab;



    public float CalculateHealth()
    {
        return healtPoint / maxHealtPoint;
    }


    void Start ()
    {
        maxHealtPoint = botSetup.MyTank.maxHp;
        healtPoint = maxHealtPoint;
    }


    void Update ()
    {
        healtbar.value = CalculateHealth();
    }




    public void SendDamage(Transform hit, float damage)
    {
        hit.GetComponent<TankDeath>().photonView.RPC("AdDamage", PhotonTargets.Others, damage);
    }

    [PunRPC]
    void AdDamage(float damages)
    {
        if (PhotonNetwork.isMasterClient)
        {
            Debug.LogError("Tylko server może dodejmować HP graczom!");
            return;
        }
        else
        {
            GameManager.LocalPlayer.gameObject.GetComponent<PlayerGO>().myPlayer.currentHp -= damages;
        }
    }

    public void ShootEffect(Vector3 StartfirePoint, Quaternion aa)
    {
        GetComponent<PhotonView>().RPC("RpcDoShootEffect", PhotonTargets.All, StartfirePoint, aa);
    }

    public void AddPlayerScoreforKillBot()
    {
        if(lastShooter != null)
            lastShooter.GetComponent<TankRPC>().myPV.RPC("RpcAddPlayerScoreforKillBot", PhotonTargets.All, null);
    }

    [PunRPC]
    void RpcDoShootEffect(Vector3 StartfirePoint, Quaternion aa)
    {
        Debug.Log("Ustaw właściciela pocisku");
        BulletTrailPrefab.GetComponent<BulletMovment>().own = gameObject;
        Instantiate(BulletTrailPrefab, StartfirePoint, aa);
    }

    public void SE(Vector3 StartfirePoint, Quaternion aa)
    {
        photonView.RPC("RpcDoShootEffectBOT",PhotonTargets.All, StartfirePoint, aa);
    }

    [PunRPC]
    void RpcDoShootEffectBOT(Vector3 StartfirePoint, Quaternion aa, PhotonMessageInfo pmi)
    {
        BulletTrailPrefab.GetComponent<BulletMovment>().own = gameObject;
        Instantiate(BulletTrailPrefab, StartfirePoint, aa);
    }

    [PunRPC]
    void AdBotDamage(float damages)
    {
        //Debug.Log("BOT: gracz mnie uderzył :(");
        GetComponent<BOTHealt>().healtPoint -= damages;
    }

    public void SetLastShooter(Player shooter)
    {
        myPV.RPC("RpcSetLastShooter",PhotonTargets.All, shooter.pp);
    }

    [PunRPC]
    void RpcSetLastShooter(PhotonPlayer pp)
    {
        if(PlayersManager.FindPlayer(pp).gameObject != null)
            lastShooter = PlayersManager.FindPlayer(pp).gameObject;
    }

    public void SyncHP(float realHP)
    {
        myPV.RPC("RpcSyncHP", PhotonTargets.All, realHP);
    }

    [PunRPC]
    void RpcSyncHP(float realHP)
    {
        healtPoint = realHP;
    }


    public void DownloadHP()
    {
        myPV.RPC("HiBOTRPC", PhotonTargets.All, null);
    }

    [PunRPC]
    void HiBOTRPC(PhotonMessageInfo pmi)
    {
        //ok już wysyłamy swoje dane każdemu
        myPV.RPC("MyBOTInfoRPC", pmi.sender, healtPoint);//wysle tu damage,score moje aby inni es sobie podmienili
    }

    [PunRPC]
    void MyBOTInfoRPC(float HP)
    {
        healtPoint = HP;
    }


}
