using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShoot
{
    //Mogłem użyć interfersu do deklaracji własności
    //ale chciałem poćwiczyć używanie klas abstrakcyjnych :P

    //int MaxAmmo { get; set; }
    //int TempMaxAmmo { get; }
    //float ReloadTime { get; set; }
    //float ReloadMagazieTime { get; set; }
    //float Damage { get; set; }
    //float DamageLotery { get; set; }


    void Shooting();
    void CheckShooting();
    IEnumerator Reload();
    IEnumerator ReloadEffect();
}
