using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universal : MonoBehaviour {

    public bool destroy;
    public bool timeDestroy;
    public bool setActiveFalse;
    public bool timeSetActiveFalse;

    public float time=1f;

	void Start ()
    {
        if(destroy)
		    Destroy(gameObject);

        if (timeDestroy)
            Destroy(this.gameObject, time);

        if (setActiveFalse)
            gameObject.SetActive(false);

        if (timeSetActiveFalse)
            StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(time);
        gameObject.SetActive(false);
    }

	

}
