using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullWindow : MonoBehaviour
{
    public Text resolutionText;

    private int Width = 0;
    private int Height = 0;



    public void SetWidth(int width)
    {
        Width = width;
    }

    public void SetHeight(int height)
    {
        Height = height;
    }

    public void OnResize()
    {
        resolutionText.text = Width + "x" + Height;
        Screen.SetResolution(Width, Height, Screen.fullScreen);
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetWidth(1920);
        SetHeight(1005);
        OnResize();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            Screen.fullScreen = true;
        }
    }
}
