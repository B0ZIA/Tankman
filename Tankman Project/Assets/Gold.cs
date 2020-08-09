using UnityEngine;

public class Gold : Item
{
    public override void GiveRevard(Player player)
    {
        player.coin += 1;

        transform.position = MapsManager.RandomPos();
        photonView.RPC("SetItemPositionRPC", PhotonTargets.All, transform.position);
    }

    [PunRPC]
    void SetItemPositionRPC(Vector3 pos)
    {
        transform.position = pos;
    }
}
