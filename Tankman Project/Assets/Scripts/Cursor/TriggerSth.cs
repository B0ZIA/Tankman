using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerSth : MonoBehaviour
{
    //Zawsze aktualne listy obiektów które są blisko gracza
    public List<AntiTankBarrier> tempObiektDoZniszczenia = new List<AntiTankBarrier>();
    public List<AntiTankBarrier> tempObiektDoNaprawy = new List<AntiTankBarrier>();

    //(Nie)Pozwala włączyć pola edycji obiektów do wysadzenia/Naprawienia
    public bool ustawJakoDoZniszczenia = false;
    public bool ustawJakoDoNaprawy = false;

    //dla kursora
    public Texture2D cursorTextureDynamit;
    public Texture2D cursorTextureNaprawka;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = new Vector3(10f,10f);

    //Dla metod wywoływanych przez przyciski
    private bool przelacznikDlaNaprawiarka = true;
    private bool przelacznikDlaDynamit = true;
    private bool przelacznikDlaZasoby = true;
    private bool przelacznikDlaZebySmoka = true;

    bool wykonajSieRaz2 = true;
    bool wykonajSieRaz = true;
    public GameObject wstawiaczZebowSmoka;

    public Sprite zasiek;
    public Sprite zebySmoka;
    public Sprite stalowyX;




    void Update()
    {
        SetHUD();
        SetObjectsLists();
    }

    public void SetHUD()
    {
        if (HUDManager.Instance.playerGO.myPlayer.Dynamit > 0)   //Jeśli mam coś dynamitów...
            SprawdzCzySaDoZniszczenia();    //To zobacze czy są w pobliżu obiekty do wysadzenia
        else
            //Bo jeśli nie to nie ma co się bawić w wysadzanie  
            HUDManager.Instance.dynamit.GetComponent<Button>().interactable = false;


        if (HUDManager.Instance.playerGO.myPlayer.Naprawiarka > 0)   //Jeśli mam coś naprawiarek...
            SprawdzCzySaDoNaprawy();    //To zobacze czy są w pobliżu obiekty do naprawienia
        else
            //Bo jeśli nie to nie ma co się bawić w naprawianie  
            HUDManager.Instance.naprawiarka.GetComponent<Button>().interactable = false;

        if (HUDManager.Instance.playerGO.myPlayer.Zasoby <= 0)
        {
            HUDManager.Instance.zasoby.GetComponent<Button>().interactable = false;
            HUDManager.Instance.zasobyMenu.SetActive(false);
        }
        else
            HUDManager.Instance.zasoby.GetComponent<Button>().interactable = true;


        if (TankShoot.Instance.GetComponent<PlayerGO>().myPlayer.Naprawiarka <= 0)
            ustawJakoDoNaprawy = false;          //Niepozwala włączyć pola edycji obiektów do naprawy

        if (TankShoot.Instance.GetComponent<PlayerGO>().myPlayer.Dynamit <= 0)
            ustawJakoDoZniszczenia = false;     //Niepozwala włączyć pola edycji obiektów do wysadzenia
    }

    public void SetObjectsLists()
    {
        if (tempObiektDoZniszczenia.Count != 0)
        {
            //(nie)aktywuje pola w pobliżu do wysadzenia
            if (ustawJakoDoZniszczenia)
            {
                foreach (AntiTankBarrier item in tempObiektDoZniszczenia)
                    item.poleDoNiszczenia.SetActive(true);
            }
            else
            {
                foreach (AntiTankBarrier item in tempObiektDoZniszczenia)
                    item.poleDoNiszczenia.SetActive(false);
            }
        }

        if (tempObiektDoNaprawy.Count != 0)
        {
            //(nie)aktywuje pola w pobliżu do naprawy
            if (ustawJakoDoNaprawy)
            {
                foreach (AntiTankBarrier item in tempObiektDoNaprawy)
                    item.poleDoNaprawy.SetActive(true);
            }
            else
            {
                foreach (AntiTankBarrier item in tempObiektDoNaprawy)
                    item.poleDoNaprawy.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Resetuje listy obiektów do naprawy i zniszczenia w ułamku sekundy
    /// </summary>
    /// <returns></returns>
    public IEnumerator ResetColliera()
    {
        GetComponent<CircleCollider2D>().radius = 0.1f;
        tempObiektDoZniszczenia.Clear();
        tempObiektDoNaprawy.Clear();
        yield return new WaitForSecondsRealtime(0.1f);
        GetComponent<CircleCollider2D>().radius = 3.7f;
    }

    //(Nie)Aktywuje możliwość wysadzania elementów terenu
    void SprawdzCzySaDoZniszczenia()
    {
        if (tempObiektDoZniszczenia.Count > 0)
        {
            HUDManager.Instance.dynamitTlo.SetActive(true);
            HUDManager.Instance.dynamit.GetComponent<Button>().interactable = true;
            wykonajSieRaz = true;
        }
        else
        {
            if (wykonajSieRaz)
            {
                DezaktywujWysadzanie();
                przelacznikDlaDynamit = true;
                HUDManager.Instance.dynamit.GetComponent<Button>().interactable = false;
                HUDManager.Instance.dynamitTlo.SetActive(false);
                wykonajSieRaz = false;
            }
        }
    }

    //(Nie)Aktywuje możliwość naprawiania elementów terenu
    void SprawdzCzySaDoNaprawy()
    {
        if (tempObiektDoNaprawy.Count > 0)
        {
            HUDManager.Instance.naprawiarkaTlo.SetActive(true);
            HUDManager.Instance.naprawiarka.GetComponent<Button>().interactable = true;
            wykonajSieRaz2 = true;
        }
        else
        {
            if(wykonajSieRaz2)
            {
                DezaktywujNaprawianie();
                przelacznikDlaNaprawiarka = true;
                HUDManager.Instance.naprawiarka.GetComponent<Button>().interactable = false;
                HUDManager.Instance.naprawiarkaTlo.SetActive(false);
                wykonajSieRaz2 = false;
            }
        }
    }

    //Pszeszukuje obiekty z bliskiego otoczenia i dodaje je
    // do ObiektyDoZniszczenia i ObiektyDoNaprawy 
    void OnTriggerEnter2D(Collider2D collision)
    {
        SetBarrierInNearList(collision);
    }

    //Jeśli jakiś obiekt z tempObiektDoZniszczenia lub tempObiektDoNaprawy
    // był i już nie jest w otoczeniu usuwa go z właśnie z tej listy
    // i uniemożliwia naprawienie/wysadzenie go
    void OnTriggerExit2D(Collider2D collision)
    {
        SetBarrierOutNearList(collision);
    }

    public void SetBarrierInNearList(Collider2D collision)
    {
        if (collision.GetComponent<AntiTankBarrier>() == null)
            return;
        if (collision.tag == Tag.NAPRAWIONEZEBYSMOKA)
            tempObiektDoZniszczenia.Add(collision.gameObject.GetComponent<AntiTankBarrier>());
        if (collision.tag == Tag.ZNISZCZONEZEBYSMOKA)
            tempObiektDoNaprawy.Add(collision.gameObject.GetComponent<AntiTankBarrier>());
    }

    public void SetBarrierOutNearList(Collider2D collision)
    {
        if (collision.GetComponent<AntiTankBarrier>() == null)
            return;
        if (collision.tag == Tag.NAPRAWIONEZEBYSMOKA)
            tempObiektDoZniszczenia.Remove(collision.gameObject.GetComponent<AntiTankBarrier>());
        if (collision.tag == Tag.ZNISZCZONEZEBYSMOKA)
            tempObiektDoNaprawy.Remove(collision.gameObject.GetComponent<AntiTankBarrier>());

        //Wyłącza pola edycji
        if (collision.gameObject.GetComponent<AntiTankBarrier>().poleDoNaprawy != null)
            collision.gameObject.GetComponent<AntiTankBarrier>().poleDoNaprawy.SetActive(false);
        if (collision.gameObject.GetComponent<AntiTankBarrier>().poleDoNiszczenia != null)
            collision.gameObject.GetComponent<AntiTankBarrier>().poleDoNiszczenia.SetActive(false);
    }

    
    //Wykonuje się kedy kliknisz na ikonkę Naprawiarki. Aktywuje/Dezaktywuje naprawianie
    public void NaprawiarkaPrzycisk()
    {
        if (przelacznikDlaNaprawiarka) { DezaktywujWysadzanie(); AktywujNaprawianie(); przelacznikDlaDynamit = true; przelacznikDlaNaprawiarka = false; }
        else { DezaktywujNaprawianie(); przelacznikDlaNaprawiarka = true;}
    }

    //Wykonuje się kedy kliknisz na ikonkę Dynamitu. Aktywuje/Dezaktywuje wysadzanie
    public void DynamitPrzycisk()
    {
        if (przelacznikDlaDynamit) { DezaktywujNaprawianie(); AktywujWysadzanie(); przelacznikDlaNaprawiarka = true; przelacznikDlaDynamit = false; }
        else { DezaktywujWysadzanie(); przelacznikDlaDynamit = true; }
    }

    //Wykonuje się kedy kliknisz na ikonkę Dynamitu. Aktywuje/Dezaktywuje wysadzanie
    public void ZasobyPrzycisk()
    {
        if (przelacznikDlaZasoby) { HUDManager.Instance.zasobyMenu.SetActive(true); przelacznikDlaZasoby = false; }
        else { HUDManager.Instance.zasobyMenu.SetActive(false); przelacznikDlaZasoby = true; }
    }

    //Wykonuje się kedy kliknisz na ikonkę ZebówSmoka w zasobyMenu. Aktywuje/Dezaktywuje wstawianie Zębów Smoka
    public void ZebySmokaPrzycisk(int RodzajZaporyIndex)
    {
        if (przelacznikDlaZebySmoka)
        {
            Sprite mySprite;
            WskaznikManager WK = wstawiaczZebowSmoka.GetComponent<WskaznikManager>();
            switch (RodzajZaporyIndex)
            {
                case 0:
                    mySprite = zasiek;
                    WK.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.3f, 1.2f);
                    WK.zielonePole.transform.localScale = new Vector3(0.5f, 1.3f, 1f);
                    WK.czerwonePole.transform.localScale = new Vector3(0.5f, 1.3f, 1f);
                    break;
                case 1:
                    mySprite = zebySmoka;
                    WK.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.5f, 0.5f);
                    WK.zielonePole.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                    WK.czerwonePole.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                    break;
                case 2:
                    mySprite = stalowyX;
                    WK.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.5f, 0.5f);
                    WK.zielonePole.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                    WK.czerwonePole.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                    break;
                default: mySprite = zebySmoka; break;
            }

            wstawiaczZebowSmoka.GetComponent<SpriteRenderer>().sprite = mySprite;
            WK.barrierType = (AntiTankBarrier.RodzajZapory)RodzajZaporyIndex;
            Instantiate(wstawiaczZebowSmoka, transform.position, Quaternion.identity);
        }
        HUDManager.Instance.zasobyMenu.SetActive(false);
    }


    void AktywujNaprawianie()
    {
        Cursor.SetCursor(cursorTextureNaprawka, hotSpot, cursorMode);//Ustawia ikonę kursora
        GameManager.LocalPlayer.gameObject.GetComponent<TankShoot>().SetShootingOpportunity(false);
        //hudManager.tankShoot.techTree.nieStrzelajGraczu = true;    //Wyłącza strzelanie podczas Naprawiania
        ustawJakoDoNaprawy = true;           //Pozwala włączyć pola edycji obiektów do naprawy
    }

    public void DezaktywujNaprawianie()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);      //Ustawia ikonę kursora
        //hudManager.tankShoot.ZaChwilePozwoleStrzelac();
        GameManager.LocalPlayer.gameObject.GetComponent<TankShoot>().SetShootingOpportunity(true);
        //hudManager.tankShoot.techTree.nieStrzelajGraczu = false;//Zpowrotem pozwala na strzelanie
        ustawJakoDoNaprawy = false;          //Niepozwala włączyć pola edycji obiektów do naprawy
    }

    void AktywujWysadzanie()
    {
        Cursor.SetCursor(cursorTextureDynamit, hotSpot, cursorMode);//Ustawia ikonę kursora
        GameManager.LocalPlayer.gameObject.GetComponent<TankShoot>().SetShootingOpportunity(false);
        //hudManager.tankShoot.techTree.nieStrzelajGraczu = true;    //Wyłącza strzelanie podczas Naprawiania
        ustawJakoDoZniszczenia = true;       //Pozwala włączyć pola edycji obiektów do wysadzenia
    }

    public void DezaktywujWysadzanie()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);   //Ustawia ikonę kursora
        //hudManager.tankShoot.ZaChwilePozwoleStrzelac();
        GameManager.LocalPlayer.gameObject.GetComponent<TankShoot>().SetShootingOpportunity(true);
        //hudManager.tankShoot.techTree.nieStrzelajGraczu = false;//Zpowrotem pozwala na strzelanie
        ustawJakoDoZniszczenia = false;     //Niepozwala włączyć pola edycji obiektów do wysadzenia
    }
}
