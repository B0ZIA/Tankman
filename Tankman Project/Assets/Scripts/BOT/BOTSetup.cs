using UnityEngine;

public class BOTSetup : Photon.MonoBehaviour
{
    //id tego bota w tym pokoju - uniwersalne 
    public int ID = 0;

    [SerializeField]
    private TankData myTank;
    public TankData MyTank { get { return myTank; } }

    public string respawnPointTag = "AMXRespawn";

    [SerializeField]
    private Behaviour[] componentToDisable;
    [SerializeField]
    private GameObject hull;
    [SerializeField]
    private GameObject turret;
    [SerializeField]
    private GameObject hud;
    [SerializeField]
    private Transform hudPoint;
    [SerializeField]
    private Transform turretPoint;

    public GameObject Hull
    {
        get
        {
            return hull;
        }
    }



    void Start()
    {
        SetBotComponents();
    }

    void Update()
    {
        turret.transform.position = turretPoint.position;
        hud.transform.position = hudPoint.position;
    }



    /// <summary>
    /// Are seting bot components in server or remote game.
    /// </summary>
    public void SetBotComponents()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            for (int i = 0; i < componentToDisable.Length; i++)
            {
                componentToDisable[i].enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < componentToDisable.Length; i++)
            {
                componentToDisable[i].enabled = true;
            }
        }
    }
}
