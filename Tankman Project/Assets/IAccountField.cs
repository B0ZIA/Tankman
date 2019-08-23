using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAccountField
{
    GameObject LoginOrRegisterPanel { get; set; }
    GameObject RegisterPanel { get; set; }
    GameObject LoginPanel { get; set; }
    GameObject EnternPanel { get; set; }

    void ShowAccountPanel(AccountPanel type);
}
