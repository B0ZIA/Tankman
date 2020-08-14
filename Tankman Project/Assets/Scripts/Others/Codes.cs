using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Codes : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            GameManager.LocalPlayer.score += 500;
            GameManager.LocalPlayer.Dynamit += 1;
            GameManager.LocalPlayer.Naprawiarka += 1;
            GameManager.LocalPlayer.Zasoby += 1;
            GameManager.LocalPlayer.coin += 10;
        }
    }
}
