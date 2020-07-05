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

        tempGracz.GetComponent<TankShot>().SetShootingOpportunity(true);

        if (zapora.fieldForRepair.activeInHierarchy)
        {
            zapora.Repair();
            Instantiate(DzwiekNaprawy);
            tempGracz.GetComponent<PlayerGO>().myPlayer.Naprawiarka -= 1;
            tempGracz.GetComponent<PlayerGO>().triggerSth.StartCoroutine(tempGracz.GetComponent<PlayerGO>().triggerSth.ResetColliera());
            HUDManager.Instance.naprawiarkaTlo.SetActive(false);
            tempGracz.GetComponent<PlayerGO>().triggerSth.DezaktywujNaprawianie();
        }
        if (zapora.fieldForDestroy.activeInHierarchy)
        {
            zapora.Destroy();
            Instantiate(DzwiekNiszczenia);
            tempGracz.GetComponent<PlayerGO>().myPlayer.Dynamit -= 1;
            tempGracz.GetComponent<PlayerGO>().triggerSth.StartCoroutine(tempGracz.GetComponent<PlayerGO>().triggerSth.ResetColliera());
            HUDManager.Instance.dynamitTlo.SetActive(false);
            tempGracz.GetComponent<PlayerGO>().triggerSth.DezaktywujWysadzanie();
        }
        zapora.fieldForRepair.SetActive(false);
        zapora.fieldForDestroy.SetActive(false);
        GetComponent<SpriteRenderer>().color = normalnyKolor;
    }

    public void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().color = podkreslonyKolor;
        GameManager.LocalPlayer.gameObject.GetComponent<TankShot>().SetShootingOpportunity(false);
    }

    public void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = normalnyKolor;
    }
}
