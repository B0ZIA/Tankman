using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New GameplayData", menuName = "GameplayData")]
public class HostData : ScriptableObject
{
    public static HostData Instance;

    public Maps currentMap;

    public List<Player> Players = new List<Player>();
    public static List<Player> players
    {
        get { return Instance.Players; }
    }



}
