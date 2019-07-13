using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShoot : Shoot, IAmRemoteShoot
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

    public override float ReloadMagazieTime
    {
        get { return turret.reloadMagazieTime; }
    }

    public override float Damage
    {
        get { return turret.damage; }
    }

    public override float DamageLotery
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
        tempMaxAmmo = MaxAmmo;
        StartCoroutine(CheckReload());
        timeToFire = 0;
        isReloadnig = false;
        allow = true;
    }

    void Update()
    {
        CheckShooting();
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

    public override void Shooting()
    {
        allow = false;

        base.Shooting();

        RaycastHit2D hit = MakeRaycastHit2D();

        if (hit.collider != null)
        {
            float tempDamage = Mathf.Round(Random.Range(Damage - DamageLotery, Damage + DamageLotery));

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

    public override void CheckShooting()
    {
        //Debug.Log("<color=red>1</color>");
        base.CheckShooting();
        if (!ICanShoot)
            return;
        //Debug.Log("<color=yellow>2</color>");

        if (trafie && Time.time >= timeToFire)
        {
            //Debug.Log("<color=blue>3</color>");
            trafie = false;
            timeToFire = Time.time + ReloadMagazieTime / 8;
            Shooting();
        }
    }

    [PunRPC]
    protected override void RpcDoShootEffect(Vector3 pos, Quaternion rot, PhotonMessageInfo pmi)
    {
        BulletTrailPrefab.GetComponent<BulletMovment>().own = Player.FindPlayer(pmi.sender).gameObject.GetComponent<TankEvolution>().HullGameObject;
        Instantiate(BulletTrailPrefab, pos, rot);
    }
}
