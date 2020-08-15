using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TankButton : MonoBehaviour
{
    [SerializeField]
    private ButtonState buttonState;

    [SerializeField]
    private List<TankButton> toHide = new List<TankButton>();
    [SerializeField]
    private List<TankButton> toPropose = new List<TankButton>();
    [SerializeField]
    private List<TankButton> toBlock = new List<TankButton>();
    [SerializeField]
    private List<TankButton> toSave = new List<TankButton>();

    [SerializeField]
    private GameObject hidePanel;
    [SerializeField]
    private GameObject proposePanel;
    [SerializeField]
    private GameObject blockPanel;
    [SerializeField]
    private Button myButton;
    


    private void Update()
    {
        switch (buttonState)
        {
            case ButtonState.Hide:
                Hide();
                break;
            case ButtonState.Propose:
                Propose();
                break;
            case ButtonState.Block:
                Block();
                break;
            case ButtonState.Save:
                Save();
                break;
            default:
                break;
        }
    }

    public void OnClick()
    {
        for (int i = 0; i < toHide.Count; i++)
        {
            toHide[i].buttonState = ButtonState.Hide;
        }
        for (int i = 0; i < toPropose.Count; i++)
        {
            toPropose[i].buttonState = ButtonState.Propose;
        }
        for (int i = 0; i < toBlock.Count; i++)
        {
            toBlock[i].buttonState = ButtonState.Block;
        }
        for (int i = 0; i < toSave.Count; i++)
        {
            toSave[i].buttonState = ButtonState.Save;
        }
    }

    private void Hide()
    {
        myButton.interactable = false;
        hidePanel.SetActive(true);
        proposePanel.SetActive(false);
        blockPanel.SetActive(false);
    }

    private void Block()
    {
        myButton.interactable = false;
        hidePanel.SetActive(false);
        proposePanel.SetActive(false);
        blockPanel.SetActive(true);
    }

    private void Save()
    {
        myButton.interactable = false;
        hidePanel.SetActive(false);
        proposePanel.SetActive(false);
        blockPanel.SetActive(false);
    }

    private void Propose()
    {
        if (HUDManager.tempGranicaWbicjaLewla > GameManager.LocalPlayer.score)
            Hide();
        else
        {
            myButton.interactable = true;
            hidePanel.SetActive(false);
            proposePanel.SetActive(true);
            blockPanel.SetActive(false);
        }
    }

    private enum ButtonState
    {
        Hide,
        Propose,
        Block,
        Save
    }
}
