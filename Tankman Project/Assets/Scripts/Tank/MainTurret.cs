using System;
using UnityEngine;

public class MainTurret : TrackingMechanism
{
    public static MainTurret Instance { get; private set; }

    public bool OgraniczonaRotacja = false;
    public GameObject staticTarget;
    [SerializeField]
    private PhotonView myPV;



    public void Awake()
    {
        if(!myPV.isMine)
            enabled = false;
        else if (Instance == null)
            Instance = this;
        else
            enabled = false;
    }

    private void Start()
    {
        rotatingSpeed = TankEvolution.Instance.HeadTurnSpeed;
    }

    void FixedUpdate()
    {
        if (OgraniczonaRotacja)
            LimitedRotate();
        else
            NormalRotate();
    }

    void LimitedRotate()
    {
        if (transform.localRotation.z <= 0.2 && transform.localRotation.z >= -0.2)
            RotatingToTarget(Camera.main.ScreenToWorldPoint(Input.mousePosition), -TankEngine.Instance.TurnValue * TankEngine.Instance.TurnSpeed);
        else
            RotatingToTarget(staticTarget.transform.position, -TankEngine.Instance.TurnValue * TankEngine.Instance.TurnSpeed);
    }

    void NormalRotate ()
    {
        RotatingToTarget(Camera.main.ScreenToWorldPoint(Input.mousePosition), -TankEngine.Instance.TurnValue * TankEngine.Instance.TurnSpeed);
    }
}