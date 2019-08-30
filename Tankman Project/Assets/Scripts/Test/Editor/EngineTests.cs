using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class EngineTests
{
    public Engine engine;



    [Test]
    public void MoveTest()
    {
        GameObject gameObject = new GameObject();
        gameObject.transform.position = new Vector2(0, 0);
        Vector2 startPos = gameObject.transform.position;
        gameObject.AddComponent<Rigidbody2D>();
        engine = gameObject.AddComponent<Engine>();
        engine.Setup();
        float speed = 10000f;

        for (int i = 0; i < 10; i++)
        {
            engine.Move(speed);
        }
        Vector2 endPos = gameObject.transform.position;

        Assert.AreNotEqual(startPos, endPos);
    }
}
