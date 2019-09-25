using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, ICloneable
{
    protected abstract GameObject itemObject { get; set; }
    protected Sprite texture;

    public Item(Sprite _texture)
    {
        //Konstruktor zadba aby ustawić wszystkie atrybuty
        texture = _texture;
    }

    public virtual object Clone()
    {
        return (Gold)this.MemberwiseClone();
    }
    public virtual void Create()
    {
        itemObject = new GameObject();

        itemObject.AddComponent<SpriteRenderer>().sprite = texture;
        itemObject.transform.position = ItemManager.RandomPos();
    }

    public abstract void GiveRevard();
}
