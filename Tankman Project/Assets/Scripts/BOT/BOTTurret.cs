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
        if (coll.gameObject.tag == Tag.LOCALPLAYERBODY || coll.gameObject.tag == Tag.REMOTEPLAYERBODY)
        {
            target = coll.gameObject;
        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == Tag.LOCALPLAYERBODY || coll.gameObject.tag == Tag.REMOTEPLAYERBODY)
        {
            target = staticTarget;
        }
    }
}
