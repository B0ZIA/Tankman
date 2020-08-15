using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nick : Photon.MonoBehaviour
{
    public Text nick;
    public Color transparent;
    public Image fill;
    public Image background;


    void Start ()
    {
        if (photonView.isMine == true)
        {
            nick.color = transparent;
            fill.color = transparent;
            background.color = transparent;
            nick.text = GetComponent<PlayerGO>().myPlayer.nick;
        }
    }
}
