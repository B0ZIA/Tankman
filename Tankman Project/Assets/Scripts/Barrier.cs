using UnityEngine;

public class Barrier : Photon.MonoBehaviour
{
    [SerializeField]
    protected Sprite repairedTexture;
    [SerializeField]
    protected Sprite destroyedTexture;

    public bool destroyed = false;



    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == Tag.LOCALPLAYERBODY || coll.gameObject.tag == Tag.BOT)
            Destroy();
    }

    public void Destroy()
    {
        photonView.RPC("DestroyBarrierRPC", PhotonTargets.AllBuffered, null);
    }

    public void Repair()
    {
        photonView.RPC("RepairBarrierRPC", PhotonTargets.AllBuffered, null);
    }

    [PunRPC]
    protected void DestroyBarrierRPC()
    {
        gameObject.tag = Tag.DESTROYED_BARRIER;
        destroyed = true;
        GetComponent<SpriteRenderer>().sprite = destroyedTexture;
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    [PunRPC]
    protected void RepairBarrierRPC()
    {
        destroyed = false;
        gameObject.tag = Tag.REPAIRED_BARRIER;
        GetComponent<SpriteRenderer>().sprite = repairedTexture;
        GetComponent<BoxCollider2D>().isTrigger = false;
    }
}
public enum BarrierType
{
    Zasiek,
    ZebySmoka,
    StalowyX,
    Plot
}
