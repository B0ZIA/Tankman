using UnityEngine;
using System;

/*
 * ###################################
 * #        by Jakub Główczyk        #
 * #            [#][#][ ]            #
 * ###################################
 */

public class ItemManager : Photon.MonoBehaviour
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

    public Sprite gold_texture;


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
        //InvokeRepeating("GenerateFood", 0, spawnSpeed);
        //InvokeRepeating("GenerateDynamite", 0, spawnSpeed);
        //InvokeRepeating("GenerateZasoby", 0, spawnSpeed);
        //InvokeRepeating("GenerateNaprawka", 0, spawnSpeed);
        //InvokeRepeating("GenerateCoin", 0, spawnSpeed);
        SpawningGold();
    }

    public void SpawningGold()
    {
        Item item = new Gold(gold_texture);
        item.Create();

        for (int i = 0; i < maxCoin; i++)
        {
            Item currentGold = (Gold)item.Clone();
            currentGold.Create();
        }
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

    public static Quaternion RandomRot()
    {
        Quaternion rot = Quaternion.Euler(0, 0, UnityEngine.Random.Range(1.0f, 360.0f));
        return rot;
    }

    int scoreCounter = 0;
    void GenerateFood()
    {
        if (scoreCounter < maxScore)
        {
            SpawnItem(Items.Item_Exp, RandomPos());
            scoreCounter++;
        }
    }

    int dynamiteCounter;
    void GenerateDynamite()
    {
        if (dynamiteCounter < maxDynamit)
        {
            SpawnItem(Items.Item_Dynamite, RandomPos());
            dynamiteCounter++;
        }
    }

    int stockCounter;
    void GenerateStock()
    {
        if (stockCounter < maxZasoby)
        {
            SpawnItem(Items.Item_Zasoby, RandomPos());
            stockCounter++;
        }
    }

    int repairCounter;
    void GenerateRepair()
    {
        if (repairCounter < maxNaprawiarka)
        {
            SpawnItem(Items.Item_Naprawka, RandomPos());
            repairCounter++;
        }
    }

    int coinCounter;
    void GenerateCoin()
    {
        if (coinCounter < maxCoin)
        {
            SpawnItem(Items.Item_Coin, RandomPos());
            coinCounter++;
        }
    }

    void SpawnItem (Items whatSpawn, Vector3 pos)
    {
        Quaternion rot = RandomRot();

        PhotonNetwork.InstantiateSceneObject(whatSpawn.ToString(), pos, rot, 0, null);
    }

    public enum Items
    {
        Item_Exp,
        Item_Coin,
        Item_Dynamite,
        Item_Naprawka,
        Item_Zasoby
    }
}
