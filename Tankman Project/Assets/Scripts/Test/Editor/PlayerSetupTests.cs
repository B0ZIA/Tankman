using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerSetupTests
{
    PlayerSetup ps = new PlayerSetup();



    //[Test]
    //public void SetGameObjecTag()
    //{
    //    GameObject gameObject = new GameObject();
    //    string tag = TagManager.Tag.LOCALPLAYERBODY.ToString();

    //    ps.SetGameObjectTag(gameObject, tag);

    //    Assert.AreEqual(tag, gameObject.tag);
    //}

    [Test]
    public void SetGameObjectLayerTest()
    {
        //GameObject gameObject = new GameObject();
        //int layer = PlayerSetup.LOCAL_PLAYER_LAYER;


        //ps.SetGameObjectLayer(gameObject, layer);

        //Assert.AreEqual(layer, gameObject.layer);
    }

    [Test]
    public void DisableComponentsTest()
    {
        GameObject tester = new GameObject("tester");

        tester.AddComponent<TankEngine>();
        tester.AddComponent<TankChassis>();
        tester.AddComponent<TankPeriscope>();

        Behaviour[] components = new Behaviour[] 
        {
            tester.GetComponent<TankEngine>(),
            tester.GetComponent<TankChassis>(),
            tester.GetComponent<TankPeriscope>()
        };

        ps.DisableComponents(components);

        for (int i = 0; i < components.Length; i++)
        {
            Assert.IsFalse(components[i].enabled);
        }
    }

    [Test]
    public void DisableGameObjectTest()
    {
        GameObject firstObject = new GameObject("firstObject");
        GameObject secondObject = new GameObject("secondObject");
        GameObject thirdObject = new GameObject("thirdObject");

        GameObject[] gameObjects = new GameObject[] { firstObject, secondObject, thirdObject };

        ps.DisableGameObjects(gameObjects);

        for (int i = 0; i < gameObjects.Length; i++)
        {
            Assert.IsFalse(gameObjects[i].activeSelf);
        }
    }
}
