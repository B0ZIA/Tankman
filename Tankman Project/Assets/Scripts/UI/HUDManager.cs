using UnityEngine;  
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/*
 * ###################################
 * #        by Jakub Główczyk        #
 * #            [#][ ][ ]            #
 * ###################################
 */

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    public static int tempGranicaWbicjaLewla;

    public PlayerGO playerGO;
    public TechTree techTree;

    public GameObject[] zebatki;
    public GameObject zasobyMenu;

    public Slider SliderHp;
    public Slider SliderHpOnTank;
    public Slider SliderExp;
    public Slider SliderAmmo;
    public Slider SliderReload;
    public Slider SliderRegeneration;
    public Color fullExpFillColor;
    public Color normalExpFillColor;

    public Text meltTime;
    public Text hpMaxHp;
    public Text expMaxExpText;
    public Text coinText;
    public Text coinTextInShoop;
    public Text currentAmmoText;
    public static Text nickText;
    public Text _nickText;
    public static Text consoll;
    public Text _consoll;
    public Text dynamitText;
    public Text naprawiarkaText;
    public Text zasobyText;
    public Image dynamit;
    public GameObject dynamitTlo;
    public Image naprawiarka;
    public GameObject naprawiarkaTlo;
    public Image zasoby;

    [SerializeField]
    private PhotonView myPV;


    public void Awake()
    {
        if (!myPV.isMine)
            enabled = false;
        else if (Instance == null)
            Instance = this;
        else
            enabled = false;
    }

    void Start ()
    {
        consoll = _consoll;
        nickText = _nickText;
        nickText.text = playerGO.myPlayer.nick;
        StartRefresh();
	}


    public void StartRefresh()
    {
        StartCoroutine(SetHUDOnUpdate());
    }

    IEnumerator SetHUDOnUpdate()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            UpdateHud();
        }
    }

	
	void UpdateHud ()
    {
        //Aktualizuje Wszystkie slidery
        SliderHp.value = CalculateHealth();
        SliderAmmo.value = CalculateAmmo();
        SliderReload.value = CalculateReloadTime();
        if (playerGO.myPlayer.hp != TankHealth.Instance.MaxHP)
            SliderRegeneration.value = CalculateTimeToRegeneration();
        else
            SliderRegeneration.value = 1;
        if (SliderExp.value == 1)
            SliderExp.fillRect.GetComponent<Image>().color = fullExpFillColor;
        else
            SliderExp.fillRect.GetComponent<Image>().color = normalExpFillColor;

        //Aktualizuje Texty
        hpMaxHp.text = playerGO.myPlayer.hp + "/" + TankHealth.Instance.MaxHP;
        if (GameManager.LocalPlayer.tankTier == TankTier.CzrawtyTier)
            expMaxExpText.text = playerGO.myPlayer.score + "/∞";
        else
            expMaxExpText.text = playerGO.myPlayer.score + "/" + tempGranicaWbicjaLewla.ToString();
        currentAmmoText.text = TankShoot.Instance.TempMaxAmmo.ToString();

        coinText.text = "   Coin: <color=yellow>" + playerGO.myPlayer.coin.ToString() + "</color>";
        coinTextInShoop.text = coinText.text;

        zasobyText.text = "x" + playerGO.myPlayer.Zasoby.ToString();
        naprawiarkaText.text = "x" + playerGO.myPlayer.Naprawiarka.ToString();
        dynamitText.text = "x" + playerGO.myPlayer.Dynamit.ToString();

        //Aktualizuje zębatki czyli tier czołgu
        switch (GameManager.LocalPlayer.tankTier)
        {
            case TankTier.PierwszyTier:
                PrzelaczZebatki(0);
                tempGranicaWbicjaLewla = GameManager.FIRST_LEVEL_LIMIT;
                SliderExp.value = (float)playerGO.myPlayer.score / tempGranicaWbicjaLewla;
                break;

            case TankTier.DrugiTier:
                PrzelaczZebatki(1);
                tempGranicaWbicjaLewla = GameManager.SECOND_LEVEL_LIMIT;
                SliderExp.value = ((float)playerGO.myPlayer.score - GameManager.FIRST_LEVEL_LIMIT) / (tempGranicaWbicjaLewla - GameManager.FIRST_LEVEL_LIMIT);
                break;

            case TankTier.TrzeciTier:
                PrzelaczZebatki(2);
                tempGranicaWbicjaLewla = GameManager.THIRD_LEVEL_LIMIT;
                SliderExp.value = ((float)playerGO.myPlayer.score - GameManager.SECOND_LEVEL_LIMIT) / (tempGranicaWbicjaLewla - GameManager.SECOND_LEVEL_LIMIT);
                break;

            case TankTier.CzrawtyTier:
                PrzelaczZebatki(3);
                tempGranicaWbicjaLewla = playerGO.myPlayer.score + 1501;
                SliderExp.value = 1;
                break;
        }
    }

    private float CalculateAmmo()
    {
        return (float)TankShoot.Instance.TempMaxAmmo / (float)TankShoot.Instance.MaxAmmo;
    }

    private float CalculateReloadTime()
    {
        return ((TankShoot.Instance.realReloadTime * -1f) + TankShoot.Instance.ReloadTime) / TankShoot.Instance.ReloadTime;
    }

    private float CalculateTimeToRegeneration()
    {
        return ((TankHealth.Instance.tempTime * -1f) + TankHealth.Instance.CzasDoRozpoczeciaRegeneracji) / TankHealth.Instance.CzasDoRozpoczeciaRegeneracji;
    }


    void PrzelaczZebatki(int aktywnaZebatka)
    {
        for (int i = 0; i < zebatki.Length; i++)
        {
            if (i == aktywnaZebatka)
                zebatki[i].SetActive(true);
            else if (i < aktywnaZebatka)
                zebatki[i].SetActive(true); 
            else
                zebatki[i].SetActive(false);
        }
    }


    public float CalculateHealth()
    {
        return GameManager.LocalPlayer.hp / TankHealth.Instance.MaxHP;
    }


    public enum TankTier
    {
        PierwszyTier,
        DrugiTier,
        TrzeciTier,
        CzrawtyTier
    }

    public static void OnEnterButton(string text)
    {
        consoll.text = text;
        nickText.gameObject.SetActive(false);
    }

    public void OnEnterButtonUI(string text)
    {
        consoll.text = text;
        nickText.gameObject.SetActive(false);
    }

    public void OnExitButton()
    {
        consoll.text = "";
        nickText.gameObject.SetActive(true);
    }


    public void CzyPokazacMenuZasobow(bool tak)
    {
        if (tak)
            zasobyMenu.SetActive(true);
        else
            zasobyMenu.SetActive(true); //TODO: inna opcja
    }
}
