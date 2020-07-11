using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    public string RoomID;
    public bool IsHost = false;
    public IrcClient ircClient;
    // Start is called before the first frame update
    void Start()
    {
        RoomID = ircClient.HostGame();
        Debug.Log(RoomID);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // called whenever there's a new multiplayer event
    public void OnEvent(string arg_eventName, string arg_sender, string arg_data)
    {
        Debug.Log("There is a new event (" + arg_eventName + ") from " + arg_sender + ":");
        Debug.Log(arg_data);

        switch (arg_eventName) {
            case "join":
                SendEventSeed();
                break;
            case "seed":
                break;
            case "seed_name":
                break;
        }
    }

    void SendEvent(string arg_eventName, string arg_data)
    {
        ircClient.SendIrcMessage(arg_eventName + " " + arg_data);
    }

    void SendEventJoin()
    {
        SendEvent("join", "");
    }

    public void SendEventSeed()
    {
        SendEvent("seed", "1234");
        SendEvent("seed_name", "myseed");
    }
}
