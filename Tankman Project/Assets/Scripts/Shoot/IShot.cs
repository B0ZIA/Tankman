using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShot
{
    int MaxAmmo { get; } 
    float ReloadTime { get; }
    float ReloadMagazineTime { get; }
    float Damage { get; }
    float MaxDamageDisparity { get; }


    void Shoot();
    void CheckShoot();
    IEnumerator Reload();
    IEnumerator ReloadEffect();
}
