using UnityEngine;

public class TankEngine : Engine, IMove, ITurn
{   
    public const float RETREAT_SPEED_RATIO = 0.75f;
    public const float SWIM_SPEED_RATIO = 0.65f;
    public static TankEngine Instance { get; private set; }

    [SerializeField] private PhotonView myPV;
    [SerializeField] private EngineAudio engineAudio;

    public override float MoveSpeed { get { return TankEvolution.Instance.Speed; }}
    public override float TurnSpeed{ get { return TankEvolution.Instance.TurnSpeed; } }
    public float SpeedValue { get; private set; }
    public float TurnValue { get; private set; }
    public bool retreat { get; private set; }

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

        SpeedValue = Input.GetAxis("Vertical");
        if (SpeedValue < 0)
            retreat = true;
        else
            retreat = false;

        TurnValue = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        Move(CalculateMoveSpeed(), SpeedValue);

        Turn(TurnSpeed, TurnValue);
    }

    private float CalculateMoveSpeed()
    {
        if (retreat)
        {
            if (TankWaterCollision.Instance.ISwim || TankWaterCollision.Instance.ISink)
                return MoveSpeed * RETREAT_SPEED_RATIO * SWIM_SPEED_RATIO;
            else
                return MoveSpeed * RETREAT_SPEED_RATIO;
        }
        else
        {
            if (TankWaterCollision.Instance.ISwim || TankWaterCollision.Instance.ISink)
                return MoveSpeed * SWIM_SPEED_RATIO;
            else
                return MoveSpeed;
        }
    }
}
