using UnityEngine;
using System;

/*
 * ###################################
 * #        by Jakub Główczyk        #
 * #            [#][#][ ]            #
 * ###################################
 */

public class FoodSpawn : Photon.MonoBehaviour
{
    [SerializeField]
    public GameObject prefFood;
    [SerializeField]
    public GameObject prefDynamite;
    [SerializeField]
    public GameObject prefZasoby;
    [SerializeField]
    public GameObject prefNaprawka;
    [SerializeField]
    public GameObject prefCoin;

    [SerializeField]
    public float spawnSpeed;
    [SerializeField]
    public int maxScore;
    [SerializeField]
    public int maxDynamit;
    [SerializeField]
    public int maxNaprawiarka;
    [SerializeField]
    public int maxZasoby;
    [SerializeField]
    public int maxCoin;

    private const float SPAWN_POS_MIN_X= -30.00f;
    private const float SPAWN_POS_MAX_X = 30.00f;
    private const float SPAWN_POS_MIN_Y =-30.00f;
    private const float SPAWN_POS_MAX_Y = 30.00f;



    void Start()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            Destroy(gameObject);
            return;
        }

        SetupSpawn();
    }

    public void SetupSpawn()
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        SpawnAllItem();
    }

    public void SpawnAllItem()
    {
        InvokeRepeating("GenerateFood", 0, spawnSpeed);
        InvokeRepeating("GenerateDynamite", 0, spawnSpeed);
        InvokeRepeating("GenerateZasoby", 0, spawnSpeed);
        InvokeRepeating("GenerateNaprawka", 0, spawnSpeed);
        InvokeRepeating("GenerateCoin", 0, spawnSpeed);
    }

    public static Vector3 RandomPos()
    {
        float x = UnityEngine.Random.Range(SPAWN_POS_MIN_X, SPAWN_POS_MAX_X);
        Math.Round(x, 2);
        float y = UnityEngine.Random.Range(SPAWN_POS_MIN_Y, SPAWN_POS_MAX_Y);
        Math.Round(y, 2);
        float layer = -1f;
        return new Vector3(x, y, layer);
    }

    int scoreCounter = 0;
    void GenerateFood()
    {
        if (scoreCounter < maxScore)
        {
            SpawnItem(Items.Score, RandomPos());
            scoreCounter++;
        }
    }

    int dynamiteCounter;
    void GenerateDynamite()
    {
        if (dynamiteCounter < maxDynamit)
        {
            SpawnItem(Items.Dynamit, RandomPos());
            dynamiteCounter++;
        }
    }

    int stockCounter;
    void GenerateStock()
    {
        if (stockCounter >= maxZasoby)
        {
            SpawnItem(Items.Zasoby, RandomPos());
            stockCounter++;
        }
    }

    int repairCounter;
    void GenerateRepair()
    {
        if (repairCounter >= maxNaprawiarka)
        {
            SpawnItem(Items.Naprawiarka, RandomPos());
            repairCounter++;
        }
    }

    int coinCounter;
    void GenerateCoin()
    {
        if (coinCounter >= maxCoin)
        {
            SpawnItem(Items.Coin, RandomPos());
            coinCounter++;
        }
    }

    void SpawnItem (Items whatSpawn, Vector3 pos)
    {
        Quaternion rot = Quaternion.Euler(0, 0, UnityEngine.Random.Range(1.0f, 360.0f));

        switch (whatSpawn)
        {
            case Items.Score:
                PhotonNetwork.InstantiateSceneObject("Item_Exp", pos, rot, 0, null);
                break;
            case Items.Coin:
                PhotonNetwork.InstantiateSceneObject("Item_Coin", pos, rot, 0, null);
                break;
            case Items.Dynamit:
                PhotonNetwork.InstantiateSceneObject("Item_Dynamite", pos, rot, 0, null);
                break;
            case Items.Naprawiarka:
                PhotonNetwork.InstantiateSceneObject("Item_Naprawka", pos, rot, 0, null);
                break;
            case Items.Zasoby:
                PhotonNetwork.InstantiateSceneObject("Item_Zasoby", pos, rot, 0, null);
                break;
            default:
                break;
        }
    }

    public enum Items
    {
        Score,
        Coin,
        Dynamit,
        Naprawiarka,
        Zasoby
    }
}
