using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map", menuName = "MapData")]
public class MapData : ScriptableObject
{
    public Maps type;
    public GameObject prefab;
    public RangeInt XPositions = new RangeInt(-30, 30);
    public RangeInt YPositions = new RangeInt(-30, 30);
    public RangeInt RangeInt;

    public List<BotsCount> bots;
    public List<ItemCount> items;
}
