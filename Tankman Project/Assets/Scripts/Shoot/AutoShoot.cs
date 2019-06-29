using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShoot : Shoot, IAmRemoteShoot
{
    public AutomaticTurret.TowerType rodzajWiezy;
    public ParticleSystem muzzleFlash;
    public GameOver tankStore;

    //Z interfejsu
    public bool check { get; set; }
    public bool allow { get; set; }
    public bool trafie { get; set; }

    public override int MaxAmmo
    {
        get { return maxAmmo; }
        set { return; }
    }

    public override float ReloadTime
    {
        get { return reloadTime; }
        set { return; }
    }

    public override float ReloadMagazieTime
    {
        get { return reloadMagazieTime; }
        set { return; }
    }

    public override float Damage
    {
        get { return damage; }
        set { return; }
    }

    public override float DamageLotery
    {
        get { return damageLotery; }
        set { return; }
    }

    void Start()
    {
        check = true;
        allow = true;

        Set();
    }

    public void Set()
    {
        Reset();

        switch (rodzajWiezy)
        {
            case AutomaticTurret.TowerType.O_ITopLeft:
                maxAmmo = 7;
                reloadTime = 6;
                reloadMagazieTime = 0.5f;
                damage = 12f;
                damageLotery = 0;
                break;
            case AutomaticTurret.TowerType.O_ITopRight:
                maxAmmo = 5;
                reloadTime = 5;
                reloadMagazieTime = 0.75f;
                damage = 20f;
                damageLotery = 0;
                break;
            case AutomaticTurret.TowerType.O_IButton:
                maxAmmo = 25;
                reloadTime = 9f;
                reloadMagazieTime = 0.3f;
                damage = 5f;
                damageLotery = 0;
                break;
            case AutomaticTurret.TowerType.IS7OnHead:
                maxAmmo = 10;
                reloadTime = 9;
                reloadMagazieTime = 0.5f;
                damage = 17f;
                damageLotery = 0;
                break;
            case AutomaticTurret.TowerType.PZI:
                maxAmmo = 8;
                reloadTime = 4.5f;
                reloadMagazieTime = 0.5f;
                damage = 12f;
                damageLotery = 0;
                break;
            case AutomaticTurret.TowerType.PZVI:
                maxAmmo = 1;
                reloadTime = 7;
                reloadMagazieTime = 0.5f;
                damage = 215f;
                damageLotery = 0;
                break;
            default:
                break;
        }
    }

    public void Reset()
    {
        tempMaxAmmo = maxAmmo;
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
