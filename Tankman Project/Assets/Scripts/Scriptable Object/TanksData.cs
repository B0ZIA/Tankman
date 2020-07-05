using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Zawiera informacje o czołgach
/// </summary>
[CreateAssetMenu(fileName = "New Tank list", menuName = "Tank/List")]
public class TanksData : ScriptableObject
{
    public static List<NamedTank> tanks;
    [SerializeField] private List<NamedTank> _tanks;

    public void Setup()
    {
        tanks = _tanks;
    }

    public static TankData FindTankData(DostempneCzolgi myTank)
    {
        for (int i = 0; i < tanks.Count; i++)
        {
            if (tanks[i].tank == myTank)
                return tanks[i].tankData;
        }
        return null;
    }

    public static DostempneCzolgi FindTankEnum(TankData myTank)
    {
        for (int i = 0; i < tanks.Count; i++)
        {
            if (tanks[i].tankData == myTank)
                return tanks[i].tank;
        }
        return 0;
    }
}


[Serializable]
public struct NamedTank
{
    public DostempneCzolgi tank;
    public TankData tankData;
}


public enum DostempneCzolgi
{
    //III Rzesza
    PZI,
    PZVI,
    PZIV,
    PZVIII,
    STUGIV,
    JAGDTIGER,
    //ZSRR
    T38,
    T3475,
    IS2,
    IS7,
    SU76,
    SU85,
    //JAPONIA
    KHA_GO,
    HO_NI,
    HO_NI_III,
    HO_RO,
    HA_TO_300MM,
    SINKHOTO,
    HT_NO_VI,
    O_I,
    TYPE_I_CHI_KHE,
    TYPE_III_HI_NU
}