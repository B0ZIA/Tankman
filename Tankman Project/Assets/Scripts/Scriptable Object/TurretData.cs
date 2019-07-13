using UnityEngine;

[CreateAssetMenu(fileName = "New Turret", menuName = "Tank/Turret")]
public class TurretData : ScriptableObject
{
    public TowerType towerType;
    public int maxAmmo = 7;
    public float reloadTime = 6;
    public float reloadMagazieTime = 0.5f;
    public float damage = 12f;
    public float damageLotery = 0;
}

public enum TowerType
{
    O_ITopLeft,
    O_ITopRight,
    O_IButton,
    IS7OnHead,
    //PZI,
    //PZVI
}
