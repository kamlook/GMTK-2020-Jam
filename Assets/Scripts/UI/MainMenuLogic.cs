using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuLogic : MonoBehaviour
{
    public TextMeshProUGUI roomCodeDisplay; // used on the "host game" page
    public TextMeshProUGUI roomCodeTextInput; // used on the "join game" page
    public TextMeshProUGUI loadingMessage; // used on the "join game" page
    private MultiplayerManager _multiplayerManager;

    void Start()
    {
        _multiplayerManager = FindObjectOfType<MultiplayerManager>();
        if (roomCodeDisplay != null && _multiplayerManager != null)
        {
            roomCodeDisplay.text = _multiplayerManager.RoomID.Substring(1);
        }
    }

    public void ButtonQuit()
    {
        Application.Quit();
    }

    public void ButtonGoToMenuHost()
    {
        _multiplayerManager.HostGame();
        SceneManager.LoadScene("host_game_menu");
    }

    public void ButtonGoToMenuJoin()
    {
        SceneManager.LoadScene("join_game_menu");
    }

    public void ButtonGoToMenuStart()
    {
        SceneManager.LoadScene("start_menu");
    }

    public void ButtonJoinGame()
    {
        string roomcode = roomCodeTextInput.text;
        roomCodeTextInput.text = "";
        if (roomcode.Length - 1 != 5)
        {
            roomCodeTextInput.text = "INVALID CODE";
        } else
        {
            _multiplayerManager.JoinGame(roomcode);
            loadingMessage.gameObject.SetActive(true);
        }
    }

    public void OnTextChangeName(string arg_text)
    {

    }

    public void OnTextChangeRoomCode(string arg_text)
    {

    }
}
