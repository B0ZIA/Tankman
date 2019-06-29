using System.Collections.Generic;
using UnityEngine;

/*
 * ###################################
 * #        by Jakub Główczyk        #
 * #            [#][#][ ]            #
 * ###################################
 */

/// <summary>
/// Zwykła smutna klasa Player, ktora posiada kazdy gracz.
/// </summary>
public class Player
{
    //Dane lokalne 
    public static List<Player> players = new List<Player>();    //Lista wszystkich graczy w jednym miejscu
    public static Player myPlayer;    //To jest gracz lokalny

    //Dane podstawowe 
    public string nick;               //Nazwa gracza 
    public NationManager.Nation nation;  //Nacja którą wybrał gracz
    public PhotonPlayer pp;           //Dzięki temu moge w latwy sposob szukac graczy itp.
    public GameObject gameObject;     //potrzebny w odwolaniach poprzez PhotonPlayer do obiektu gracza
    public DostempneCzolgi tank;    //Czołg gracza
    public float hp = 600f;           //Punkty zycia gracza

    //Dane ukryte, dla servera
    public HUDManager.TankTier tankTier; //Poziom czołgu gracza
    public int Score = 0;             //Punkty gracza potrzebne do ulepszenia czołgu
    public int score
    {
        get { return Score; }
        set
        {
            if (value < 0 && value - Score < 0) Debug.LogError("nie możesz mień ujemnych punktow ani takowych dodawać!");
            else if (value - Score > 1500) Debug.LogError("Nie można jednorazowo zwiększyć score o więcej niż 1500 punktów!");
            else if (0 < HUDManager.tempGranicaWbicjaLewla - value)
                Score = value;
            else
                Score = HUDManager.tempGranicaWbicjaLewla;
        }
    }
    public int coin = 0;
    private int _dynamit;
    public int Dynamit
    {
        get { return _dynamit; }
        set { if (value <= 3 && value >= 0) _dynamit = value; }
    }
    private int _naprawiarka;
    public int Naprawiarka
    {
        get { return _naprawiarka; }
        set { if (value <= 3 && value >= 0) _naprawiarka = value; }
    }
    private int _zasoby;
    public int Zasoby
    {
        get { return _zasoby; }
        set { if (value <= 3 && value >= 0) _zasoby = value; }
    }
    public const int maxUpdatePoint = 12;
    public const int proportionScoreToUpdatePoint = 250;
    public int updatePoint
    {
        get
        {
            if (Score / proportionScoreToUpdatePoint < maxUpdatePoint)
                return Score / proportionScoreToUpdatePoint;
            else
                return maxUpdatePoint;
        }
    }



    public Player()
    {
        nick = "UnknowPlayer";
        nation = NationManager.Nation.IIIRZESZA;
    }

    /// <summary>
    /// Debuguje ilosc wszystkich graczy i wypisuje ich po kolei w konsoli
    /// </summary>
    public static void DebugListyGraczy()
    {
        Debug.Log("Debug listy graczy! Ilosc graczy:" +
            players.Count + " wszyscy gracze: ");
        foreach(var player in players)
        {
            Debug.Log(player.nick + ", ");
        }
    }

    /// <summary>
    /// Pozwala znalesc gracza dzieki podanemu pp,zwraca Player gracza
    /// </summary>
    /// <param name="pp"></param>
    /// <returns>PhotonPlayer szukanego gracza</returns>
    public static Player FindPlayer(PhotonPlayer pp)
    {
        for(int i = 0; i < players.Count; i++)
        {
            if (players[i].pp == pp)
                return players[i];
        }
        return null;
    }
}
