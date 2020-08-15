using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : Photon.MonoBehaviour
{
    public abstract void GiveRevard(Player player);
}
