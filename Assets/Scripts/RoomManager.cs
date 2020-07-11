using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameManager gameManager;
    public Transform spawnPoint;
    public ExitDoor exitDoor;

    private int _roomNumber;
    private System.Random _random;  // For whenever the room itself needs some randomness


    // Called by game manager to start the room
    public void RoomStart(int roomNum, int seed) {
      _roomNumber = roomNum;
      _random = new System.Random(seed);
      //
      // Debug.Log("RANDOM: " + _random.Next(10).ToString() );
      // Debug.Log("RANDOM: " + _random.Next(10).ToString() );
      // Debug.Log("RANDOM: " + _random.Next(10).ToString() );
      // Debug.Log("RANDOM: " + _random.Next(10).ToString() );
    }

    public void RoomCompleted() {
      // Tell the game manager that this room is completed
      // Debug.Log("Room completed!" + _roomNumber);
      gameManager.RoomCompleted();

    }

    public void LockDoor() {
      Debug.Log("LOCK");
      exitDoor.Lock();
    }

    public void UnlockDoor() {
      exitDoor.Unlock();
    }
}
