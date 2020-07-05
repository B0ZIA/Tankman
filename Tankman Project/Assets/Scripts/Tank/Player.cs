using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Zwykła smutna klasa Player, ktora posiada kazdy gracz.
/// </summary>
public class Player
{
    //Dane lokalne 
    private static List<Player> players = new List<Player>();
    public static Player myPlayer;

    //Dane podstawowe 
    public string nick;
    public NationManager.Nation nation;
    public PhotonPlayer pp;    
    public GameObject gameObject;  
    public DostempneCzolgi tank;  
    public float hp = 600f;         

    //Dane ukryte, dla servera
    public HUDManager.TankTier tankTier; 
    public int Score = 0;       
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

    public static List<Player> GetPlayers()
    {
        return players;
    }

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
