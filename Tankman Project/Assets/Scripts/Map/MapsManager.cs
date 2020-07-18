using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class MapsManager : Photon.MonoBehaviour
{
    public static MapsManager Instance;

    [SerializeField]
    private List<MapData> maps;



    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public static void AsMasterSpawnMapForAllPlayers(Maps map)
    {
        if (PhotonNetwork.isMasterClient)
        {
            var prefab = Instance.maps.FirstOrDefault(p => p.type == map).prefab;
            PhotonNetwork.InstantiateSceneObject(prefab.name, Vector3.zero, Quaternion.identity, 0, null);
        }
    }

    public static void SpawnMap(String mapName)
    {
        PhotonNetwork.InstantiateSceneObject(mapName, new Vector3(0, 0, 0), Quaternion.identity, 0, null);
    }

    public static Maps RandomMapType()
    {
        Maps randomMap = (Maps)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Maps)).Length);
        return randomMap;
    }

    public MapData GetMapData(Maps map)
    {
        return new MapData();
    }
}

public enum Maps
{
    Village,
    Town
}
