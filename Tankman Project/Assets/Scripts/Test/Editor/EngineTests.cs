using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class EngineTests
{
    public Engine engine;



    [UnityTest]
    public IEnumerator MoveTest()
    {
        GameObject gameObject = new GameObject();
        gameObject.transform.position = new Vector3(0, 0, 0);

        Vector3 startPos = gameObject.transform.position;
        gameObject.AddComponent<Rigidbody2D>();
        engine = gameObject.AddComponent<Engine>();
        engine.Setup();

        float speed = 10000f;

        for (int i = 0; i < 1000; i++)
        {
            engine.Move(speed);
        }
        Vector3 endPos = gameObject.transform.position;

        Assert.AreNotEqual(startPos, endPos);

        yield return null;
    }

    [Test]
    public void TestTest()
    {
        GameObject gameObject = new GameObject();
        gameObject.transform.position = Vector3.zero;

        gameObject.transform.position = new Vector3(10,10,10);

        Assert.AreEqual(new Vector3(10, 10, 10), gameObject.transform.position);
    }
}
