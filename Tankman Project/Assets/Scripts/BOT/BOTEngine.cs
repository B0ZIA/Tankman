using UnityEngine;
using UnityEditor;
using System.Collections;


/// <summary>
/// Scritp responsible for Bot AI
/// </summary>
public class BOTEngine : Engine, IMove, ITurn
{
    public PhotonView myPV;
        
    private float tempMoveSpeed;
    public override float MoveSpeed { get { return botSetup.MyTank.speed; }}

    private float tempTurnSpeed;
    public override float TurnSpeed { get { return botSetup.MyTank.turnSpeed * 7f; }}

    private float respawnTime = 30f;    //Czas potrzebny do odrodzenia się czołgu po jego śmierci 
    private float shootDistance = 1f;   //Zasięg strzału czołgu 
    private bool szukamTargetu = false; //Jestem w procesie szukania targetu, nie mam ustawionego
    private bool canDeath = true;   //Jeśli jestem martwy to nie mogę umierać ;)

    [SerializeField]
    private LayerMask toFindStreet;
    [SerializeField]
    private BOTSetup botSetup;

    public Radar radar;
    public GameObject target;
    public GameObject tempTargetRoad;
    public GameObject[] respawns;
    public Material deathMatereial;
    public Material defaultMaterial;

    private bool move;



    public void Awake()
    {
        tempMoveSpeed = MoveSpeed;
        tempTurnSpeed = TurnSpeed;
        myPV = GetComponent<PhotonView>();
    }

    void Start()
    {
        Setup();
        SetPosition();
        SetTargetPosition();
        move = true;
    }

    void Update()
    {
        if (!move)
        {
            tempTurnSpeed = 0;
            tempMoveSpeed = 0;
        }
        if (tylkoRazUstawTarget)
        {
            SetTargetPosition();
            tylkoRazUstawTarget = false;
        }

        if (botHealt.healtPoint <= 0)
        {
            if (canDeath == true)
            {
                StartCoroutine(OdliczanieDoRespawnu());
                botHealt.AddPlayerScoreforKillBot();
            }
        }

        if(target != null)
        {
            TurnToTarget(target.transform.position, tempTurnSpeed);

            float angle = Vector3.Angle(transform.right, target.transform.position - transform.position);

            if (angle > 90)  // Target prze demną
                Move(tempMoveSpeed, 0.5f);
            else            //Target za mną
                Move(-tempMoveSpeed, 0.5f);
        }
    }

    public void SetPosition()
    {
        respawns = GameObject.FindGameObjectsWithTag(botSetup.respawnPointTag);
        int index = botSetup.ID;

        while (index < 0)
        {
            if (respawns[botSetup.ID] == null)
                index -= 1;
            else
                break;
        }

        if (respawns[botSetup.ID] != null)
            transform.position = new Vector3(respawns[botSetup.ID].transform.position.x, respawns[botSetup.ID].transform.position.y, -2);
        else
            Debug.Log("Nie znaleziono na mapie miejsca gdzie mozna by było zespanic bota!");
    }

    public GameObject stan;
    public GameObject[] botHead;
    public BOTHealt botHealt;
    private bool movement = true;
    public GameObject tankExplosion;
    public GameObject[] targets;


    
    void SetTargetPosition()
    {
        //UnityEngine.Debug.Log("Rozpoczynam akcje szukania targetu !");
        //Rozpoczyna akcję szukania targetu,
        // jeśli taka akcja już się wykonuje to jej nie powiela
        if(!szukamTargetu)
            StartCoroutine(FindTarget());
    }



    IEnumerator FindTarget()
    {
        int bezpiecznik = 0;
        szukamTargetu = true;
        while (true)
        {
            //zakreśla w nieskończoność kułka radarem aby znaleźć target
            RaycastHit2D hit = new RaycastHit2D();// = MakeRaycast(Color.black, toFindStreet, shootDistance);




















            yield return new WaitForSecondsRealtime(0.5f);
            bezpiecznik++;
            if (bezpiecznik > 10)
                //mamGraczaWZasiegu = false;
            //UnityEngine.Debug.Log("Cały czas szukam target !!!");

            //Radar szuka najbliższego obiektu dlatego kiedy zrobi pełne kółko to
            //jego zasięg się zwiększy bo nic nie znalazł...
            StartCoroutine(ChangeRadius()); 

            //... albo zakończy akcję jeśli coś znajdzie)
            if (hit.collider != null)
            {
                if (hit.collider.tag == "RoadPoint")
                {
                   // UnityEngine.Debug.Log("Znalazłem target !!!");
                    //~
                    tempTargetRoad = target;
                    target = hit.collider.gameObject;
                    //~
                    break;
                }
            }

        }
        szukamTargetu = false;
    }

