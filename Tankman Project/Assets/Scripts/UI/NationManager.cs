using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NationManager : MonoBehaviour {

    public GameObject[] flagi;  //Bardzo ważna jest kolejność: według enuma Nation... od 0, rosnąco
    private static Nation _myNation = Nation.IIIRZESZA;
    public static Nation myNation
    {
        get { return _myNation; }
        private set { }
    }

    public Color active;
    public Color inactive;

    public void JesliKliknieszWFlage(int IDnacji)
    {
        switch (IDnacji)
        {
            case (int)Nation.ZSRR:
                _myNation = Nation.ZSRR;
                break;

            case (int)Nation.IIIRZESZA:
                _myNation = Nation.IIIRZESZA;
                break;

            case (int)Nation.JAPONIA:
                _myNation = Nation.JAPONIA;
                break;

            default:
                Debug.LogError("Nie wybrano właściwej nacjii!");
                break;
        }

        for (int i = 0; i < flagi.Length; i++)
        {
            if (i == IDnacji)
                flagi[i].GetComponent<Image>().color = active;
            else
                flagi[i].GetComponent<Image>().color = inactive;
        }
        GameManager.myNation = _myNation;
    }

    public enum Nation
    {
        ZSRR,
        IIIRZESZA,
        JAPONIA
    }

    public static DostempneCzolgi ReturnStartTank(Nation nation)
    {
        switch (nation)
        {
            case Nation.ZSRR:
                return DostempneCzolgi.T38;
            case Nation.IIIRZESZA:
                return DostempneCzolgi.PZI;
            case Nation.JAPONIA:
                return DostempneCzolgi.KHA_GO;
            default:
                return DostempneCzolgi.PZI;
        }
    }
}
