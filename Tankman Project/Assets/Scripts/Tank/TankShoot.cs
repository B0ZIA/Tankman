using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * ###################################
 * #        by Jakub Główczyk        #
 * #            [#][#][ ]            #
 * ###################################
 */

/// <summary>
/// Sctipt responsoible for shoot player. Don't turn off this script in PlayerSetup
/// </summary>
public class TankShoot : Shoot
{
    public static TankShoot Instance { get; private set; }

    [Header("'TankShoot' Reference")]
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
        set { return; }
    }

    public override float ReloadTime
    {
        get { return TankEvolution.Instance.Reload; }
        set { return; }
    }

    public override float ReloadMagazieTime
    {
        get { return TankEvolution.Instance.ReloadBetweenMagazine; }
        set { return; }
    }

    public override float Damage
    {
        get { return TankEvolution.Instance.Damage; }
        set { return; }
    }

    public override float DamageLotery
    {
        get { return TankEvolution.Instance.DamageLotery; }
        set { return; }
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
        CheckShooting();
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
        tempMaxAmmo = ammo;
    }


    public override void Shooting()
    {
        muzzleFlash.Play();
        //~~~~
        base.Shooting();
        //~~~~
        RaycastHit2D hit = MakeRaycastHit2D();

        if (hit.collider != null)
        {
            float tempDamage = Mathf.Round(Random.Range(Damage - DamageLotery, Damage + DamageLotery));

            HitPlayerHowPlayer(hit, tempDamage);

            HitBotHowPlayer(hit, tempDamage);
        }
    }

    public override void CheckShooting()
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
        base.CheckShooting();
        if (!ICanShoot)
            return;
        //~~~~

        if (Input.GetButton("Fire1") && Time.time >= timeToFire)
        {
            timeToFire = Time.time + ReloadMagazieTime / 8;

            if (!shoot)
                return;

            Shooting();
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

