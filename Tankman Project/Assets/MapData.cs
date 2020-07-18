using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map", menuName = "MapData")]
public class MapData : ScriptableObject
{
    public Maps type;
    public GameObject prefab;

    public List<BotsCount> bots;
}
