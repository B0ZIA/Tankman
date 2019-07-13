using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ###################################
 * #        by Jakub Główczyk        #
 * #            [#][#][ ]            #
 * ###################################
 */


/// <summary>
/// Zawiera informacje o czołgu
/// </summary>
[CreateAssetMenu(fileName = "New Tank", menuName = "Tank/NormalTank")]
public class TankData : ScriptableObject
{
    [Header("Textures:")]
    public Sprite hullTexture;
    public Sprite turretTexture;
    public Sprite turretCapTexture;

    [Header("Values:")]
    public DostempneCzolgi tank = DostempneCzolgi.PZI;
    public int level = 1;
    public int maxAmmo = 8;
    public float maxHp = 320f;
    public float damage = 12f;
    public float damageLotery = 1f;
    public float headTurnSpeed = 60f;
    public float turnSpeed = 40f;
    public float speed = 1.3f;
    //public string RW;
    public float reload = 4.5f;
    public float reloadBetweenMagazine = 1f;
    public float cameraSize = 3.5f;

    [Header("Collider Box Value:")]
    public Vector2 offset = new Vector2 (0.01f,-0.01f);
    public Vector2 size = new Vector2(3.18f, 1.74f);

    [Header("Object positions:")]
    public Vector2 turretPos = new Vector2(-0.345f, 0.375f);
    public Vector2 turretCapPos = new Vector2(0f, 0f);
    public Vector2 barrlelEndPos = new Vector2(-0.87f, 0.14f);


    [Header("Others:")]
    public bool iHaveTurretCap = false;
    public bool turretCan360 = true;
    public string description;
    public TowerType[] otherTurrets;

    public void SetDescriptionOnHUD()
    {
        HUDManager.OnEnterButton(description);
    }

    public int curretTab;
    public int curretTabTier;
    public string tempDescription;
    public string tempDescription2;
    public bool czyPosiadamLufe;
    public bool czyPosiadamPrzykrycieLufy;
}