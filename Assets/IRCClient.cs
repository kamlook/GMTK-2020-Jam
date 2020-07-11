using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
using System.Threading.Tasks;

public class IRCClient : MonoBehaviour
{
    public string TestMessage;
    public bool SendTestMessage;
    public string nickname = "mrbrandon";
    private string _channel = "dobdob";  // MUST NOT HAVE CAPITAL LETTERS
    private string _server = "irc.freenode.net";
    private int _port=6667;
    private StreamReader _streamReader;
    private StreamWriter _streamWriter;
    private TcpClient _tcpClient;

    // Start is called before the first frame update
    void Start()
    {
        // initialize connection
        _tcpClient = new TcpClient(_server, _port);
        _streamReader = new StreamReader(_tcpClient.GetStream());
        _streamWriter = new StreamWriter(_tcpClient.GetStream());

        // Debug.Log("Joining the channel");
        // join the irc room
        _streamWriter.WriteLine("PASS " + "password");
        _streamWriter.WriteLine("NICK " + nickname);
        _streamWriter.WriteLine("USER " + nickname + " 8 * :" + nickname);
        _streamWriter.WriteLine("JOIN #" + _channel);
        _streamWriter.Flush();

        // Create a task for reading messages from IRC
        Task taskReceiveMessages = new Task( () => ReceiveMessages(_streamReader));
        taskReceiveMessages.Start();
    }

    // Update is called once per frame
    void Update()
    {
        // send test message
        if (SendTestMessage)
        {
            SendTestMessage = false;
            SendMessage(TestMessage);
        }        
    }

    public void SendMessage(string arg_message)
    {
        _streamWriter.WriteLine("PRIVMSG #" + _channel + " :" + arg_message);
        // _streamWriter.WriteLine(arg_message);
        _streamWriter.Flush();
    }

    public void ReceiveMessages(StreamReader arg_streamReader) {
        string line = null;
        do {
            // read messages
            try {
                line = arg_streamReader.ReadLine();
                Debug.Log(line);
            } catch (Exception e) {
                Debug.Log(e);
            }
        } while (line != null);
        
    }
}
