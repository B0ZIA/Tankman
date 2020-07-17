using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapsManager : MonoBehaviour
{
    public static MapManager Instance;



    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public static void InstantianeSceneObject()
    {
        if (PhotonNetwork.isMasterClient)
        {
            Map randomMap = (Map)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Map)).Length);
            Instance.GetComponent<PhotonView>().RPC("UstawAktualnyBiomWszystkim", PhotonTargets.AllBuffered, randomMap.ToString());

            SpawnItemSpawner();
            switch (randomMap)
            {
                case Map.Biom_Village:
                    SpawnT3476Bot(6);
                    SpawnTypeBot(3);
                    SpawnTigerBot(3);
                    break;
                //case Map.City:  TODO: odkomentować jak mapa zostanie dopracowana
                //    SpawnT3476Bot(4);
                //    SpawnTypeBot(2);
                //    SpawnTigerBot(2);
                //    break;
                default:
                    Debug.LogError("Map was not finded!");
                    break;
            }
            SpawnMap(randomMap.ToString());
        }
    }

    public static void SpawnMap(String mapName)
    {
        PhotonNetwork.InstantiateSceneObject(mapName, new Vector3(0, 0, 0), Quaternion.identity, 0, null);
    }
}