    IEnumerator ChangeRadius()
    {
        int i = 0;
        while(true)
        {
            //UnityEngine.Debug.Log("Zmieniam zasięg radaru !!!");
            yield return new WaitForSecondsRealtime(1);
            if(i < 4)
                shootDistance += 2;
            if (i > 20)
                break;
            if (szukamTargetu)
                break;
            i++;
        }
    }

    public bool tylkoRazUstawTarget = false;
    
    
    //If BOT collision rock, home itp.
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (movement == true)
        {
            if (coll.gameObject.tag == TagManager.GetTag(Tag.StaticGameObject)
                || coll.gameObject.tag == TagManager.GetTag(Tag.RemotePlayerBody)
                || coll.gameObject.tag == TagManager.GetTag(Tag.LocalPlayerBody))
            {
                SetTargetPosition();
                StartCoroutine(ChangeSpeed());
            }

            if (coll.gameObject.tag == TagManager.GetTag(Tag.Bot))
            {   
                StartCoroutine(ChangeSpeed());
            }
        }
    }
    //If BOT collision he's target
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == TagManager.GetTag(Tag.RoadPoint))
        {
            //UnityEngine.Debug.Log("<color=blue>BOT</color>: idę za Kolejnym punktem ścieżki");
            RoadPoint roadPoint = coll.gameObject.GetComponent<RoadPoint>();
            roadPoint.ResetCollider();
            int bezpiecznik = 0;
            while (true)
            {
                int index = Random.Range(0, roadPoint.punktyDrogiObok.Length);
                if (roadPoint.punktyDrogiObok[index] != tempTargetRoad)
                {
                    tempTargetRoad = target;
                    target = roadPoint.punktyDrogiObok[index];
                    break;
                }
                else
                {
                    bezpiecznik++;
                    if(bezpiecznik > 10)
                    {
                        //UnityEngine.Debug.Log("<color=blue>BOT</color>: koniec ścieżki");
                        tempTargetRoad = target;
                        target = roadPoint.punktyDrogiObok[index];
                        break;
                    }
                }
            }
        }
    }

    void TankFreez()
    {
        move = false;
        tempMoveSpeed = 0f;
        tempTurnSpeed = 0f;
        for (int i=0; i<botHead.Length; i++)
        {
            botHead[i].GetComponent<BOTTurret>().enabled = false;
            botHead[i].GetComponent<BotShot>().enabled = false;
            botHead[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        myPV.RPC("ZniszczBotaRPC", PhotonTargets.All, null);
    }

    void TankUnFreez()
    {
        move = true;
        tempTurnSpeed = TurnSpeed;
        tempMoveSpeed = MoveSpeed;
        for (int i = 0; i < botHead.Length; i++)
        {
            botHead[i].GetComponent<BOTTurret>().enabled = true;
            botHead[i].GetComponent<BotShot>().enabled = true;
            botHead[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
        SetPosition();
        SetTargetPosition();
        myPV.RPC("NaprawBotaRPC", PhotonTargets.All, botHealt.maxHealtPoint);
    }

    [PunRPC]
    void ZniszczBotaRPC()
    {
        stan.SetActive(false);
        Instantiate(tankExplosion, transform.position, transform.rotation);
        for (int i = 0; i < botHead.Length; i++)
        {
            botHead[i].GetComponent<SpriteRenderer>().material = deathMatereial;
            GetComponent<SpriteRenderer>().material = deathMatereial;
        }
    }

    [PunRPC]
    void NaprawBotaRPC(float realHP)
    {
        stan.SetActive(true);
        botHealt.healtPoint = realHP;
        botHealt.SyncHP(realHP);
        for (int i = 0; i < botHead.Length; i++)
        {
            botHead[i].GetComponent<SpriteRenderer>().material = defaultMaterial;
            GetComponent<SpriteRenderer>().material = defaultMaterial;
        }
    }

    IEnumerator OdliczanieDoRespawnu()
    {
        canDeath = false;
        TankFreez();

        yield return new WaitForSecondsRealtime(respawnTime);

        SetPosition();
        TankUnFreez();
        canDeath = true;
    }


    IEnumerator ChangeSpeed ()
    {
        movement = false;
        //UnityEngine.Debug.Log("<color=blue>BOT</color>: próbuję ominąć przeszkodę");
        float time = 0f;

        for (int i=0;i<=2 ;i++)
        {
            yield return new WaitForSecondsRealtime(time);

            if (i == 0)
            {
                tempMoveSpeed = 0;
                time = 1f;
            }
            if (i == 1)
            {
                //if (radar.TargetPrzedemna)
                    tempMoveSpeed = MoveSpeed * -1;
                //else
                    //tempTurnSpeed = tempTurnSpeed * -1;
                time = 2f;
            }
            if (i == 2)
            {
                tempMoveSpeed = MoveSpeed;
                tempTurnSpeed = TurnSpeed;
            }   
        }

       // UnityEngine.Debug.Log("<color=blue>BOT</color>: kończę ominąć przeszkodę");
        movement = true;
    }
}
