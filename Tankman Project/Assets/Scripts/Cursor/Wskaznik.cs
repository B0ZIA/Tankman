using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wskaznik : MonoBehaviour {

    public TrybWskaznika trybWskaznika;
    //public float distance;



    public float dystansDoMyszki;
    public Transform raycastStart; //Start fite point



    public float maxDistance;
    //public GameObject object1;
    public GameObject maxFirePoint;
    public LayerMask WhatToHit;

    public Color red;
    public Color green;
    public Color yellow;



    void Update ()
    {
        switch (trybWskaznika)
        {
            case TrybWskaznika.Bezkolizyjny:
                ObracajBezkolizyjnie();
                break;
            case TrybWskaznika.Kolizyjny:
                ObracajKolizyjnie();
                break;
        }
        transform.rotation = Quaternion.identity;
    }

    void ObracajBezkolizyjnie()
    {
        //distance = Vector2.Distance(object1.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //wskaznik.transform.localPosition = new Vector3(-distance * 3, 0, 0);
    }

    void ObracajKolizyjnie()
    {
        dystansDoMyszki = Vector2.Distance(raycastStart.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));

        maxFirePoint.transform.localPosition = new Vector3(-dystansDoMyszki * 3f, 0f, 0f);

        Vector2 mousePosition = new Vector2 (maxFirePoint.transform.position.x, maxFirePoint.transform.position.y);
        Vector2 raycastStartPos = new Vector2(raycastStart.position.x, raycastStart.position.y);
        RaycastHit2D hit = Physics2D.Raycast(raycastStartPos, mousePosition - raycastStartPos, dystansDoMyszki, WhatToHit);
        Debug.DrawLine(raycastStartPos, mousePosition, Color.cyan);

        if (hit.collider != null)
        {
            if(hit.collider.tag == TagManager.GetTag(Tag.RemotePlayerBody) || hit.collider.tag == TagManager.GetTag(Tag.Bot))
                GetComponent<SpriteRenderer>().color = red;
            else
                GetComponent<SpriteRenderer>().color = yellow;

            Debug.DrawLine(raycastStartPos, hit.point, Color.red);
            transform.position = new Vector3(hit.point.x, hit.point.y, 0f);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = green;
            transform.localPosition = new Vector3(-dystansDoMyszki*3, 0, 0);
        }
       //distance = Vector2.Distance(object1.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //object2.transform.localPosition = new Vector3(-distance * 3, 0, 0);
    }

    public enum TrybWskaznika
    {
        Bezkolizyjny,
        Kolizyjny
    }

}
