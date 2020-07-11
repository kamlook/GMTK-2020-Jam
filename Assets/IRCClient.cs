using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class IRCClient : MonoBehaviour
{
    public string TestMessage;
    public bool SendTestMessage;
    private string _nickname = "Brandon";
    private string _channel = "iofEFNIO3rfrK390";
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

        // join the irc room
        _streamWriter.WriteLine("NICK " + _nickname);
        _streamWriter.WriteLine("USER " + _nickname + " * * :" + _nickname);
        _streamWriter.WriteLine("JOIN #" + _channel);
        _streamWriter.Flush();
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

        // read messages
        while(_streamReader.Peek() >= 0)
        {
            Debug.Log(_streamReader.ReadLine());
        }
    }

    public void SendMessage(string a_message)
    {
        _streamWriter.WriteLine("PRIVMSG #" + _channel + " :" + a_message);
        _streamWriter.Flush();
    }
}
