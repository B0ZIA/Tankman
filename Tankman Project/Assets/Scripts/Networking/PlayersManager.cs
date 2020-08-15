using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : Photon.MonoBehaviour
{
    public static PlayersManager Instance;
    //private static List<Player> players = new List<Player>();
    [SerializeField]
    private HostData host;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (HostData.Instance == null)
            HostData.Instance = host;
        HostData.Instance.Players = new List<Player>();
    }

    public static List<Player> GetPlayers()
    {
        return HostData.players;
    }

    public static Player FindPlayer(PhotonPlayer pp)
    {
        for (int i = 0; i < HostData.players.Count; i++)
        {
            if (HostData.players[i].pp == pp)
                return HostData.players[i];
        }
        return null;
    }

    /// <summary>
    /// Ustawia wszystkich graczy zdalnych w mojej kopii gry
    /// (tylko to co potrzebuje zwykły szary gracz)
    /// </summary>
    public void UpdatePlayerList()
    {
        if (!PhotonNetwork.isMasterClient)
            photonView.RPC("SetPlayersListRPC", PhotonTargets.MasterClient, null);
    }

    public void UpdateMyPlayer(int viewID)
    {
        photonView.RPC("SetPlayerRPC", PhotonTargets.All,
            GameManager.LocalPlayer.pp,
            viewID,
            NationManager.myNation,
            TanksData.FindTankData(NationManager.ReturnStartTank(NationManager.myNation)).maxHp,
            NationManager.ReturnStartTank(NationManager.myNation),
            true); //Player gracza wysyłamy jeśli target ma go na liście graczy
    }

    public void SpawnPlayer()
    {
        GameObject myPlayerGO = PhotonNetwork.Instantiate("Player", GameManager.Instance.GetRandomGameObject(TagsManager.GetTag(Tag.PlayerSpawn)).transform.position,
            GameManager.Instance.GetRandomGameObject(TagsManager.GetTag(Tag.PlayerSpawn)).transform.rotation, 0);
        UpdateMyPlayer(myPlayerGO.GetComponent<PhotonView>().viewID);
    }

    [PunRPC]
    void SpawnujGraczaRPC(int pvID, PhotonMessageInfo pmi)
    {
        GameObject newPlayerGO = PhotonView.Find(pvID).gameObject;
        Player player;

        if (PlayersManager.FindPlayer(pmi.sender) == null)
        {
            Debug.LogError("Błąd krytyczny!");
            return;
        }
        else
        {
            player = PlayersManager.FindPlayer(pmi.sender);
        }

        player.gameObject = newPlayerGO;    //Tu jest problem
        newPlayerGO.GetComponent<PlayerGO>().myPlayer = player;
        newPlayerGO.GetComponent<PlayerGO>().myPlayer.nation = GameManager.myNation;
        newPlayerGO.name = "Player_" + player.nick; //+nick
    }

    /// <summary>
    /// Wysyłam (ja server) dane wszystkich graczy proszącemu o to 
    /// </summary>
    /// <param name="pmi">proszący o dane</param>
    [PunRPC]
    void SetPlayersListRPC(PhotonMessageInfo pmi)
    {
        if (!PhotonNetwork.isMasterClient)
            return;

        for (int i = 0; i < PlayersManager.GetPlayers().Count; i++)
        {
            var player = PlayersManager.GetPlayers()[i];

            if (player.pp != pmi.sender)
                photonView.RPC("SetPlayerRPC", pmi.sender,
                    player.pp,
                    player.gameObject.GetComponent<PhotonView>().viewID,
                    player.nation,
                    player.gameObject.GetComponent<PlayerGO>().myPlayer.currentHp,
                    player.gameObject.GetComponent<PlayerGO>().myPlayer.tank,
                    false); //true wysyłamy jeśli target ma gracza na liście graczy
        }
    }

    /// <summary>
    /// To wykonuje gracz proszący o dane reszty graczy,
    ///  ustawia podstawowe dane o tych graczach
    /// </summary>
    /// <param name="remotePlayerPP"></param>
    /// <param name="remotePlayerIndex"></param>
    /// <param name="remotePlayerID"></param>
    /// <param name="remoteNation"></param>
    /// <param name="remoteHP"></param>
    /// <param name="playerIsOnPlayersList"></param>
    [PunRPC]
    void SetPlayerRPC(PhotonPlayer remotePlayerPP, int remotePlayerID, NationManager.Nation remoteNation,
                        float remoteHP, Tanks remoteTank, bool playerIsOnPlayersList = false)
    {
        //Tworzę gracza i dodaje go do mojej listy graczy 
        Player player;
        if (playerIsOnPlayersList)
        {
            player = PlayersManager.FindPlayer(remotePlayerPP);
        }
        else
        {
            player = new Player();
            PlayersManager.GetPlayers().Add(player);
        }

        //Ustawiam podstawowe dane tego gracza 
        player.nick = remotePlayerPP.NickName;
        player.pp = remotePlayerPP;
        player.nation = remoteNation;
        player.currentHp = remoteHP;
        player.tank = remoteTank;

        //Ustawiam odwołanie gracza z listy i właściwego obiektu
        GameObject newPlayerGO = PhotonView.Find(remotePlayerID).gameObject;
        player.gameObject = newPlayerGO;
        newPlayerGO.GetComponent<PlayerGO>().myPlayer = player;

        //Taki bajer
        newPlayerGO.name = "Player_" + player.nick;

        //Ustawiam dane widoczne dla gracza proszącego (ustawiam czołg, sliderHP i nick) 
        newPlayerGO.GetComponent<PlayerGO>().myPlayer.currentHp = player.currentHp;
        newPlayerGO.GetComponent<PlayerGO>().myPlayer.nick = player.nick;
        newPlayerGO.GetComponent<Nick>().nick.text = player.nick;
        newPlayerGO.GetComponent<PlayerGO>().myPlayer.nation = player.nation;
        newPlayerGO.GetComponent<PlayerGO>().myPlayer.tank = player.tank;
        newPlayerGO.GetComponent<TankEvolution>().SetStartTankHowNewPlayer(player.tank);
    }
}