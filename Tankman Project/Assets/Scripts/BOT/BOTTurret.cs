using UnityEngine;

public class BOTTurret : TrackingMechanism
{
    public GameObject target;
    public GameObject staticTarget;
    public BOTEngine botMovement;
    public BOTSetup botSetup;

    void Awake()
    {
        rotatingSpeed = botSetup.MyTank.headTurnSpeed;
    }

    void Update()
    {
        if (target != null)
        {
            RotatingToTarget(target.transform.position);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == TagManager.GetTag(Tag.LocalPlayerBody) || coll.gameObject.tag == TagManager.GetTag(Tag.RemotePlayerBody))
        {
            target = coll.gameObject;
        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == TagManager.GetTag(Tag.LocalPlayerBody) || coll.gameObject.tag == TagManager.GetTag(Tag.RemotePlayerBody))
        {
            target = staticTarget;
        }
    }
}
