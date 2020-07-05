using UnityEngine;
using System.Collections.Generic;
using System;

public class TechTree : MonoBehaviour
{
    public static TechTree Instance { get; private set; }

    private bool techActive = false;
    private GameObject tempPanel;

    [SerializeField]
    private GameObject zsrrPanel;
    [SerializeField]
    private GameObject iiiRzeszaPanel;
    [SerializeField]
    private GameObject japoniaPanel;

    [SerializeField]
    private List<TankButtonEnum> tankButtons;

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

    void Start()
    {
        switch (GameManager.LocalPlayer.nation)
        {
            case NationManager.Nation.ZSRR:
                tempPanel = zsrrPanel;
                break;
            case NationManager.Nation.IIIRZESZA:
                tempPanel = iiiRzeszaPanel;
                break;
            case NationManager.Nation.JAPONIA:
                tempPanel = japoniaPanel;
                break;
        }
    }

    //Pokazuje lub ukrywa Drzewko Technologii
    public void TechTreeButton()
    {
        if (techActive)
        {
            tempPanel.SetActive(false);
            techActive = false;
        }
        else
        {
            tempPanel.SetActive(true);
            techActive = true;
        }
    }

    public void EnebledTechTreeCanvas(bool enebled)
    {
        tempPanel.SetActive(enebled);
        techActive = enebled;
    }

    public void TankSwitchTierButton (int myNewTier)
    {
        switch (myNewTier-1)
        {
            case (int)HUDManager.TankTier.PierwszyTier:
                GameManager.LocalPlayer.tankTier = HUDManager.TankTier.PierwszyTier;
                break;
            case (int)HUDManager.TankTier.DrugiTier:
                GameManager.LocalPlayer.tankTier = HUDManager.TankTier.DrugiTier;
                break;
            case (int)HUDManager.TankTier.TrzeciTier:
                GameManager.LocalPlayer.tankTier = HUDManager.TankTier.TrzeciTier;
                break;
            case (int)HUDManager.TankTier.CzrawtyTier:
                GameManager.LocalPlayer.tankTier = HUDManager.TankTier.CzrawtyTier;
                break;
            default:
                break;
        }
    }

    public TankButton FindTankButton(DostempneCzolgi myTank)
    {
        for (int i = 0; i < tankButtons.Count; i++)
        {
            if (tankButtons[i].tank == myTank)
                return tankButtons[i].tankButton;
        }
        return null;
    }

    public DostempneCzolgi FindTankEnum(TankButton myTankButton)
    {
        for (int i = 0; i < tankButtons.Count; i++)
        {
            if (tankButtons[i].tankButton == myTankButton)
                return tankButtons[i].tank;
        }
        return 0;
    }
}

[Serializable]
public struct TankButtonEnum
{
    public DostempneCzolgi tank;
    public TankButton tankButton;
}

