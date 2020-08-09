using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    [SerializeField]
    private List<ItemPrefab> items;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void AsServerSpawnItemsForAllPlayers(MapData mapData)
    {
        if (PhotonNetwork.isMasterClient && mapData.items != null)
        {
            for (int i = 0; i < mapData.items.Count; i++)
            {
                for (int j = 0; j < mapData.items[i].count; j++)
                {
                    SpawnItem(mapData.items[i].item);
                }
            }
        }
    }

    private void SpawnItem(Items item)
    {
        var itemPrefab = items.FirstOrDefault(p => p.item == item).prefab;
        PhotonNetwork.Instantiate(itemPrefab.name, MapsManager.RandomPos(), MapsManager.RandomRot(), 0, null);
    }
}

public enum Items
{
    Item_Exp,
    Item_Coin,
    Item_Dynamite,
    Item_Naprawka,
    Item_Zasoby
}

[Serializable]
public struct ItemPrefab
{
    public Items item;
    public GameObject prefab;
}

[Serializable]
public struct ItemCount
{
    public Items item;
    public int count;
}


