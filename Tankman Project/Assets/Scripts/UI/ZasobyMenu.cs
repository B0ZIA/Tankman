using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZasobyMenu : MonoBehaviour
{
    private void OnMouseEnter()
    {
        HUDManager.Instance.CzyPokazacMenuZasobow(true);
    }

    private void OnMouseExit()
    {
        HUDManager.Instance.CzyPokazacMenuZasobow(false);
    }
}
