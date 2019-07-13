using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleRobocze : MonoBehaviour {

    public AntiTankBarrier zapora;
    public Color normalnyKolor;
    public Color podkreslonyKolor;
    public GameObject DzwiekNiszczenia;
    public GameObject DzwiekNaprawy;


    /// <summary>
    /// Jeśli klikniesz w pole robocze wykonują się akcje z wysadzeniem/naprawieniem
    /// </summary>
    public void OnMouseDown()
    {
        //gracz który wykonuje akcje
        GameObject tempGracz = GameManager.LocalPlayer.gameObject;

        tempGracz.GetComponent<TankShoot>().SetShootingOpportunity(true);

        if (zapora.poleDoNaprawy.activeInHierarchy)
        {
            zapora.NaprawZasiek();
            Instantiate(DzwiekNaprawy);
            tempGracz.GetComponent<PlayerGO>().myPlayer.Naprawiarka -= 1;
            //tempGracz.GetComponent<PlayerGO>().triggerSth.szukajDoNaprawy = false;
            tempGracz.GetComponent<PlayerGO>().triggerSth.StartCoroutine(tempGracz.GetComponent<PlayerGO>().triggerSth.ResetColliera());
            HUDManager.Instance.naprawiarkaTlo.SetActive(false);
            tempGracz.GetComponent<PlayerGO>().triggerSth.DezaktywujNaprawianie();
        }
        if (zapora.poleDoNiszczenia.activeInHierarchy)
        {
            zapora.ZniszczZasiek();
            Instantiate(DzwiekNiszczenia);
            tempGracz.GetComponent<PlayerGO>().myPlayer.Dynamit -= 1;
            //tempGracz.GetComponent<PlayerGO>().triggerSth.szukajDoZniszczenia = false;
            tempGracz.GetComponent<PlayerGO>().triggerSth.StartCoroutine(tempGracz.GetComponent<PlayerGO>().triggerSth.ResetColliera());
            HUDManager.Instance.dynamitTlo.SetActive(false);
            tempGracz.GetComponent<PlayerGO>().triggerSth.DezaktywujWysadzanie();
        }
        zapora.poleDoNaprawy.SetActive(false);
        zapora.poleDoNiszczenia.SetActive(false);
        GetComponent<SpriteRenderer>().color = normalnyKolor;
        //tempGracz.GetComponent<TankShoot>().nieStrzelajBoNajechalesNaPrzycisk = false;
    }

    public void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().color = podkreslonyKolor;
        GameManager.LocalPlayer.gameObject.GetComponent<TankShoot>().SetShootingOpportunity(false);
    }

    public void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = normalnyKolor;
    }
}
