using UnityEngine;

/*
 * ###################################
 * #        by Jakub Główczyk        #
 * #            [#][ ][ ]            #
 * ###################################
 */

public class AntiTankBarrier : Barrier
{
    [SerializeField]
    private HUDManager.TankTier[] tankTiersWhichCanDestroy;
    [SerializeField]
    public GameObject fieldForDestroy;
    [SerializeField]
    public GameObject fieldForRepair;



    void OnCollisionEnter2D (Collision2D coll)
    {
        if (coll.gameObject.tag == Tag.LOCALPLAYERBODY)
        {
            HUDManager.TankTier otherPlayerTankTier = coll.gameObject.GetComponent<TankEngine>().tankStore.playerSetup.GetComponent<PlayerGO>().myPlayer.tankTier;
            Debug.Log(otherPlayerTankTier);
            for (int i = 0; i < tankTiersWhichCanDestroy.Length; i++)
            {
                if (tankTiersWhichCanDestroy[i] == otherPlayerTankTier)
                {
                    Destroy();
                    TriggerSth triggerSth = coll.gameObject.GetComponent<TankEngine>().tankStore.triggerSth;
                    triggerSth.StartCoroutine(triggerSth.ResetColliera());
                    break;
                }
            }
        }
        if(coll.gameObject.tag == Tag.BOT)  
        {
            Destroy();
        }
    }
}
