using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ShootTests
{
    Shot shot;

    [Test]
    public void InstantianeAudioTest()
    {
        Vector3 pos = new Vector3(0, 0, 0);
        var prefab = Resources.Load("a");
        shot.InstantianeAudio(pos, (GameObject)prefab);
    }
}
