using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Item, ICloneable
{
    public Gold(Sprite _texture) : base(_texture)
    {
        Debug.Log("Stworzono nowy Gold!");
    }

    protected override GameObject itemObject { get; set; }

    public override void GiveRevard()
    {
        //TODO: Dodać punkty do sklepu
    }
}
