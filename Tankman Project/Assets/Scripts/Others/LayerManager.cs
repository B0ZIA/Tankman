using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerManager
{
    public static void SetGameObjectLayer(GameObject gameObject, Layer layer)
    {
        gameObject.layer = (int)layer;
    }

    public static int GetLayer(Layer layer)
    {
        return (int)layer;
    }

    public static Layer FindLayerEnum(int layer)
    {
        Layer _layer = Layer.Default;
        try
        {
            _layer = (Layer)layer;
        }
        catch (UnityException ue)
        {
            Debug.LogWarning(ue.Message);
        }

        return _layer;
    }
}
public enum Layer
{
    Default = 0,
    TransparentFX = 1,
    IgnoreRaycast = 2,
    Water = 4,
    UI = 5,
    LocalPlayer = 10,
    RemotePlayer = 11,
}
