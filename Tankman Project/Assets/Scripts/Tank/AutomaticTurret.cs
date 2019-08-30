using UnityEngine;

public class AutomaticTurret : TrackingMechanism
{
    [SerializeField]
    private TurretData turret;
    [SerializeField]
    private GameObject curretTarget;
    [SerializeField]
    private GameObject staticTarget;

    [SerializeField]
    private float turnSpeed = 20;
    public override float RotatingSpeed
    {
        get { return turnSpeed; }
    }


    void Start ()
    {
        //Działko na IS7 jeśli nie widzi przeciwnika obraca się w tą samą stronę co główna wieża
        if(turret.towerType == TowerType.IS7OnHead)
		    curretTarget = TankEvolution.Instance.BarrelEndPoint.gameObject;
        //(...) natomiast reszta działek w tę samą stronę co kadłub  
        else
            curretTarget = staticTarget;
    }

    public void FixedUpdate()
    {
        switch (turret.towerType)
        {
            case TowerType.O_ITopLeft:
                LimitedRotate(WhereToRotate.Forward);
                break;

            case TowerType.O_ITopRight:
                LimitedRotate(WhereToRotate.Forward);
                break;

            case TowerType.O_IButton:
                LimitedRotate(WhereToRotate.Backwards);
                break;

            case TowerType.IS7OnHead:
                NormalRotate();
                break;

            default:
                break;
        }
    }


    void NormalRotate()
    {
        if (curretTarget != null)
            RotatingToTarget(curretTarget.transform.position);
    }

    protected void LimitedRotate(WhereToRotate wKtoraStroneObracac = WhereToRotate.Forward)
    {
        switch (wKtoraStroneObracac)
        {
            case WhereToRotate.Forward:
                if (transform.localRotation.z <= 0.3 && transform.localRotation.z >= -0.3)
                    RotatingToTarget(curretTarget.transform.position);
                else
                    RotatingToTarget(staticTarget.transform.position);
                break;

            case WhereToRotate.Backwards:
                if (transform.localRotation.w <= 0.3 && transform.localRotation.w >= -0.3)
                    RotatingToTarget(curretTarget.transform.position);
                else
                    RotatingToTarget(staticTarget.transform.position);
                break;
        }
    }


    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == TagManager.GetTag(Tag.Bot) || coll.gameObject.tag == TagManager.GetTag(Tag.RemotePlayerBody))
        {
            curretTarget = coll.gameObject;
        }
    }

	void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == TagManager.GetTag(Tag.Bot) || coll.gameObject.tag == TagManager.GetTag(Tag.RemotePlayerBody))
        {
            if (turret.towerType == TowerType.IS7OnHead)
                curretTarget = TankEvolution.Instance.BarrelEndPoint;
            else
                curretTarget = staticTarget;
        }
    }

    public enum WhereToRotate
    {
        Forward,
        Backwards
    }
}
