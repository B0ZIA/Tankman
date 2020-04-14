using UnityEngine;
/*
 * ###################################
 * #        by Jakub Główczyk        #
 * #            [#][#][#]            #
 * ###################################
 */

public static class TagManager
{
    public static void SetGameObjectTag(GameObject gameObject, Tag tag)
    {
        gameObject.tag = tag.ToString();
    }

    public static string GetTag(Tag tag)
    {
        return tag.ToString();
    }

    public static Tag FindTagEnum(string tag)
    {
        Tag _tag = Tag.Null;
        Debug.Log(tag);
        try
        {
            _tag = (Tag)System.Enum.Parse(typeof(Tag), tag);
        }
        catch(UnityException ue)
        {
            Debug.LogWarning(ue.Message);
        }

        Debug.Log(_tag);
        return _tag;
    }
}
public enum Tag
{
    /// <summary>Default Tag</summary>
    Null,
    /// <summary>Main bot object</summary>
    Bot,
    /// <summary>local player object when tank have collider</summary>
    LocalPlayerBody,
    /// <summary>remote player object when tank have collider</summary>
    RemotePlayerBody,
    /// <summary>Main local player object</summary>
    LocalPlayer,
    /// <summary>Main remote player object</summary>
    RemotePlayer,
    /// <summary>bullet object</summary>
    Bullet,
    /// <summary>dragon teeth that are repaired</summary>
    RepairedBarrier,
    /// <summary>dragon teeth that are destroyed</summary>
    DestroyedBarrier,
    /// <summary>Item which you can find on current map</summary>
    Dymanite,
    /// <summary>Item which you can find on current map</summary>
    RepairDevice,
    /// <summary>Item which you can find on current map</summary>
    Resources,
    /// <summary>Item which you can find on current map</summary>
    Coin,
    /// <summary>Item which you can find on current map</summary>
    Score,
    /// <summary>Object with collider when tank have limit movening</summary>
    Water,
    /// <summary>Object with collider when tank can dead</summary>
    DeepWater,
    /// <summary>GameObject which spawn items</summary>
    FoodSpawner,
    /// <summary>Point where player can spawn your tank</summary>
    PlayerSpawn,
    /// <summary>all GameObject on the current map with which the bullet can collide (bridge,house etc.)</summary>
    StaticGameObject,
    /// <summary>Bots driving on this points</summary>
    RoadPoint
}
