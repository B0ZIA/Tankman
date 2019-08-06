using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ###################################
 * #        by Jakub Główczyk        #
 * #            [#][ ][ ]            #
 * ###################################
 */

/// <summary>
/// Setup local and remote player gameplay and permissions in game.
/// Player can be:
///     -LocalPlayer: you as a player
///     -RemotePlayer: remote player clone.
///     -ServerPlayer: stores game data, all permissions
/// If you want to be ServerPlayer you must be first in game room or 
/// ServerPlayer leave game and send you ServerPlayer permissions.
/// </summary>
public class PlayerSetup : Photon.MonoBehaviour
{
    [SerializeField]
    Behaviour[] componentToDisable;

    [SerializeField]
    GameObject[] objectsToDisable;

    private GameObject[] itemDetectors;
    private GameObject[] tempItemDetectors;
    static bool GameWasSetupByThisServerPlayer = true;

    const int LOCAL_PLAYER_LAYER = 10;
    const int REMOTE_PLAYER_LAYER = 11;



    void Start()
    {
        if (photonView.isMine)
        {
            SetLocalPlayerTagAndLayer();

            SetupGameScene();

            SetupRemoteBots();
        }
        else
        {
            SetRemotePlayerTagAndLayer();

            DisableComponentsAsRemotePlayer();
        }
    }

    private void SetLocalPlayerTagAndLayer()
    {
        gameObject.tag = Tag.LOCALPLAYER;
        GetComponent<TankEvolution>().HullGameObject.gameObject.tag = Tag.LOCALPLAYERBODY;
        GetComponent<TankEvolution>().HullGameObject.gameObject.layer = LOCAL_PLAYER_LAYER;
    }

    static void SetupGameScene()
    {
        DisableLoadingBaner();
    }

    static void DisableLoadingBaner()
    {
        GameObject Camera = GameObject.FindGameObjectWithTag("SceneCamera");
        Camera.SetActive(false);
    }

    private static void SetupRemoteBots()
    {
        SetRemoteBotsHP();
    }

    private static void SetRemoteBotsHP()
    {
        GameObject[] bots = GameObject.FindGameObjectsWithTag("BOTID");
        for (int i = 0; i < bots.Length; i++)
        {
            if (bots.Length > 0)
                bots[i].GetComponent<BOTSetup>().Hull.GetComponent<BOTHealt>().DownloadHP();
        }
    }

    private void SetRemotePlayerTagAndLayer()
    {
        gameObject.tag = Tag.REMOTEPLAYER;
        GetComponent<TankEvolution>().HullGameObject.gameObject.tag = Tag.REMOTEPLAYERBODY;
        GetComponent<TankEvolution>().HullGameObject.gameObject.layer = REMOTE_PLAYER_LAYER;
    }

    public void DisableComponentsAsRemotePlayer()
    {
        for (int i = 0; i < componentToDisable.Length; i++)
        {
            componentToDisable[i].enabled = false;
        }

        for (int i = 0; i < objectsToDisable.Length; i++)
        {
            objectsToDisable[i].SetActive(false);
        }
    }
    

    void Update()
    {
        if (ServerPermission() && !GameWasSetupByThisServerPlayer)
        {
            SetupGameAsFirstOrNewServerPlayer();
        }
    }

    void  SetupGameAsFirstOrNewServerPlayer()
    {
        //Niech każdy gracz zdalny ma włączony detektor itemów
        StartCoroutine(SetOtherPlayerColliderScore());

        //To ja widzę czy gracz zebrał item czy nie więc każę innym to synchronizować
        StartCoroutine(UpdateScorePlayers());

        //BOTy są lokalne tylko i wyłącznie na serverze więc niech takie będą
        SetBotForServer();

        GameWasSetupByThisServerPlayer = false;
    }

    bool ServerPermission()
    {
        if (PhotonNetwork.isMasterClient && photonView.isMine)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Co sekundę dba o to aby każdy zdalny gracz (w tej kopi gry) miał
    ///  włączony detektor "serverowy" który wysyła dane detektora do lokalnych graczy przez RPC
    /// </summary>
    /// <returns></returns>
    IEnumerator SetOtherPlayerColliderScore()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1);
            if (tempItemDetectors.Length != itemDetectors.Length)
            {
                Debug.Log("Rozpoczynam Włączanie colliderów czołgu u master Clienta! bo dołączył nowy gracz!!!");
                itemDetectors = GameObject.FindGameObjectsWithTag("ItemDetector");
                for (int i = 0; i < itemDetectors.Length; i++)
                {
                    if (itemDetectors != null)
                    {
                        itemDetectors[i].GetComponent<ItemDetector>().enabled = true;
                    }
                }
                itemDetectors = GameObject.FindGameObjectsWithTag("ItemDetector");
            }
            tempItemDetectors = GameObject.FindGameObjectsWithTag("ItemDetector");
        }
    }

    /// <summary>
    /// Co sekundę pobiera od każdego zdalnego gracza (w tej kopii gry)
    ///  jego itemy i wysyła je odpowiedni do lokaknych graczy przez RPC
    ///  itemy: score, coin, dynamit, naprawiarka, zasoby
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateScorePlayers()
    {
        itemDetectors = GameObject.FindGameObjectsWithTag("ItemDetector");
        for (int i = 0; i < itemDetectors.Length; i++)
        {
            itemDetectors[i].GetComponent<ItemDetector>().enabled = true;
        }
        tempItemDetectors = itemDetectors;

        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);
            //TODO: może jednak skorzystać z Player.players
            GameObject[] players = GameObject.FindGameObjectsWithTag("RemotePlayer");
            for (int i = 0; i < players.Length; i++)
            {
                if (players.Length > 0)
                {
                    Player tempPlayer = players[i].GetComponent<PlayerGO>().myPlayer;
                    if (tempPlayer != null)
                    {
                        Player player = tempPlayer;
                        players[i].GetComponent<PlayerSetup>().UpdateScore
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
    void UpdateScore(int SCORE, int COIN, int DYNAMIT, int NAPRAWIARKA, int ZASOBY)
    {
        //Każdy inny gracz w tej kopii gry(kopii masterClienta)
        //wysyła swoje score(czyli score które przechwuje masterClient)
        //swoim sobowtórom we wszystkich innych kopiach gry
        photonView.RPC("RpcUpdateScore", PhotonTargets.All, SCORE, COIN, DYNAMIT, NAPRAWIARKA, ZASOBY);
    }

    [PunRPC]
    void RpcUpdateScore(int SCORE, int COIN, int DYNAMIT, int NAPRAWIARKA, int ZASOBY)
    {
        if (GetComponent<PlayerGO>().myPlayer == null)
            return;

        //Każdy gracz w swojej kopi gry ustawia sobie
        // itemy które wysłał mu server
        GetComponent<PlayerGO>().myPlayer.score = SCORE;
        GetComponent<PlayerGO>().myPlayer.coin = COIN;
        GetComponent<PlayerGO>().myPlayer.Dynamit = DYNAMIT;
        GetComponent<PlayerGO>().myPlayer.Naprawiarka = NAPRAWIARKA;
        GetComponent<PlayerGO>().myPlayer.Zasoby = ZASOBY;

    }

    private static void SetBotForServer()
    {
        GameObject[] bots = GameObject.FindGameObjectsWithTag("BOTID");
        for (int ii = 0; ii < bots.Length; ii++)
        {
            if (bots != null)
                bots[ii].GetComponent<BOTSetup>().SetBotComponents();
        }
    }
}
