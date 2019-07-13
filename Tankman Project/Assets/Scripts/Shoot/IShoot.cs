using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShoot
{
    int MaxAmmo { get; } 
    int TempMaxAmmo { get; }
    float ReloadTime { get; }
    float ReloadMagazieTime { get; }
    float Damage { get; }
    float DamageLotery { get; }


    void Shooting();
    void CheckShooting();
    IEnumerator Reload();
    IEnumerator ReloadEffect();
}
