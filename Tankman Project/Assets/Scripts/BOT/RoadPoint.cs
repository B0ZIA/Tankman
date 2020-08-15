using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPoint : MonoBehaviour {

    public GameObject[] punktyDrogiObok;

    public void ResetCollider()
    {
        StartCoroutine(Reset());
    }

    IEnumerator Reset ()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSecondsRealtime(10f);
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
