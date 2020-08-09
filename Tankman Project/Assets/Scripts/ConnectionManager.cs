using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ConnectionManager służy do połączenia się gracza z grą, poinformowania go
/// gdzie jest w grze, ile ma FPS'ów, PING'u
/// </summary>
public class ConnectionManager : Photon.MonoBehaviour
{
    public delegate void Action();

    //Player
    public static event Action onJoinedLobby;
    public static event Action onJoinedBattle;
    public static event Action onDisconnect;

    //Server
    public static event Action onCreateRoomServer;




    /// <summary>
    /// Dołącz do gry, tylko dołącz
    /// </summary>
    public void Connect()
    {
        PhotonNetwork.sendRate = 30;
        PhotonNetwork.sendRateOnSerialize = 30;
        PhotonNetwork.ConnectUsingSettings(GameManager.GAME_VERSION);
    }   

    /// <summary>
    /// Jeśli stworzysz pokój (czyli jesteś serverem) to w scenie roboczej
    /// Spawnisz mape i boty dla wszystkich graczy 
    /// oraz w tym mejscu informujesz przyszłych graczy że dołączyłeś
    /// </summary>
    void OnCreatedRoom()
    {
        if (PhotonNetwork.isMasterClient)
        {
            Maps map = Maps.Village;
            //Maps map = MapsManager.RandomMapType();
            GameManager.Instance.GetGameplay().currentMap = map;
            MapsManager.AsMasterSpawnMapForAllPlayers(map);
            //MapData mapData = MapsManager.Instance.GetMapData(map);
            //BotsManager.Instance.AsMasterSpawnBotsForAllPlayers(mapData);
            //ItemManager.Instance.AsServerSpawnItemsForAllPlayers(mapData);
            photonView.RPC("InitPlayer", PhotonTargets.All, PhotonNetwork.player); //Tak naprawdę wysyłasz to tylko sobie
        }
    }

    void OnDisconnectedFromPhoton()
    {
        Debug.Log("Gracz zerwał połączene");
        SceneManager.LoadScene(1);
    }

    #region GUI w lewym górnym rogu (FPS, PING, ActualGameStat)

    //zmienne dla GUI
    private string stan;
    private string PING;
    private string FPStext;
    private string serverText;
    private float deltaTime = 0.0f;

    //Stan gry w lewym górnym rogu - PING FPC ROOMSTAT
    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        style.normal.textColor = Color.white;

        GUI.Label(new Rect(5, 5, 200, 200), FPStext, style);
        GUI.Label(new Rect(65, 5, 200, 200), PING, style);
        GUI.Label(new Rect(140, 5, 200, 200), stan, style);
        GUI.Label(new Rect(215, 5, 200, 200), serverText, style);
    }


    void Start()
    {
        StartCoroutine(FPSRefreez());
    }

    void Update()
    {
        //Game State
        //If Player join to the room to instead writing "Joined" write:
        if (PhotonNetwork.connectionStateDetailed.ToString() != "Joined")
        {
            stan = PhotonNetwork.connectionStateDetailed.ToString();
        }
        else
        {
            stan = "Online: " + PhotonNetwork.room.PlayerCount;
        }
        if (PhotonNetwork.isMasterClient)
            serverText = "<color=red>Server permissions!</color>";
        else
            serverText = "";

        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        int ping = PhotonNetwork.networkingPeer.RoundTripTime;
        PING = "PING: " + ping.ToString();
    }


    IEnumerator FPSRefreez()
    {
        while(true)
        {
            float msec = deltaTime * 1000.0f;
            float FPS = 1.0f / deltaTime;
            FPStext = string.Format("FPS: {1:0.}", msec, FPS);
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    #endregion  

    void OnJoinedLobby()
    {
        GameManager.Instance.LoadGameScene();
    }

    #region Rejestrowanie (dodawanie i usuwanie) graczy

    /// <summary>
    /// Jeśli nowy gracz dołączy do gry to to wykonują wszyscy gracze prócz niego samego
    /// (serwer tego nie wykona bo sam jest w pokoju ('prucz niego samego...'))
    /// </summary>
    /// <param name="NewPlayerPP">PhotonPlayer nowego gracza</param>
    void OnPhotonPlayerConnected(PhotonPlayer NewPlayerPP)
    {
        //Aha! tylko server niech o tym informuje innych
        if (PhotonNetwork.isMasterClient)
        {
            //###
            //Tu się zaczyna cała przygoda nowego gracza
            //###

            //przywitajcie gracza wielkimi brawami
            photonView.RPC("InitPlayer", PhotonTargets.All, NewPlayerPP);
        }
    }

    /// <summary>
    /// Jeśli gracz opuści grę to to wykonują wszyscy gracze prócz niego samego
    /// (skoro opuszcza grę to ki *uj go to obchodzi)     
    /// </summary>
    /// <param name="OldPlayerPP">PhotonPlayer starego gracza</param>
    void OnPhotonPlayerDisconnected(PhotonPlayer OldPlayerPP)
    {
        //Każdy indywidualnie usuwa go ze swojej listy graczy 
        var tmpPlayer = PlayersManager.FindPlayer(OldPlayerPP);
        if (tmpPlayer != null)
        {
            PlayersManager.GetPlayers().Remove(tmpPlayer);
        }
    }

    /// <summary>
    /// Jeśli jakiś nowy gracz wbije to serwer każe wykonać to każdemu graczowi
    /// z parametrem nowego gracza który serwer wyśle 
    /// </summary>
    /// <param name="newPlayer">newPlayer</param>
    /// <param name="pmi">default</param>
    [PunRPC]
    void InitPlayer(PhotonPlayer newPlayer)
    {
        //Nowy gracz pobiera listę graczy od servera, w której go jeszcze nie ma
        if (newPlayer == PhotonNetwork.player)
            PlayersManager.Instance.UpdatePlayerList();

        //Każdy gracz (wraz z nowym) dodaje tego nowego gracza do listy graczy
        Player player = new Player();
        PlayersManager.GetPlayers().Add(player);
        player.nick = newPlayer.NickName;
        player.pp = newPlayer;

        //Nowy gracz spawni się, każdy gracz łączy przed chwilą utworzonego gracza
        // z listy z jego parametrami (które w jakiś sposób inni gracze od nowego gracza dostaną)
        if (newPlayer == PhotonNetwork.player)
            PlayersManager.Instance.SpawnPlayer();
    }

    #endregion
}