using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShot : Shot, IAmRemoteShoot
{
    public TurretData turret;
    public ParticleSystem muzzleFlash;
    public GameOver tankStore;

    //Z interfejsu
    public bool check { get; set; }
    public bool allow { get; set; }
    public bool trafie { get; set; }

    public override int MaxAmmo
    {
        get { return turret.maxAmmo; }
    }

    public override float ReloadTime
    {
        get { return turret.reloadTime; }
    }

    public override float ReloadMagazineTime
    {
        get { return turret.reloadMagazieTime; }
    }

    public override float Damage
    {
        get { return turret.damage; }
    }

    public override float MaxDamageDisparity
    {
        get { return turret.damageLotery; }
    }

    void Start()
    {
        check = true;
        allow = true;

        Reset();
    }

    public void Reset()
    {
        currentAmmo = MaxAmmo;
        StartCoroutine(CheckReload());
        timeToFire = 0;
        isReloadnig = false;
        allow = true;
    }

    void Update()
    {
        CheckShoot();
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

    public override void Shoot()
    {
        allow = false;

        base.Shoot();

        RaycastHit2D hit = MakeRaycastHit2D();

        if (hit.collider != null)
        {
            float tempDamage = Mathf.Round(Random.Range(Damage - MaxDamageDisparity, Damage + MaxDamageDisparity));

            HitPlayerHowAutoTurretPlayer(hit, tempDamage);

            //HitBotHowAutoPlayer(hit, tempDamage, tankStore.GetComponent<PlayerGO>().myPlayer);
            HitBotHowPlayer(hit, tempDamage);
        }
        allow = true;
    }


    public virtual void Check()
    {
        RaycastHit2D hit = MakeRaycastHit2D();

        if (hit.collider != null)
        {
            if (hit.collider.tag == Tag.BOT || hit.collider.tag == Tag.REMOTEPLAYERBODY)
            {
                if (allow == true)
                {
                    trafie = true;
                }
            }
        }
    }

    public override void CheckShoot()
    {
        //Debug.Log("<color=red>1</color>");
        base.CheckShoot();
        if (!ICanShoot)
            return;
        //Debug.Log("<color=yellow>2</color>");

        if (trafie && Time.time >= timeToFire)
        {
            //Debug.Log("<color=blue>3</color>");
            trafie = false;
            timeToFire = Time.time + ReloadMagazineTime / 8;
            Shoot();
        }
    }

    [PunRPC]
    protected override void RpcDoShootEffect(Vector3 pos, Quaternion rot, PhotonMessageInfo pmi)
    {
        BulletTrailPrefab.GetComponent<BulletMovment>().own = Player.FindPlayer(pmi.sender).gameObject.GetComponent<TankEvolution>().HullGameObject;
        Instantiate(BulletTrailPrefab, pos, rot);
    }
}
