using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAmRemoteShoot
{
    bool check { get; set; }
    bool allow { get; set; }
    bool trafie { get; set; }

    void Check();
    void CheckShoot();
    IEnumerator CheckReload();
    void Reset();
}
