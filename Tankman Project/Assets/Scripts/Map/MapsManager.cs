using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        return maps.FirstOrDefault(p => p.type == map);
    }

    public static Vector3 RandomPos()
    {
        var mapData = Instance.maps.FirstOrDefault(p => p.type == GameManager.Instance.GetGameplay().currentMap);
        
        float x = UnityEngine.Random.Range(mapData.XPositions.start, mapData.XPositions.end);
        Math.Round(x, 2);
        float y = UnityEngine.Random.Range(mapData.YPositions.start, mapData.YPositions.end);
        Math.Round(y, 2);
        float layer = -1f;
        return new Vector3(x, y, layer);
    }

    public static Quaternion RandomRot()
    {
        Quaternion rot = Quaternion.Euler(0, 0, UnityEngine.Random.Range(1.0f, 360.0f));
        return rot;
    }
}

public enum Maps
{
    Village,
    Town
}
