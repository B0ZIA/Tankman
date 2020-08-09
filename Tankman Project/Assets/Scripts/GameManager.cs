using UnityEngine;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// He is responsible for the overall operation of the game
/// </summary>
public class GameManager : Photon.MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static Player LocalPlayer
    {
        get
        {
            if (PlayersManager.FindPlayer(PhotonNetwork.player) != null)
                return PlayersManager.FindPlayer(PhotonNetwork.player);
            else
            {
                Debug.LogError("Don't find localPlayer! Mayby he isn't on scene?!");
                return null;
            }
        }
    }

    [SerializeField]
    private TanksData tanksData;   //only setup static list
    [SerializeField]
    private HostData gameplay;
    
    public static BattleMode.Type myMode;  //one from four game mode
    public static NationManager.Nation myNation = NationManager.Nation.IIIRZESZA; //player nation
    public const string GAME_VERSION = "2.8";  //You need change when you add new things for game

    //level boundary
    public const int FIRST_LEVEL_LIMIT = 1000;
    public const int SECOND_LEVEL_LIMIT = 3000;
    public const int THIRD_LEVEL_LIMIT = 6000;



    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            tanksData.Setup();
            SceneManager.LoadScene(1);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public HostData GetGameplay()
    {
        return gameplay;
    }

    /// <summary>
    /// Find GameObject on current map and return random GameObject
    /// </summary>
    /// <returns></returns>
    public GameObject GetRandomGameObject(string gameObjectTag)
    {
        GameObject[] respawns = GameObject.FindGameObjectsWithTag("PlayerRespawn");
        int i = UnityEngine.Random.Range(0, respawns.Length);
        return respawns[i];
    }

    /// <summary>
    /// Metoda którą wykonuje server. spawnuje podany obiekt(nazwaObiektu) w podanej pozycji i rotacji.
    /// Każdy gracz może poprosić server aby zespawnił obiekt, serwer sprawdza czy tak może być 
    /// i spawni go u wszystkich graczy
    /// </summary>
    /// <param name="nazwaObiektu">Nazwa prefabu, który znajduje się w Resources. On będzie spawniony w grze</param>
    /// <param name="pos">pozycja w której chcesz zespawnić obiekt</param>
    /// <param name="rot">chyba wiadomo ;)</param>
    [PunRPC]
    void SpawnSceneObjectRPC(string nazwaObiektu, Vector3 pos, Quaternion rot, PhotonMessageInfo pmi)
    {
        if (!PhotonNetwork.isMasterClient)  //jeśli nie jestem serwerem
            return;

        if (PlayersManager.FindPlayer(pmi.sender).Zasoby <= 0)  //Jeśli gracz który chce wstawiać nie ma narzędzi
            return;

        PhotonNetwork.InstantiateSceneObject(nazwaObiektu, pos, rot, 0, null);
    }

    

    #region Mordowanie gracza (lub tyklo bicie ;)

    [PunRPC]
    void OdbierzHpGraczowiRPC(PhotonPlayer ofiaraPP, float currentDamage, PhotonMessageInfo pmi)
    {
        if (!PhotonNetwork.isMasterClient)
            return;

        float DAMAGE = TanksData.FindTankData(PlayersManager.FindPlayer(pmi.sender).tank).damage;
        float DAMAGELOTERY = TanksData.FindTankData(PlayersManager.FindPlayer(pmi.sender).tank).damageLotery;
        float tempDamage = Mathf.Round(UnityEngine.Random.Range(DAMAGE - DAMAGELOTERY, DAMAGE + DAMAGELOTERY));

        if (PlayersManager.FindPlayer(pmi.sender).tank == Tanks.O_I ||
            PlayersManager.FindPlayer(pmi.sender).tank == Tanks.IS7)
            tempDamage = currentDamage;

        Player ofiara = PlayersManager.FindPlayer(ofiaraPP);
        if (ofiara.currentHp <= tempDamage)
        {
            GetComponent<PhotonView>().RPC("ZabiJOfiareRPC", ofiaraPP, ofiaraPP);
            int reward = TanksData.FindTankData(PlayersManager.FindPlayer(pmi.sender).gameObject.GetComponent<PlayerGO>().myPlayer.tank).level * 200;
            PlayersManager.FindPlayer(pmi.sender).gameObject.GetComponent<PlayerGO>().myPlayer.score += reward;
        }
        else
        {
            GetComponent<PhotonView>().RPC("OdbierzHpOfiaraRPC", PhotonTargets.All, ofiaraPP, tempDamage);
        }
    }

    [PunRPC]
    void OdbierzHpGraczowiJakoBotRPC(PhotonPlayer ofiaraPP, float DAMAGE, PhotonMessageInfo pmi)
    {
        if (!PhotonNetwork.isMasterClient)
            return;

        Player ofiara = PlayersManager.FindPlayer(ofiaraPP);

        if (ofiara.currentHp <= DAMAGE)
            GetComponent<PhotonView>().RPC("ZabiJOfiareRPC", ofiaraPP, ofiaraPP);
        else
            GetComponent<PhotonView>().RPC("OdbierzHpOfiaraRPC", PhotonTargets.All, ofiaraPP, DAMAGE);
    }

    [PunRPC]
    void OdbierzHpOfiaraRPC(PhotonPlayer ofiaraPP, float damage)
    {
        //Debug.Log("Gracz " + Player.FindPlayer(ofiaraPP).nick + " stracił " + damage + " punktów!");
        PlayersManager.FindPlayer(ofiaraPP).currentHp -= damage;
    }

    [PunRPC]
    void ZabiJOfiareRPC(PhotonPlayer ofiaraPP)
    {
        PlayersManager.FindPlayer(ofiaraPP).gameObject.GetComponent<TankDeath>().OnDead();
    }

    #endregion

    #region LoadingGameScene

    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 1 && scene.buildIndex != 0)
        {
            Matchmaking.JoinToAnyBattleOfType(myMode);
        }
    }

    #endregion
}