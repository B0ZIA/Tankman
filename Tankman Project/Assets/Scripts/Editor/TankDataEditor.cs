using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// No to jest do okna edycji plików czołgów tj. TankData
/// </summary>
[CustomEditor(typeof(TankData))] [SerializeField]
public class TankDataEditor : Editor
{
    Rect icon = new Rect(Screen.width - 400, 30, 384, 192);
    Rect iconText = new Rect(10, 100, Screen.width, Screen.height);
    GUISkin skin;


    void OnEnable()
    {
        skin = Resources.Load<GUISkin>("skin");
    }

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        //return;

        TankData myTarget = (TankData)target;
        //mam własny edytor więc muszę w ten sposób zapisywać zmiany. Po wprowadzeniu zmian
        // należy zapisać cały projekt CTRL+S lub File/SaveProject
        EditorUtility.SetDirty(myTarget);

        DrawIcon(myTarget);

        myTarget.curretTab = GUILayout.Toolbar(myTarget.curretTab, new string[] { "Niszczyciel", "Zwykły" });

        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Czołg:", EditorStyles.boldLabel);
        myTarget.tank = (Tanks)EditorGUILayout.EnumPopup(myTarget.tank);
        EditorGUILayout.EndHorizontal();


        DrawDefaultTexture(myTarget);

        if (myTarget.curretTab == 0)    //Niszczyciel czołgów
        {
            myTarget.turretCan360 = false;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Tekstura przykrycia wieży:");
            myTarget.czyPosiadamPrzykrycieLufy = EditorGUILayout.Toggle(myTarget.czyPosiadamPrzykrycieLufy);
            if(myTarget.czyPosiadamPrzykrycieLufy)
                myTarget.turretCapTexture = (Sprite)EditorGUILayout.ObjectField(myTarget.turretCapTexture, typeof(Sprite), true);
            else
                myTarget.turretCapTexture = null;
            if (myTarget.turretCapTexture == null) myTarget.iHaveTurretCap = false; else myTarget.iHaveTurretCap = true;
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            myTarget.turretCan360 = true;
        }

        DrawDefaultParametrs(myTarget);

        DrawColliderSet(myTarget);

        DrawDefaultObjectPositon(myTarget);

        if (myTarget.curretTab == 0)    //Niszczyciel czołgów
        {
            myTarget.turretCapPos = EditorGUILayout.Vector2Field("przykrycie lufy:", myTarget.turretCapPos);
        }
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("*Należy postępować analogicznie do Collider aby ustawić te parametry", skin.GetStyle("tankDescription"));
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.Label("Opis:", EditorStyles.boldLabel);
        myTarget.tempDescription = "<color=red>" + myTarget.name + "- </color>";
        GUILayout.Label(myTarget.tempDescription+"...");
        myTarget.description = myTarget.tempDescription + myTarget.tempDescription2;
        myTarget.tempDescription2 = EditorGUILayout.TextField(myTarget.tempDescription2);   
        GUILayout.Label("*Jest to opis głównie wykorzystywany w drzewku technoligii po najechaniu na czołg" +
            ". Nie trzeba podawać nazwy czołgu specjalnie do tego opisu bo ona zostanie wygenerowana automatycznie" +
            " więc... pisz od razu opis ;)", skin.GetStyle("tankDescription"));

