using UnityEngine;

public class Gold : Item, ICloneable
{
    public Gold(Sprite _texture) : base(_texture)
    {
        Debug.Log("Stworzono nowy Gold!");
    }

    public override void Create()
    {
        base.Create();
        itemObject.tag = TagManager.GetTag(Tag.Score);
        itemObject.AddComponent<PhotonView>();
    }

    protected override GameObject itemObject { get; set; }

    public override void GiveRevard(Player player)
    {
        player.coin += 1;

        transform.position = ItemManager.RandomPos();
        photonView.RPC("SetItemPositionRPC", PhotonTargets.All, transform.position);
    }

    [PunRPC]
    void SetItemPositionRPC(Vector3 pos)
    {
        transform.position = pos;
    }
}
