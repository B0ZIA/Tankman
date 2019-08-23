using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountContlorer : MonoBehaviour
{
    private AccountField accountField;
    [SerializeField] private Registration registration;
    [SerializeField] private Login login;

    public Text playerDisplay;

    void Awake()
    {
        accountField = new AccountField();
    }

    void Update()
    {
        if (DBManager.LogedIn)
        {
            playerDisplay.text = "Player: " + DBManager.username;
        }
    }

    public void EnternAccount_Click()
    {
        accountField.ShowAccountPanel(AccountPanel.LoginOrRegister);
    }

    public void RegisterPlayer_Click()
    {
        accountField.ShowAccountPanel(AccountPanel.Register);
    }

    public void LoginPlayer_Click()
    {
        accountField.ShowAccountPanel(AccountPanel.Login);
    }

    public void Register_Click()
    {
        registration.CallRegister(accountField);
    }

    public void Login_Click()
    {
        login.CallLogin(accountField);
    }
}
