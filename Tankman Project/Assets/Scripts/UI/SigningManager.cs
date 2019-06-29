using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * ###################################
 * #        by Jakub Główczyk        #
 * #            [#][ ][ ]            #
 * ###################################
 */

/// <summary>
/// The Script responsible for choosing Player nick
/// </summary>
public class SigningManager : MonoBehaviour
{
    [SerializeField]
    private GameObject faded;
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private InputField nameFiled;

    private ConnectionManager connectionManager;
    private AudioSource music;
    private const float fadedMusicSpeed = 0.2f;
    private string nick;

    void Awake()
    {
        connectionManager = FindObjectOfType<ConnectionManager>();// GetComponent<ConnectionManager>();
        music = GetComponent<AudioSource>();
    }

    void OnSubmit()
    {
        nick = nameFiled.text;
        if (nick == "")
            nick = "SecretPlayer";
    }

    void FadedLobbyMusic()
    {
            StartCoroutine(FadedMusic());
    }

    IEnumerator FadedMusic()
    {
        while (music.volume > 0)
        {
            yield return new WaitForSecondsRealtime(fadedMusicSpeed);
            music.volume -= 0.01f;
        }
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            StartButton();
    }

    public void StartButton()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 && connectionManager != null)
        {
            faded.SetActive(true);
            OnSubmit();
            FadedLobbyMusic();
            PhotonNetwork.playerName = nick;
            connectionManager.Connect();
            canvas.SetActive(false);
        }
    }
}
