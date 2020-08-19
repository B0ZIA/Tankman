using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour {

    public float distance;
    public Transform raycastStart;

    public float maxDistance;
    public GameObject maxFirePoint;
    public LayerMask WhatToHit;

    public Color red;
    public Color green;
    public Color yellow;



    void Update ()
    {
        FollowMouse();

        transform.rotation = Quaternion.identity;
    }

    void FollowMouse()
    {
        distance = Vector2.Distance(raycastStart.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));

        maxFirePoint.transform.localPosition = new Vector3(-distance * 3f, 0f, 0f);

        Vector2 mousePosition = new Vector2 (maxFirePoint.transform.position.x, maxFirePoint.transform.position.y);
        Vector2 raycastStartPos = new Vector2(raycastStart.position.x, raycastStart.position.y);
        RaycastHit2D hit = Physics2D.Raycast(raycastStartPos, mousePosition - raycastStartPos, distance, WhatToHit);
        Debug.DrawLine(raycastStartPos, mousePosition, Color.cyan);

        if (hit.collider != null)
        {
            if(hit.collider.tag == TagsManager.GetTag(Tag.RemotePlayerBody) || hit.collider.tag == TagsManager.GetTag(Tag.Bot))
                GetComponent<SpriteRenderer>().color = red;
            else
                GetComponent<SpriteRenderer>().color = yellow;

            Debug.DrawLine(raycastStartPos, hit.point, Color.red);
            transform.position = new Vector3(hit.point.x, hit.point.y, 0f);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = green;
            transform.localPosition = new Vector3(-distance*3, 0, 0);
        }
    }
}
