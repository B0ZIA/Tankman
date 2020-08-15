using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The Script responsible for choosing the game mode: {1vs1, Blitskrieg, 2 Teams, FFA}
/// </summary>
public class BattleMode : MonoBehaviour {

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
			case (int)Type.FFA:
                GameManager.myMode = Type.FFA;
			 break;
			case (int)Type.Blitzkrieg:
                GameManager.myMode = Type.Blitzkrieg;
			break;
			case (int)Type.TwoTeams:
                GameManager.myMode = Type.TwoTeams;
			 break;
			case (int)Type.OnevsOne:
                GameManager.myMode = Type.OnevsOne;
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

	public enum Type
	{
		FFA,
		Blitzkrieg,
		TwoTeams,
		OnevsOne
	}
}
