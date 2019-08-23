using System.Collections;
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
///     -RemotePlayer: remote player clone in your game
///     -ServerPlayer: player who stores game data and have all permissions
/// If you want to be ServerPlayer you must be first in game room or 
/// ServerPlayer leave game and send you ServerPlayer permissions.
/// </summary>
public class PlayerSetup : Photon.MonoBehaviour
{
    [SerializeField]
    private Behaviour[] remotePlayerInactiveComponents;

    [SerializeField]
    private GameObject[] remotePlayerInactiveGameObjects;

    private GameObject[] itemDetectors;
    private GameObject[] tempItemDetectors;
    static bool GameWasSetupForMeAsServerPlayer = true;

    public const int LOCAL_PLAYER_LAYER = 10;
    public const int REMOTE_PLAYER_LAYER = 11;
    const int TIME_TO_UPDATE_ITEM_DETECTOR = 1;


    void Start()
    {
        GameObject myColliderObject = GetComponent<TankEvolution>().HullGameObject;

        if (photonView.isMine)
        {
            SetGameObjectTag(myColliderObject, Tag.REMOTEPLAYERBODY);
            SetGameObjectLayer(myColliderObject, LOCAL_PLAYER_LAYER);

            SetupGameScene();

            SetupRemoteBots();
        }
        else
        {
            SetGameObjectTag(myColliderObject, Tag.REMOTEPLAYERBODY);
            SetGameObjectLayer(myColliderObject, REMOTE_PLAYER_LAYER);

            DisableComponents(remotePlayerInactiveComponents);
            DisableGameObjects(remotePlayerInactiveGameObjects);
        }
    }

    public void SetGameObjectTag(GameObject gameObject, string tag)
    {
        gameObject.tag = tag;
    }

    public void SetGameObjectLayer(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
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

    public void DisableComponents(Behaviour[] components)
    {
        for (int i = 0; i < components.Length; i++)
        {
            components[i].enabled = false;
        }
    }

    public void DisableGameObjects(GameObject[] gameObjects)
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(false);
        }
    }

    void Update()
    {
        if (ServerPermission() && !GameWasSetupForMeAsServerPlayer)
        {
            Debug.Log("I am server");
            SetupGameForMeAsServerPlayer();
        }
    }

    bool ServerPermission()
    {
        if (PhotonNetwork.isMasterClient && photonView.isMine)
            return true;
        else
            return false;
    }

    void SetupGameForMeAsServerPlayer()
    {
        StartCoroutine(SetupingPlayersItemDetectorWhenNewPlayerJoin());

        SetItemsDetector();
        tempItemDetectors = itemDetectors;
        StartCoroutine(SendingRemotePlayersTheirDataStoredByServer());

        SetBotForServer();

        GameWasSetupForMeAsServerPlayer = false;
    }
    
    IEnumerator SetupingPlayersItemDetectorWhenNewPlayerJoin()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(TIME_TO_UPDATE_ITEM_DETECTOR);

            if (tempItemDetectors.Length != itemDetectors.Length)
            {
                SetItemsDetector();
                itemDetectors = GameObject.FindGameObjectsWithTag("ItemDetector");
            }

            tempItemDetectors = GameObject.FindGameObjectsWithTag("ItemDetector");
        }
    }

    private void SetItemsDetector()
    {
        itemDetectors = GameObject.FindGameObjectsWithTag("ItemDetector");
        for (int i = 0; i < itemDetectors.Length; i++)
        {
            if (itemDetectors != null)
            {
                itemDetectors[i].GetComponent<ItemDetector>().enabled = true;
            }
        }
    }

    IEnumerator SendingRemotePlayersTheirDataStoredByServer()
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
    void UpdatePlayerData(int SCORE, int COIN, int DYNAMIT, int NAPRAWIARKA, int ZASOBY)
    {
        photonView.RPC("SetPlayerDataRPC", PhotonTargets.All, SCORE, COIN, DYNAMIT, NAPRAWIARKA, ZASOBY);
    }

    [PunRPC]
    void SetPlayerDataRPC(int SCORE, int COIN, int DYNAMIT, int NAPRAWIARKA, int ZASOBY)
    {
        if (GetComponent<PlayerGO>().myPlayer == null)
            return;

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
