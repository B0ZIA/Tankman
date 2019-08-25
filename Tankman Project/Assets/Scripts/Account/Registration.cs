using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Registration : MonoBehaviour
{
    public InputField nameField;
    public InputField passwordField;

    public Button submitButton;

    public void CallRegister(IAccountField accountField)
    {
        StartCoroutine(Register(accountField));
    }

    public IEnumerator Register(IAccountField accountField)
    {
        WWWForm form = new WWWForm();
        form.AddField("name",nameField.text);
        form.AddField("password", passwordField.text);

        WWW www = new WWW("http://localhost/sqlconnect/register.php", form);
        yield return www;
        if (www.text == "0")
        {
            Debug.Log("User created successfully!");
            accountField.ShowAccountPanel(AccountPanel.LoginOrRegister);
        }
        else
        {
            Debug.Log("User creation failed. Error #" + www.text);
        }
    }

    public void VerifyInputs()
    {
        submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
    }
}
