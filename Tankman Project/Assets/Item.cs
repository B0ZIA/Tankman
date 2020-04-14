using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : Photon.MonoBehaviour, ICloneable
{
    protected abstract GameObject itemObject { get; set; }
    protected Sprite texture;

    private SpriteRenderer spriteRenderer;

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
        itemObject.transform.position = ItemManager.RandomPos();

        spriteRenderer = itemObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = texture;
        spriteRenderer.sortingOrder = 10;


        itemObject.AddComponent<BoxCollider2D>().size = new Vector2(0.2f, 0.2f);
    }

    public abstract void GiveRevard(Player player);
}
