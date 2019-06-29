using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * ###################################
 * #        by Jakub Główczyk        #
 * #            [#][#][#]            #
 * ###################################
 */

/// <summary>
/// The Script responsible for choosing the game mode: {1vs1, Blitskrieg, 2 Teams, FFA}
/// </summary>
public class ModeManager : MonoBehaviour {

    [SerializeField]
	private GameObject[] modeButton;

    private readonly Color32 active = new Color32(0, 0, 0, 255);    //czarny
    private readonly Color32 inActive = new Color32(0, 0, 0, 61);   //czarno-przeźzroczysty

    private void Start()
    {
        ModeButton(0);
    }

	public void ModeButton(int mode)
	{
		switch (mode)
		{
			case (int)Mode.FFA:
                GameManager.myMode = Mode.FFA;
			 break;
			case (int)Mode.Blitzkrieg:
                GameManager.myMode = Mode.Blitzkrieg;
			break;
			case (int)Mode.TwoTeams:
                GameManager.myMode = Mode.TwoTeams;
			 break;
			case (int)Mode.OnevsOne:
                GameManager.myMode = Mode.OnevsOne;
			 break;
			default:
				Debug.Log("Nie ustawiono właściwego trybu gry!");
			 break;
		}

		for (int i = 0; i < modeButton.Length; i++)
		{
            if (i == mode)
                modeButton[i].GetComponent<Image>().color = active;
            else
                modeButton[i].GetComponent<Image>().color = inActive;
        }
	}

	public enum Mode
	{
		FFA,
		Blitzkrieg,
		TwoTeams,
		OnevsOne
	}
}
