using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Host : Photon.MonoBehaviour
{
    #region SINGLETON
    public static Host Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

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

    public IEnumerator SendingRemotePlayersTheirDataStoredByServer()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);
            GameObject[] players = GameObject.FindGameObjectsWithTag("RemotePlayer");
            for (int i = 0; i < players.Length; i++)
            {
                if (players.Length > 0)
                {
                    Player tempPlayer = players[i].GetComponent<PlayerGO>().myPlayer;
                    if (tempPlayer != null)
                    {
                        Player player = tempPlayer;
                        players[i].GetComponent<PlayerSetup>().UpdatePlayerData
                        (
                            player.score,
                            player.coin,
                            player.Dynamit,
                            player.Naprawiarka,
                            player.Zasoby
                        );
                    }
                }
            }
        }
    }
}
