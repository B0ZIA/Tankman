using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    public bool shopActive = false;
    public GameObject shopCanvas;
    public Text updatePointText;
    public Player player;

    public GameObject moroCanvas;
    public GameObject tankCanvas;
    public GameObject newUpdateIcon;

    [Range(0,Player.maxUpdatePoint)] public int tempUpdatePoint = 0;

    [Range(0, 5)] public int dzialoLevel = 0;
    [Range(0, 5)] public int zawieszenieLevel = 0;
    [Range(0, 5)] public int pancerzLevel = 0;
    [Range(0, 5)] public int silnikLevel = 0;

    public Image[] area_1;
    public Image[] area_2;
    public Image[] area_3;
    public Image[] area_4;

    public Button[] buttons;

    public Color areaInactive;
    public Color area1Active;
    public Color area2Active;
    public Color area3Active;
    public Color area4Active;

    public Material erdlMat;
    public Material marpatMat;
    public Material erbseMat;
    public Material pumaMat;
    public Material tigerstripeMat;
    public Material dpmMat;

    [Header("Sklep MORO")]
    public MoroButton[] moroButtons;

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

        player = TankEvolution.Instance.GetComponent<PlayerGO>().myPlayer;
        shopCanvas.SetActive(false);    
    }

    void Update()
    {
        if (!shopActive)
            return;

        tempUpdatePoint = player.updatePoint - dzialoLevel - zawieszenieLevel - pancerzLevel - silnikLevel;
        if (tempUpdatePoint < 0)
        {
            dzialoLevel = 0;
            zawieszenieLevel = 0;
            pancerzLevel = 0;
            silnikLevel = 0;
        }

        if (tempUpdatePoint > 0)
            UnlockButton();
        else
            LockButton();

        if (tempUpdatePoint == 0)
        {
            updatePointText.text = "";
            newUpdateIcon.SetActive(false);
        }
        else
        {
            updatePointText.text = "x" + tempUpdatePoint;
            newUpdateIcon.SetActive(true);
        }
    }


    public void ResetUpdate()
    {
        dzialoLevel = 0;
        zawieszenieLevel = 0;
        pancerzLevel = 0;
        silnikLevel = 0;
        CheckUpdate();
    }


    void UnlockButton()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }
    }

    void LockButton()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
    }


    void CheckUpdate()
    {
        for (int i = 0; i < area_1.Length; i++)
        {
            area_1[i].color = (i < dzialoLevel) ? area1Active : areaInactive;
        }
        for (int i = 0; i < area_2.Length; i++)
        {
            area_2[i].color = (i < zawieszenieLevel) ? area2Active : areaInactive;
        }
        for (int i = 0; i < area_3.Length; i++)
        {   
            area_3[i].color = (i < pancerzLevel) ? area3Active : areaInactive;
        }
        for (int i = 0; i < area_4.Length; i++)
        {
            area_4[i].color = (i < silnikLevel) ? area4Active : areaInactive;
        }
    }


    public void UpdateDzialo()
    {
        if (dzialoLevel < area_1.Length)
        {
            dzialoLevel++;
            tempUpdatePoint--;
            TankEvolution.Instance.UpdateTank(TanksData.FindTankData(player.tank));
        }
        CheckUpdate();
    }

    public void UpdateZawieszenie()
    {
        if (zawieszenieLevel < area_2.Length)
        {
            zawieszenieLevel++;
            tempUpdatePoint--;
            TankEvolution.Instance.UpdateTank(TanksData.FindTankData(player.tank));
        }
        CheckUpdate();
    }

    public void UpdatePancerz()
    {
        if (pancerzLevel < area_3.Length)
        {
            pancerzLevel++;
            tempUpdatePoint--;
            TankEvolution.Instance.UpdateTank(TanksData.FindTankData(player.tank));
        }
        CheckUpdate();
    }

    public void UpdateSilnik()
    {
        if (silnikLevel < area_4.Length)
        {
            silnikLevel++;
            tempUpdatePoint--;
            TankEvolution.Instance.UpdateTank(TanksData.FindTankData(player.tank));
        }
        CheckUpdate();
    }

    public void ShopButton()
    {
        if (shopActive)
        {
            shopCanvas.SetActive(false);
            shopActive = false;
        }
        else
        {
            shopCanvas.SetActive(true);
            shopActive = true;
        }
    }

    public void EnebledShoopCanvas(bool enebled)
    {
        shopCanvas.SetActive(enebled);
        shopActive = enebled;
    }


    public void TankButton()
    {
        tankCanvas.SetActive(true);
        moroCanvas.SetActive(false);
    }


    public void MoroButton()
    {
        tankCanvas.SetActive(false);
        moroCanvas.SetActive(true);
    }


    public void SetTexture(MoroButton.Camouflage myCamoflage)
    {
        GameManager.LocalPlayer.gameObject.GetComponent<TankRPC>().myPV.RPC("SetCamouflage", PhotonTargets.AllBuffered, myCamoflage);
    }
}
