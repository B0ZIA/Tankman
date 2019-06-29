using UnityEngine;
using System;
using System.Collections;

/*
 * ###################################
 * #        by Jakub Główczyk        #
 * #            [#][ ][ ]            #
 * ###################################
 */


//TODO: ustawić aby każdy gracz przechwywał liczbe zespawnionych itemów żeby 
// w sytuacji kiedy został on serverem kontynuował spawn
public class FoodSpawn : Photon.MonoBehaviour
{
    //Declare variables
    public GameObject Food;
    public GameObject Dynamite;
    public GameObject Zasoby;
    public GameObject Naprawka;
    public GameObject Coin;
    public float Speed;
    public int FoodMax;
    public const float minX= -30.00f;
    public const float maxX = 30.00f;
    public const float minY =-30.00f;
    public const float maxY = 30.00f;

    public int licznik=0;
	bool spawn=true;
    public const float layer = -1f;

    public int licznkiDynamit;
    public int maxDynamit;
    bool spawnDynamite = true;

    public int licznkiNaprawiarka;
    public int maxNaprawiarka;
    bool spawnNaprawiarka = true;

    public int licznkiZasoby;
    public int maxZasoby;
    bool spawnZasoby = true;

    public int licznkiCoin;
    public int maxCoin;
    bool spawnCoin = true;

    public delegate void JakisTamDel();



    void Start()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            Destroy(gameObject);
            return;
        }

        JakisTamDel jakisTam = SetupSpawn;
        jakisTam.Invoke();

        //SetupSpawn();
    }

    public void SetupSpawn()
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        RozpocznijSpawnienieObiektow();
        //StartCoroutine(Sync());
    }

    public void RozpocznijSpawnienieObiektow()
    {
        InvokeRepeating("GenerateFood", 0, Speed);
        InvokeRepeating("GenerateDynamite", 0, Speed);
        InvokeRepeating("GenerateZasoby", 0, Speed);
        InvokeRepeating("GenerateNaprawka", 0, Speed);
        InvokeRepeating("GenerateCoin", 0, Speed);
    }

    public static Vector3 SetVector()
    {
        float x = UnityEngine.Random.Range(minX, maxX);
        Math.Round(x, 2);
        float y = UnityEngine.Random.Range(minY, maxY);
        Math.Round(y, 2);
        return new Vector3(x, y, layer);
    }

    //This function creates a new food bit
    void GenerateFood()
    {
        if (licznik >= FoodMax)
            spawn = false;
        else
            spawn = true;
        if (spawn == true)
        {
			licznik++;
            //Create food object from prefab
            Spawning(CoSpawnic.Score, SetVector());
            //photonView.RPC("Spawning", PhotonTargets.MasterClient, CoSpawnic.Score, SetVector());
        }
    }

    void GenerateDynamite()
    {
        if (licznkiDynamit >= maxDynamit)
            spawnDynamite = false;
        else
            spawnDynamite = true;
        if (spawnDynamite == true)
        {
            licznkiDynamit++;
            //Create food object from prefab
            Spawning(CoSpawnic.Dynamit, SetVector());
            //photonView.RPC("Spawning", PhotonTargets.MasterClient, CoSpawnic.Dynamit, SetVector());
        }
    }
    
    void GenerateZasoby()
    {
        if (licznkiZasoby >= maxZasoby)
            spawnZasoby = false;
        else
            spawnZasoby = true;
        if (spawnZasoby == true)
        {
            licznkiZasoby++;
            //Create food object from prefab
            Spawning(CoSpawnic.Zasoby, SetVector());
            //photonView.RPC("Spawning", PhotonTargets.MasterClient, CoSpawnic.Zasoby, SetVector());
        }
    }

    void GenerateNaprawka()
    {
        if (licznkiNaprawiarka >= maxNaprawiarka)
            spawnNaprawiarka = false;
        else
            spawnNaprawiarka = true;
        if (spawnNaprawiarka == true)
        {
            licznkiNaprawiarka++;
            //Create food object from prefab
            Spawning(CoSpawnic.Naprawiarka, SetVector());
            //photonView.RPC("Spawning", PhotonTargets.MasterClient, CoSpawnic.Naprawiarka, SetVector());
        }
    }

    void GenerateCoin()
    {
        if (licznkiCoin >= maxCoin)
            spawnCoin = false;
        else
            spawnCoin = true;
        if (spawnCoin == true)
        {
            licznkiCoin++;
            //Create food object from prefab
            Spawning(CoSpawnic.Coin, SetVector());
            //photonView.RPC("Spawning", PhotonTargets.MasterClient, CoSpawnic.Coin, SetVector());
        }
    }

    //IEnumerator Sync ()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSecondsRealtime(0.1f);
    //        photonView.RPC("SyncFoodSpawnerRPC",PhotonTargets.Others,licznik,licznkiDynamit,licznkiNaprawiarka,licznkiZasoby,licznkiCoin);
    //    }
    //}

    //[PunRPC]
    //void SyncFoodSpawnerRPC (int LICZNIK,int LICZNIKDYNAMIT, int LICZNIKNAPRAWIARKA, int LICZNIKZASOBY, int LICZNIKCOIN)
    //{
    //    licznik = LICZNIK;
    //    licznkiDynamit = LICZNIKDYNAMIT;
    //    licznkiNaprawiarka = LICZNIKNAPRAWIARKA;
    //    licznkiZasoby = LICZNIKZASOBY;
    //    licznkiCoin = LICZNIKCOIN;
    //}

    void Spawning (CoSpawnic coSpawnic, Vector3 pos)
    {
        Quaternion rot = Quaternion.Euler(0, 0, UnityEngine.Random.Range(1.0f, 360.0f));

        switch (coSpawnic)
        {
            case CoSpawnic.Score:
                PhotonNetwork.InstantiateSceneObject("Item_Exp", pos, rot, 0, null);
                //Instantiate(Food, pos, rot);
                break;
            case CoSpawnic.Coin:
                PhotonNetwork.InstantiateSceneObject("Item_Coin", pos, rot, 0, null);
                //Instantiate(Coin, pos, rot);
                break;
            case CoSpawnic.Dynamit:
                PhotonNetwork.InstantiateSceneObject("Item_Dynamite", pos, rot, 0, null);
                //Instantiate(Dynamite, pos, rot);
                break;
            case CoSpawnic.Naprawiarka:
                PhotonNetwork.InstantiateSceneObject("Item_Naprawka", pos, rot, 0, null);
                //Instantiate(Naprawka, pos, rot);
                break;
            case CoSpawnic.Zasoby:
                PhotonNetwork.InstantiateSceneObject("Item_Zasoby", pos, rot, 0, null);
                //Instantiate(Zasoby, pos, rot);
                break;
            default:
                break;
        }
    }

    public enum CoSpawnic
    {
        Score,
        Coin,
        Dynamit,
        Naprawiarka,
        Zasoby
    }


}
