using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Host : Photon.MonoBehaviour
{
    [SerializeField]
    private HostData data;



    public void PleaseUpdatePlayersList()
    {
        photonView.RPC("InitPlayer", PhotonTargets.All, PhotonNetwork.player);
    }

    [PunRPC]
    void UpdatePlayersList()
    {

    }

}
