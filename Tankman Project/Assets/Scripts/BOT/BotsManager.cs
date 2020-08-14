using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

public class BotsManager : MonoBehaviour
{
    public static BotsManager Instance;

    [SerializeField]
    private List<BotPrefab> botsList;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AsMasterSpawnBotsForAllPlayers(MapData mapData)
    {
        if(PhotonNetwork.isMasterClient)
        {
            if (mapData.bots != null)
            {
                for (int i = 0; i < mapData.bots.Count; i++)
                {
                    BotPrefab prefab = botsList.FirstOrDefault(p => p.type == mapData.bots[i].type);

                    for (int j = 0; j < mapData.bots[i].count; j++)
                    {
                        Vector3 tankPos = Vector3.zero;
                        Quaternion tankRot = Quaternion.identity;

                        GameObject tank = PhotonNetwork.InstantiateSceneObject(prefab.prefab.name, tankPos, tankRot, 0, null);
                        tank.GetComponent<BOTSetup>().AsMasterSetIDForAllPlayers(BotID(prefab.type, j));
                    }
                }
            }
            else
            {
                Debug.LogWarning("MapData do not have bots list");
            }
        }
    }

    private static int BotID(BotType type, int numberOfCreate)
    {
        return (int)type + numberOfCreate;
    }
}

[Serializable]
public struct BotsCount
{
    public BotType type;
    public int count;
}

[Serializable]
public struct BotPrefab
{
    public BotType type;
    public GameObject prefab;
}

public enum BotType
{
    T3476 = 1000,
    ChiNi = 2000,
    Tiger = 3000
}