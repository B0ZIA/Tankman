using System.Collections;
using UnityEngine;


/// <summary>
/// coś co wykrywa czy czołg jest w wodzie. Jest woda zwykła w której czołg zwalnia 
///  i jest woda głęboka w której dodatkowo czołg zaczyna tonąć
/// </summary>
public class TankWaterCollision : MonoBehaviour
{
    public static TankWaterCollision Instance { get; private set; }

    [SerializeField]
    private PhotonView myPV;
    [SerializeField]
    private GameOver playerGameOver;

    private int maxSinkTime = 60;   //Tyle czasu potrzebuje aby utonąć
    private int sinkTime;   //aktualny czas przebywania w wodzie
    private bool IHasBeenSinking;   //czy jestem w procesie topienia się
    private bool sink = false;  //tonę
    private bool swim = false;  //pływam

    public bool ISwim { get { return swim; } }
    public bool ISink { get { return sink; } }



    public void Setup()
    {
        sinkTime = maxSinkTime;
    }

    public void Awake()
    {
        if (!myPV.isMine)
            enabled = false;
        else if (Instance == null)
            Instance = this;
        else
            enabled = false;
    }

    public void Start()
    {
        Setup();
    }

    public void Update()
    {
        if (!IHasBeenSinking)
            HUDManager.Instance.meltTime.text = "";
        else
            HUDManager.Instance.meltTime.text = "0:0" + sinkTime.ToString();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.tag);
        if (collision.tag == TagManager.GetTag(Tag.DeepWater))
        {
            sink = true;

            if (!IHasBeenSinking)
                StartCoroutine(TopienieCzolgu());
        }
        if (collision.tag == TagManager.GetTag(Tag.Water))
        {
            swim = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == TagManager.GetTag(Tag.DeepWater))
        {
            sink = false;
            sinkTime = maxSinkTime;
            HUDManager.Instance.meltTime.text = "";
        }
        if (collision.tag == TagManager.GetTag(Tag.Water))
        {
            swim = false;
        }
    }

    IEnumerator TopienieCzolgu()
    {
        IHasBeenSinking = true;
        bool swimOut = false;
        while(sinkTime > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            if (!sink)
            {
                swimOut = true;
                break;
            }
            sinkTime--;
        }
        if (!swimOut)
            playerGameOver.OnDead();
        IHasBeenSinking = false;
    }
}
