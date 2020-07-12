using System.IO;
using System.Net.Sockets;
using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Collections.Concurrent;
using System.Collections.Generic;

public class IrcClient : MonoBehaviour
{
    public MultiplayerManager multiplayerManager;
    public ConcurrentQueue<Dictionary<string, string>> eventQueue;
    public string testMessage;
    public bool sendTestMessage;
    private string _roomcodePrefix = "ghae89n3";
    private string _roomcode;
    public string _username = "";
    private string _nickname_prefix = "mtk";
    private string _combined_nickname;
    private string _findGameschannel = "uihefoefahhivaezp";  // MUST NOT HAVE CAPITAL LETTERS
    private string _server = "irc.freenode.net";
    private int _port=6667;
    private StreamReader _streamReader;
    private StreamWriter _streamWriter;
    private TcpClient _tcpClient;
    bool _connectToGame = false; // whether to connect to a game when irc first connects
    private string regexIrcPrivmsg = @":(?<sender>.*?)!.*?PRIVMSG (?<channel>.*?) :(?<message>.*)"; //parse PRIVMSG lines and get: sender, channel, message
    private string regexIrcNamesPrefix = @"^:\S+ 353.*? :"; // confirm that this result is from NAMES
    private string regexIrcNames = @"[a-zA-Z]+"; // match each name in a list given by NAMES

    public void Awake()
    {
        eventQueue = new ConcurrentQueue<Dictionary<string, string>>();
        _combined_nickname = _nickname_prefix + _username;
    }

    // Update is called once per frame
    void Update()
    {
        // send test message
        if (sendTestMessage)
        {
            sendTestMessage = false;
            SendIrcMessage(testMessage);
        }     

        // for each event in the eventQueue,
        // synchronously call OnEvent on the MultiplayerManager
        Dictionary<string, string> nextEvent;
        while (eventQueue.TryDequeue(out nextEvent)) {
            OnEvent(nextEvent);
        }
    }

    public void InitializeIrc()
    {
        // initialize connection
        _tcpClient = new TcpClient(_server, _port);
        _streamReader = new StreamReader(_tcpClient.GetStream());
        _streamWriter = new StreamWriter(_tcpClient.GetStream());

        // Debug.Log("Joining the channel");
        // join the irc room
        _streamWriter.WriteLine("PASS " + "password");
        _streamWriter.WriteLine("NICK " + _combined_nickname);
        _streamWriter.WriteLine("USER " + _combined_nickname + " 8 * :" + _combined_nickname);
        if (_connectToGame) {
            _streamWriter.WriteLine("JOIN #" + _roomcode);
        }
        _streamWriter.Flush();

        multiplayerManager.SendEventSeed();

        // Create a task for reading messages from IRC
        Task taskReceiveMessages = new Task( () => AsyncReceiveMessages(_streamReader));
        taskReceiveMessages.Start();
    }

    public void SendIrcMessage(string arg_message)
    {
        _streamWriter.WriteLine("PRIVMSG " + _roomcode + " :" + arg_message);
        // _streamWriter.WriteLine(arg_message);
        _streamWriter.Flush();
    }

    public void AsyncReceiveMessages(StreamReader arg_streamReader) {
        string line = null;
        do {
            // read messages
            try {
                line = arg_streamReader.ReadLine();
                // handle PING: respond to PING with PONG
                if (line.Contains("PING"))
                {
                    _streamWriter.WriteLine("PONG");
                    _streamWriter.Flush();
                }
                // handle PRIVMSG
                Match match = Regex.Match(line, regexIrcPrivmsg);
                if (match.Success)
                {
                    var data = match.Groups;
                    eventQueue.Enqueue(new Dictionary<string, string> {
                        {"name", "message"},
                        {"channel", data["channel"].Value},
                        {"sender", data["sender"].Value},
                        {"body", data["message"].Value},
                    });
                }
                // // handle NAMES
                // else if (Regex.IsMatch(line, regexIrcNamesPrefix)){
                //     line = Regex.Split(line, regexIrcNamesPrefix)[1];
                    
                //     // generate list of names:
                //     var names = Regex.Matches(line, regexIrcNames);
                    
                // }
                // // debug: print all irc messages
                Debug.Log(line);
            } catch (Exception e) {
                Debug.Log(e);
            }
        } while (line != null);
    }

    // Calls MultiplayerManagaer.OnEvent with proper data
    void OnEvent(Dictionary<string, string> nextEvent) {
        if (nextEvent["channel"] != _roomcode) { // if the message did not come from the right room
            return;
        }
        var e = nextEvent["body"].Split(new char[] { ' ' }, 2);
        string eventName = e[0];
        string eventData = e.Length > 1 ? e[1] : "";
        string eventSender = nextEvent["sender"].Replace(_nickname_prefix, "");
        multiplayerManager.OnEvent(eventName, eventSender, eventData);
    }

    void MakeRoomCode(int arg_length) {
        string letters = "abcdefghijkmnpqrstuvwxyz123456789";
        _roomcode = "#";

        for (int i = 0; i < arg_length; i++) {
            int j = (int)(UnityEngine.Random.Range(0, 33));
            _roomcode += letters[j];
        }
    }

    public string HostGame() {
        InitializeIrc();
        MakeRoomCode(5);
        _streamWriter.WriteLine("JOIN " + _roomcode);
        _streamWriter.Flush();
        _connectToGame = true;
        return _roomcode;
    }

    public void JoinGame(string arg_roomcode, string arg_username) {
        InitializeIrc();
        if (arg_roomcode[0] != '#') {
            _roomcode = "#" + arg_roomcode;
        } else {
            _roomcode = arg_roomcode;
        }
        _username = arg_username;
        Debug.Log("JOIN " + arg_roomcode);
        _streamWriter.WriteLine("JOIN " + arg_roomcode.Trim());
        _streamWriter.Flush();
        _connectToGame = true;
    }
}