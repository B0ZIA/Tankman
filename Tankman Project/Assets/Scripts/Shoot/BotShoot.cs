using UnityEngine;
using System.Collections;

public class BotShoot : Shoot, IAmRemoteShoot
{
    //Z interfejsu
    public bool check { get; set; }
    public bool allow { get; set; }
    public bool trafie { get; set; }

    [SerializeField]
    private BOTSetup botSetup;

    public override int MaxAmmo { get { return botSetup.MyTank.maxAmmo; }  }

    public override float ReloadTime { get { return botSetup.MyTank.reload; }  }

    public override float ReloadMagazieTime { get { return botSetup.MyTank.reloadBetweenMagazine; }  }

    public override float Damage { get { return botSetup.MyTank.damage; }  }

    public override float DamageLotery { get { return botSetup.MyTank.damageLotery; }  }

    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        tempMaxAmmo = MaxAmmo;
        StartCoroutine(CheckReload());
        timeToFire = 0;
        isReloadnig = false;
        check = true;
        allow = true;
        ICanShoot = true;
    }

    void Update()
    {
        CheckShooting();
    }

    public override void Shooting()
    {
        allow = false;

        base.Shooting();

        RaycastHit2D hit = MakeRaycastHit2D();

        if (hit.collider != null)
        {
            float tempDamage = Mathf.Round(Random.Range(Damage - DamageLotery, Damage + DamageLotery));

            HitPlayerHowBot(hit, tempDamage);

        }
        allow = true;
    }


    public void Check()
    {
        RaycastHit2D hit = MakeRaycastHit2D();

        if (hit.collider != null)
        {
            if (hit.collider.tag == Tag.LOCALPLAYERBODY || hit.collider.tag == Tag.REMOTEPLAYERBODY)
            {
                if (allow == true)
                {
                    trafie = true;
                }
            }
        }
    }

    [PunRPC]
    protected override void RpcDoShootEffect(Vector3 pos, Quaternion rot, PhotonMessageInfo pmi)
    {
        BulletTrailPrefab.GetComponent<BulletMovment>().own = GetComponent<BOTTurret>().botMovement.gameObject;
        Instantiate(BulletTrailPrefab, pos, rot);
    }

    public IEnumerator CheckReload()
    {
        while (true)
        {
            check = false;
            yield return new WaitForSecondsRealtime(0.1f);
            Check();
            check = true;
        }
    }

    public override void CheckShooting()
    {
        base.CheckShooting();
        if (!ICanShoot)
            return;

        if (trafie && Time.time >= timeToFire)
        {
            trafie = false;
            timeToFire = Time.time + ReloadMagazieTime / 8;
            Shooting();
        }
    }
}
