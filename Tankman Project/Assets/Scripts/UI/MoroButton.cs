using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * ###################################
 * #        by Jakub Główczyk        #
 * #            [#][ ][ ]            #
 * ###################################
 */

public class MoroButton : MonoBehaviour {

    public StanMoroButtonu stanPrzycisku;
    public Camouflage myCamouflage;
    public int ileKosztuje;
    public Button MojPrzycisk;
    public Image MojaTextura;
    public Text MojStan;    //"USED", "SELECT", "BUY"
    public GameObject mojaMetka;
    public PlayerGO playerGO;

    public Color bialy;
    public Color szary;

    Player myPlayer;



	void Start ()
    {
        MojPrzycisk = GetComponent<Button>();
        myPlayer = playerGO.myPlayer;
	}

    void Update()
    {
        SprawdzajStan();
    }

    public void SprawdzajStan()
    {
        /// AKTUALNY ///
        /// DO KUPIENIA ///
        /// DO ZALOZENIA ///

        switch (stanPrzycisku)
        {
            case StanMoroButtonu.Kupiony_Urzywany:
                UstawJakoAktualny();
                break;
            case StanMoroButtonu.Kupiony_DoPrzelaczenia:
                UstawJakoDoZalozenia();
                break;
            case StanMoroButtonu.NieKupiony:
                UstawJakoDoKupiena();
                break;
            default:
                break;
        }
    }

    void UstawJakoAktualny()
    {
        mojaMetka.SetActive(false);
        MojStan.text = "USED";
        MojStan.color = szary;
        MojaTextura.color = bialy;
    }

    void UstawJakoDoKupiena()
    {
        mojaMetka.SetActive(true);
        MojStan.text = "<color=yellow>BUY</color>";
        MojStan.color = szary;
        MojaTextura.color = szary;
    }

    void UstawJakoDoZalozenia()
    {
        mojaMetka.SetActive(false);
        MojStan.text = "SELECT";
        MojStan.color = bialy;
        MojaTextura.color = szary;
    }


    public void OnClick()
    {
        switch (stanPrzycisku)
        {
            case StanMoroButtonu.Kupiony_Urzywany:
                //NIC NIE ROBIĘ 
                break;

            case StanMoroButtonu.Kupiony_DoPrzelaczenia:
                //ZAKŁADAM TEN SKIN!!! 
                foreach (var item in ShopManager.Instance.moroButtons)
                {
                    if (item.stanPrzycisku == StanMoroButtonu.Kupiony_Urzywany)
                        item.stanPrzycisku = StanMoroButtonu.Kupiony_DoPrzelaczenia;
                }
                stanPrzycisku = StanMoroButtonu.Kupiony_Urzywany;
                ShopManager.Instance.SetTexture(myCamouflage);
                break;

            case StanMoroButtonu.NieKupiony:
                if (myPlayer.coin >= ileKosztuje)
                {
                    //KUPUJE SKIN!!!
                    foreach (var item in ShopManager.Instance.moroButtons)
                    {
                        if (item.stanPrzycisku == StanMoroButtonu.Kupiony_Urzywany)
                            item.stanPrzycisku = StanMoroButtonu.Kupiony_DoPrzelaczenia;
                    }
                    stanPrzycisku = StanMoroButtonu.Kupiony_Urzywany;
                    ShopManager.Instance.SetTexture(myCamouflage);
                    myPlayer.coin -= ileKosztuje;
                }
                break;

            default:
                break;
        }
    }


    public enum StanMoroButtonu
    {
        Kupiony_Urzywany,
        Kupiony_DoPrzelaczenia,
        NieKupiony,
    }

    public enum Camouflage
    {
        Default,
        ERDL,
        Marpat,
        Erbsenmuster,
        Puma,
        Tigerstripe,
        DPM
    }
}
