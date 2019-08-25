using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountField : MonoBehaviour, IAccountField
{
    public GameObject LoginOrRegisterPanel { get; set; }
    public GameObject RegisterPanel { get; set; }
    public GameObject LoginPanel { get; set; }
    public GameObject EnternPanel { get; set; }



    public AccountField()
    {
        try
        {
            LoginOrRegisterPanel = GameObject.FindGameObjectWithTag("LoginOrRegisterPanel");
            RegisterPanel = GameObject.FindGameObjectWithTag("RegisterPanel");
            LoginPanel = GameObject.FindGameObjectWithTag("LoginPanel");
            EnternPanel = GameObject.FindGameObjectWithTag("EnternPanel");
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
        ShowAccountPanel(AccountPanel.Disabled);
    }

    public void ShowAccountPanel(AccountPanel type)
    {
        HideAllAccountPanel();

        switch (type)
        {
            case AccountPanel.LoginOrRegister:
                LoginOrRegisterPanel.SetActive(true);
                break;
            case AccountPanel.Login:
                LoginPanel.SetActive(true);
                break;
            case AccountPanel.Register:
                RegisterPanel.SetActive(true);
                break;
            case AccountPanel.Disabled:
                EnternPanel.SetActive(true);
                break;
        }
    }

    private void HideAllAccountPanel()
    {
        LoginOrRegisterPanel.SetActive(false);
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(false);
        EnternPanel.SetActive(false);
    }
}

public enum AccountPanel
{
    LoginOrRegister,
    Login,
    Register,
    Disabled
}