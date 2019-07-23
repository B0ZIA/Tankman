using UnityEngine;

public class WstawiaczWskaznik : MonoBehaviour {

    public BarrierBuilder barrierBuilder;

    private void OnMouseDown()
    {
        barrierBuilder.BuildBarrier();
    }
}
