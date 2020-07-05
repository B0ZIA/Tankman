using UnityEngine;
using System.Collections;
using UnityEngine.UI;
    
/// <summary>
/// Sctipt responsoible for shoot player. Don't turn off this script in PlayerSetup
/// </summary>
public class TankShot : Shot, IShot
{
    public static TankShot Instance { get; private set; }

    [Header("'TankShot' Reference")]
    [Space]
    [SerializeField]
    protected GameOver playerGameOver;
    [SerializeField]
    protected ParticleSystem muzzleFlash;
    [SerializeField]
    protected TechTree techTree;
    [SerializeField]
    protected Text reloadText;

    private bool shoot = true;

    public override int MaxAmmo
    {
        get { return TankEvolution.Instance.Magazynek; }
    }

    public override float ReloadTime
    {
        get { return TankEvolution.Instance.Reload; }
    }

    public override float ReloadMagazineTime
    {
        get { return TankEvolution.Instance.ReloadBetweenMagazine; }
    }

    public override float Damage
    {
        get { return TankEvolution.Instance.Damage; }
    }

    public override float MaxDamageDisparity
    {
        get { return TankEvolution.Instance.DamageLotery; }
    }

    public void Awake()
    {
        if(!photonView.isMine)
            enabled = false;
        else if (Instance == null)
            Instance = this;
        else
            enabled = false;
    }

    void Update ()
	{
        CheckShoot();
	}

    public void SetShootingOpportunity(bool decision)
    {
        StartCoroutine(OdblokujStrzelanie(decision,0));
    }

    public void SetShootingOpportunity(bool decision, float time)
    {
        StartCoroutine(OdblokujStrzelanie(decision, time));
    }

    IEnumerator OdblokujStrzelanie(bool decision, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        shoot = decision;
    }

    public void ResetMagazine(int ammo)
    {
        currentAmmo = ammo;
    }


    public override void Shoot()
    {
        muzzleFlash.Play();
        //~~~~
        base.Shoot();
        //~~~~
        RaycastHit2D hit = MakeRaycastHit2D();

        if (hit.collider != null)
        {
            float tempDamage = Mathf.Round(Random.Range(Damage - MaxDamageDisparity, Damage + MaxDamageDisparity));

            HitPlayerHowPlayer(hit, tempDamage);

            HitBotHowPlayer(hit, tempDamage);
        }
    }

    public override void CheckShoot()
    {
        if (realReloadTime <= 0f)
        {
            realReloadTime = 0f;
            reloadText.text = "0.0";
            reloadText.color = Color.red;
        }
        else
        {
            reloadText.text = realReloadTime.ToString();
            reloadText.color = Color.white;
        }

        //~~~~
        base.CheckShoot();
        if (!ICanShoot)
            return;
        //~~~~

        if (Input.GetButton("Fire1") && Time.time >= timeToFire)
        {
            timeToFire = Time.time + ReloadMagazineTime / 8;

            if (!shoot)
                return;

            Shoot();
        }
    }

    [PunRPC]
    protected override void RpcDoShootEffect(Vector3 pos, Quaternion rot, PhotonMessageInfo pmi)
    {
        if(Player.FindPlayer(pmi.sender).gameObject != null)
        BulletTrailPrefab.GetComponent<BulletMovment>().own = 
            Player.FindPlayer(pmi.sender).gameObject.GetComponent<TankEvolution>().HullGameObject;
        Instantiate(BulletTrailPrefab, pos, rot);
    }
}

