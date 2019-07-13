using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WskaznikManager : Photon.MonoBehaviour {

    public AntiTankBarrier.RodzajZapory barrierType;
    public bool wolnoWstawiac;
    public GameObject zielonePole;
    public GameObject czerwonePole;
    public GameObject zebySmokaPerefab;
    public GameObject musicWstawianiePrefab;

    bool returnUpdate=false;

    public void Update()
    {
        if (returnUpdate)
            return;
        GameManager.LocalPlayer.gameObject.GetComponent<TankShoot>().SetShootingOpportunity(false);

        if (Input.GetKeyDown(KeyCode.R))
            transform.Rotate(new Vector3(0, 0, 90));
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == Tag.STATICGAMEOBJECT
            || collision.tag == Tag.NAPRAWIONEZEBYSMOKA
            || collision.tag == Tag.ZNISZCZONEZEBYSMOKA)
        {
            czerwonePole.SetActive(true);
            zielonePole.SetActive(false);
            wolnoWstawiac = false;
            return;
        }
    }


    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == Tag.STATICGAMEOBJECT 
    //        || collision.tag == Tag.NAPRAWIONEZEBYSMOKA 
    //        || collision.tag == Tag.ZNISZCZONEZEBYSMOKA)
    //    {
    //        czerwonePole.SetActive(true);
    //        zielonePole.SetActive(false);
    //        wolnoWstawiac = false;
    //        return;
    //    }
    //}


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == Tag.STATICGAMEOBJECT 
            || collision.tag == Tag.NAPRAWIONEZEBYSMOKA 
            || collision.tag == Tag.ZNISZCZONEZEBYSMOKA)
        {
            zielonePole.SetActive(true);
            czerwonePole.SetActive(false);
            wolnoWstawiac = true;
            return;
        }
    }

    public void Wstaw()
    {
        returnUpdate = true;
        if (wolnoWstawiac)
        {
            Debug.Log("Wstawiam!");

            string prefabName;
            switch (barrierType)
            {
                case AntiTankBarrier.RodzajZapory.Zasiek:
                    prefabName = "Map_Zasiek";
                    break;
                case AntiTankBarrier.RodzajZapory.ZebySmoka:
                    prefabName = "Map_ZębySmoka";
                    break;
                case AntiTankBarrier.RodzajZapory.StalowyX:
                    prefabName = "Map_StalowyX";
                    break;
                default:
                    prefabName = "Map_ZębySmoka";
                    break;
            }

            GameManager.Instance.photonView.RPC("SpawnSceneObjectRPC", PhotonTargets.MasterClient, prefabName, transform.position, transform.rotation);
            GameManager.LocalPlayer.gameObject.GetComponent<PlayerGO>().myPlayer.Zasoby -= 1;
            //GameObject.FindGameObjectWithTag(Tag.LOCALPLAYER).GetComponent<TankShoot>().WolnoStrzelac();
            Instantiate(musicWstawianiePrefab, transform.position, transform.rotation);
            GameManager.LocalPlayer.gameObject.GetComponent<TankShoot>().SetShootingOpportunity(true,0.2f);
            Destroy(gameObject);
        }
    }
}
