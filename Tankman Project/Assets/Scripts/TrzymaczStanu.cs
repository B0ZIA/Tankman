using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrzymaczStanu : MonoBehaviour {

    public GameObject Player;
    public float odleglosc;
    public float odleglosc2;
    public float odlegloscX = 0;

    void Update ()
    {
        transform.position = new Vector3(Player.transform.position.x + odlegloscX, Player.transform.position.y + odleglosc, odleglosc2);
	}
}