        if(myTarget.tank == Tanks.O_I)
        {
            myTarget.otherTurrets = new TowerType[]
            {
                TowerType.O_IButton,
                TowerType.O_ITopLeft,
                TowerType.O_ITopRight
            };
        }
        if (myTarget.tank == Tanks.IS7)
        {
            myTarget.otherTurrets = new TowerType[]
            {
                TowerType.IS7OnHead
            };
        }
    }





    void DrawDefaultTexture(TankData myTarget)
    {
        GUILayout.Space(10);
        GUILayout.Label("Textury:", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Tekstura kadłuba czołgu:");
        EditorGUILayout.Toggle(true);
        myTarget.hullTexture = (Sprite)EditorGUILayout.ObjectField(myTarget.hullTexture, typeof(Sprite), true);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Tekstura wieży czołgu:");
        myTarget.czyPosiadamLufe = EditorGUILayout.Toggle(myTarget.czyPosiadamLufe);
        if (myTarget.czyPosiadamLufe)
            myTarget.turretTexture = (Sprite)EditorGUILayout.ObjectField(myTarget.turretTexture, typeof(Sprite), true);
        else
            myTarget.turretTexture = null;
        EditorGUILayout.EndHorizontal();
    }

    void DrawDefaultParametrs(TankData myTarget)
    {
        GUILayout.Space(10);
        GUILayout.Label("Parametry:", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Tier:");
        myTarget.curretTabTier = GUILayout.Toolbar(myTarget.curretTabTier, new string[] { "Pierwszy", "Drugi","Trzeci","Czwarkty" });
        myTarget.level = myTarget.curretTabTier + 1;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        myTarget.maxHp = EditorGUILayout.FloatField("Punkty zdrowia:", myTarget.maxHp);
        myTarget.damage = EditorGUILayout.FloatField("Obrażenia:", myTarget.damage);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        myTarget.damageLotery = EditorGUILayout.FloatField("Wachania obrażeń:", myTarget.damageLotery);
        myTarget.maxAmmo = EditorGUILayout.IntField("Posicki w magazynku:", myTarget.maxAmmo);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        myTarget.headTurnSpeed = EditorGUILayout.FloatField("Szybkość obrotu wieży:", myTarget.headTurnSpeed);
        myTarget.turnSpeed = EditorGUILayout.FloatField("Szybkość obrotu kadłuba:", myTarget.turnSpeed);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        myTarget.speed = EditorGUILayout.FloatField("Szybkość poruszania się:", myTarget.speed);
        myTarget.reload = EditorGUILayout.FloatField("Czas ładowania magazynku:", myTarget.reload);
        EditorGUILayout.EndHorizontal();

        if(myTarget.maxAmmo > 1)
        {
            EditorGUILayout.BeginHorizontal();
            myTarget.reloadBetweenMagazine = EditorGUILayout.FloatField("Czas wkładania kolejnego pocisku:", myTarget.reloadBetweenMagazine);
            GUILayout.Label("*jeśli masz więcej niż 1 pocisk w magazynku",skin.GetStyle("tankDescription"));
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
        }
        //myTarget.cameraSize = EditorGUILayout.FloatField("Widkok kamery:", myTarget.cameraSize);
    }

    void DrawColliderSet(TankData myTarget)
    {
        GUILayout.Space(10);
        GUILayout.Label("Collider:", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Offset:");
        GUILayout.Space(10);
        GUILayout.Label("X");
        myTarget.offset.x = EditorGUILayout.FloatField(myTarget.offset.x);
        GUILayout.Label("Y");
        myTarget.offset.y = EditorGUILayout.FloatField(myTarget.offset.y);

        GUILayout.Label("Size:");
        GUILayout.Space(10);
        GUILayout.Label("X");
        myTarget.size.x = EditorGUILayout.FloatField(myTarget.size.x);
        GUILayout.Label("Y");
        myTarget.size.y = EditorGUILayout.FloatField(myTarget.size.y);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("*Należy 'wydobyć' te parametry przeciągając prefab gracza na scene, (najlepiej)podpinając mu ręcznie textury" +
            " i wchodząc odpowiednio w chierarchii w Player/Tank/Hull ustawić ręcznie parametry BoxColider2D", skin.GetStyle("tankDescription"));
        EditorGUILayout.EndHorizontal();

    }

    void DrawDefaultObjectPositon(TankData myTarget)
    {
        GUILayout.Space(10);
        GUILayout.Label("Pozycje poszczególnych obiektów:", EditorStyles.boldLabel);

        myTarget.turretPos = EditorGUILayout.Vector2Field("Wieża:", myTarget.turretPos);
        myTarget.barrlelEndPos = EditorGUILayout.Vector2Field("Wylot lufy:", myTarget.barrlelEndPos);

    }

    void DrawIcon(TankData myTarget)
    {
        //iconText.x = 10;
        //iconText.y = 100;
        //iconText.width = Screen.width;
        //iconText.height = Screen.height;
        icon = new Rect(Screen.width - 400, 30, 384, 192);
        iconText = new Rect(10, 100, Screen.width, Screen.height);

        GUILayout.BeginArea(iconText);
        GUILayout.Label(myTarget.tank.ToString(),skin.GetStyle("tank"));
        GUILayout.EndArea();

        //icon.x = Screen.width - 400;
        //icon.y = 30;
        //icon.width = 384;
        //icon.height = 192;   

        GUILayout.Space(160);
        if (myTarget.hullTexture != null)
            GUI.DrawTexture(icon, myTarget.hullTexture.texture);
        if (myTarget.turretTexture != null)
            GUI.DrawTexture(icon, myTarget.turretTexture.texture);
        if(myTarget.turretCapTexture != null)
            GUI.DrawTexture(icon, myTarget.turretCapTexture.texture);
    }
}
