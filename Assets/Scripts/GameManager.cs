using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public GameObject[] rooms;

    public float gameSpeed = 1.5f;

    private int _currentRoom = 0;
    private int _seed;
    private string _seedString = "";

    // Start is called before the first frame update
    void Start()
    {
      if (_seedString == "") {
        _seed = System.DateTime.Now.GetHashCode();
      }
      else {
        _seed = _seedString.GetHashCode();
      }
      Debug.Log(_seed);

      rooms[_currentRoom].SetActive(true);
      rooms[_currentRoom].GetComponentInChildren<RoomManager>().RoomStart(_currentRoom, _seed + _currentRoom);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RoomCompleted() {
      // Load the new room and call Room Start on it
      rooms[_currentRoom].SetActive(false);

      _currentRoom++;

      if (_currentRoom >= rooms.Length) {
        // Win!
        Debug.Log("You're a winner");
      }
      else {
        rooms[_currentRoom].SetActive(true);
        rooms[_currentRoom].GetComponentInChildren<RoomManager>().RoomStart(_currentRoom, _seed + _currentRoom);
      }
    }
}
