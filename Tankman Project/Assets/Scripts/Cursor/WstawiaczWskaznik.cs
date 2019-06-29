using UnityEngine;

public class WstawiaczWskaznik : MonoBehaviour {

    public WskaznikManager myWskaznikManager;

    private void OnMouseDown()
    {
        myWskaznikManager.Wstaw();
    }
}
