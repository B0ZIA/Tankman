using UnityEngine;

/*
 * ###################################
 * #        by Jakub Główczyk        #
 * #            [#][ ][ ]            #
 * ###################################
 */

public class AntiTankBarrier : Photon.MonoBehaviour
{
    private Barrier barrier;
    public RodzajZapory rodzajZapory;

    private Sprite repairedTexture;
    private Sprite destroyedTexture;
    private int[] tankTiersWhichCanDestroy;


    
    public GameObject poleDoNiszczenia;
    public GameObject poleDoNaprawy;
    public bool jestemZniszczony=false;



    void Awake()
    {
        barrier = new Barrier(repairedTexture, destroyedTexture, tankTiersWhichCanDestroy);

        //switch (rodzajZapory)
        //{
        //    case RodzajZapory.Zasiek:
        //        destroyedTexture = zasiekZniszczony;
        //        tankTierToDestroy = 0;
        //        tankTierToDestroy2 = 0;
        //        tankTierToDestroy3 = 0;
        //        break;
        //    case RodzajZapory.StalowyX: //Każdy tier różny od 1,2,3 niszczy płot
        //        destroyedTexture = stalowyXZniszczony;
        //        tankTierToDestroy = 0;
        //        tankTierToDestroy2 = 1;
        //        tankTierToDestroy3 = 2;
        //        break;
        //    case RodzajZapory.ZebySmoka:
        //        destroyedTexture = zabSmokaZniszczony;
        //        break;
        //    case RodzajZapory.Plot: //Każdy tier różny od 99 niszczy płot
        //        destroyedTexture = plotZniszczony;
        //        tankTierToDestroy = 99;
        //        tankTierToDestroy2 = 99;
        //        tankTierToDestroy3 = 99;
        //        break;
        //}
    }


    void OnCollisionEnter2D (Collision2D coll)
    {
        //Debug.Log("<color=red>~"+transform.name+"</color>");
        if(coll.gameObject.tag == Tag.LOCALPLAYERBODY || coll.gameObject.tag == Tag.REMOTEPLAYERBODY)
        {
            //Debug.Log("<color=yellow>~" + transform.name + "</color>");
            //int _tier = (int)coll.gameObject.GetComponent<TankEngine>().tankStore.GetComponent<PlayerGO>().myPlayer.tankTier;
            //if (_tier != tankTierToDestroy && _tier != tankTierToDestroy2 && _tier != tankTierToDestroy3)
            //{
            //    //Debug.Log(coll.gameObject.transform.name);
            //    if(coll.gameObject.GetComponent<TankEngine>().tankStore.gameObject.GetComponent<TankRPC>().myPV.isMine)
            //    {
            //        if (rodzajZapory == RodzajZapory.ZebySmoka)
            //            return;
            //        TriggerSth triggerSth = coll.gameObject.GetComponent<TankEngine>().tankStore.triggerSth;
            //        photonView.RPC("ZniszczZasiekRPC", PhotonTargets.AllBuffered, null);
            //        triggerSth.StartCoroutine(triggerSth.ResetColliera());
            //    }
            //}
        }
        if(coll.gameObject.tag == Tag.BOT)  
        {
            photonView.RPC("ZniszczZasiekRPC", PhotonTargets.AllBuffered, null);
        }
    }


    public enum RodzajZapory
    {
        Zasiek,
        ZebySmoka,
        StalowyX,
        Plot
    }

    public void ZniszczZasiek()
    {
        photonView.RPC("ZniszczZasiekRPC", PhotonTargets.AllBuffered, null);
    }

    public void NaprawZasiek()
    {
        photonView.RPC("NaprawZasiekRPC", PhotonTargets.AllBuffered, null);
    }

    [PunRPC]
    void ZniszczZasiekRPC()
    {
        //TO DO switch dla pozostalych
        switch (rodzajZapory)
        {
            case RodzajZapory.Zasiek:
                gameObject.tag = Tag.ZNISZCZONEZEBYSMOKA;
                break;
            case RodzajZapory.ZebySmoka:
                gameObject.tag = Tag.ZNISZCZONEZEBYSMOKA;
                break;
            case RodzajZapory.StalowyX:
                gameObject.tag = Tag.ZNISZCZONEZEBYSMOKA;
                break;
            case RodzajZapory.Plot:
                break;
            default:
                break;
        }
        jestemZniszczony = true;
        GetComponent<SpriteRenderer>().sprite = destroyedTexture;
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    [PunRPC]
    void NaprawZasiekRPC()
    {
        //TO DO switch dla pozostalych
        //switch (rodzajZapory)
        //{
        //    case RodzajZapory.Zasiek:
        //        gameObject.tag = Tag.NAPRAWIONEZEBYSMOKA;
        //        GetComponent<SpriteRenderer>().sprite = zasiekNormalny;
        //        break;
        //    case RodzajZapory.ZebySmoka:
        //        gameObject.tag = Tag.NAPRAWIONEZEBYSMOKA;
        //        GetComponent<SpriteRenderer>().sprite = zabSmokaNormalny;
        //        break;
        //    case RodzajZapory.StalowyX:
        //        gameObject.tag = Tag.NAPRAWIONEZEBYSMOKA;
        //        GetComponent<SpriteRenderer>().sprite = stalowyXNormalny;
        //        break;
        //    case RodzajZapory.Plot:
        //        break;
        //    default:
        //        break;
        //}
        jestemZniszczony = false;
        //GetComponent<SpriteRenderer>().sprite = destroyedTexture;
        GetComponent<BoxCollider2D>().isTrigger = false;
    }
}
