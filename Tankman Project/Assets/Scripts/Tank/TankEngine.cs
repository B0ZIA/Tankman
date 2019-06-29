using UnityEngine;

/*
 * ###################################
 * #          by Jakub Główczyk      #
 * #            [#][#][#]            #
 * ###################################
 */

public class TankEngine : Engine, ICanMove, ICanTurn
{
    public static TankEngine Instance { get; private set; }
    public GameOver tankStore; //dla Zapór 
    [SerializeField]
    private PhotonView myPV;
    private EngineAudio engineAudio;

    public override float MoveSpeed { get { return TankEvolution.Instance.Speed; }}
    public override float TurnSpeed{ get { return TankEvolution.Instance.TurnSpeed; } }
    public float SpeedValue { get; private set; }
    public float TurnValue { get; private set; }
    public bool cofanie { get; private set; }

    [Header("Audio:")]
    [SerializeField]
    private AudioSource audioSource; // Reference to the audio source used to play engine sounds.
    [SerializeField]
    private AudioClip engineIdling; // Audio to play when the tank isn't moving.
    [SerializeField]
    private AudioClip engineDriving; // Audio to play when the tank is moving.
    [SerializeField]
    private float pitchRange; // The amount by which the pitch of the engine noises can vary.


    void Awake()
    {
        if (!myPV.isMine)
            enabled = false;
        else if (Instance == false)
            Instance = this;
        else
            enabled = false;

        engineAudio = new EngineAudio(audioSource, engineIdling, engineDriving, pitchRange);
    }

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        engineAudio.SetAudio(SpeedValue, TurnValue);

        SpeedValue = Input.GetAxis("Vertical1");
        TurnValue = Input.GetAxis("Horizontal1");

        if (Input.GetKey(KeyCode.S))
            //cofanie
            cofanie = true;
        else if (Input.GetKey(KeyCode.W))
            //jazda!
            cofanie = false;
    }

    private void FixedUpdate()
    {
                                      float tempSpeed = (cofanie) 
                                                    ?
                (TankWaterCollision.Instance.ISwim || TankWaterCollision.Instance.ISink)
                /*jeśli cofam w wodzie*/            ?     /*jeśli cofam 'nie' w wodzie*/
                MoveSpeed * 0.75f * 0.65f           :                 MoveSpeed * 0.75f 

                :
                
                (TankWaterCollision.Instance.ISwim || TankWaterCollision.Instance.ISink)
                /*jeśli jadę normalnie przez wode*/ ?           /*jeśli jadę normalnie*/
                MoveSpeed * 0.65f                   :                      MoveSpeed;

        Move(tempSpeed, SpeedValue);
        TurnForValue(TurnSpeed, TurnValue);
    }
}