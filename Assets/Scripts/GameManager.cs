using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public GameObject[] rooms;

    public float gameSpeed = 1;

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
      rooms[_currentRoom].GetComponentInChildren<RoomManager>().RoomStart(_currentRoom, _seed + _currentRoom, gameSpeed);
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.F)) {
        IncreaseGameSpeed(0.1f);
      }
    }

    public void IncreaseGameSpeed(float a_amount) {
      gameSpeed += a_amount;

      Debug.Log("GAME SPEED = " + gameSpeed);

      rooms[_currentRoom].GetComponentInChildren<RoomManager>().SetRoomSpeed(gameSpeed);
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
        rooms[_currentRoom].GetComponentInChildren<RoomManager>().RoomStart(_currentRoom, _seed + _currentRoom, gameSpeed);
      }
    }
}
