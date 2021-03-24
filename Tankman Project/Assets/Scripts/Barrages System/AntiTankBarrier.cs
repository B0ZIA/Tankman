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
        if (coll.gameObject.CompareTag(TagsManager.GetTag(Tag.LocalPlayerBody)))
        {
            HUDManager.TankTier otherPlayerTankTier = coll.gameObject.GetComponentInParent<PlayerGO>().myPlayer.tankTier;
            Debug.Log(otherPlayerTankTier);
            for (int i = 0; i < tankTiersWhichCanDestroy.Length; i++)
            {
                if (tankTiersWhichCanDestroy[i] == otherPlayerTankTier)
                {
                    Destroy();
                    TriggerSth triggerSth = coll.gameObject.GetComponentInParent<TankDeath>().triggerSth;
                    triggerSth.StartCoroutine(triggerSth.ResetColliera());
                    break;
                }
            }
        }
        if(coll.gameObject.tag == TagsManager.GetTag(Tag.Bot))  
        {
            Destroy();
        }
    }
}
