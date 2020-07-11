using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameManager gameManager;
    public Transform spawnPoint;
    public ExitDoor exitDoor;
    public GameObject player;
    public List<RoomModule> modules;

    private int _roomNumber;
    protected System.Random _random;  // For whenever the room itself needs some randomness


    // Called by game manager to start the room
    public virtual void RoomStart(int roomNum, int seed, float roomSpeed) {
      gameManager = Object.FindObjectsOfType<GameManager>()[0];
      _roomNumber = roomNum;
      _random = new System.Random(seed);

      // Set up everything in this room so its public vars are pointing to the right place
      exitDoor.roomManager = this;
      player.GetComponent<Player>().roomManager = this;

      Debug.Log(modules);

      if (modules.Count > 0) {
        LockDoor();
      }

      SetRoomSpeed(roomSpeed);
    }
    public void Update() {
      // This is a temporary measure to check whether all modules are completed. For some reason it doesn't detect this on ModuleCompleted(), maybe because of prefabs?
      foreach (RoomModule m in modules) {
        if (!m.isCompleted) {
          Debug.Log(m);
          return;
        }
      }
      // If we've made it here, all buttons are pushed!
      UnlockDoor();
    }

    // Takes the game speed sent from gameManager, and applies it to all the relevant objects in the room.
    public void SetRoomSpeed(float a_speed) {
      Debug.Log(a_speed);
      player.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>().m_MoveSpeedMultiplier = a_speed;
      player.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>().m_AnimSpeedMultiplier = a_speed;
    }

    // Tell the game manager that this room is completed
    public void RoomCompleted() {
      gameManager.RoomCompleted();
    }

    public void LockDoor() {
      exitDoor.Lock();
    }

    public void UnlockDoor() {
      Debug.Log("UNLKC");
      exitDoor.Unlock();
    }

    public void Respawn(GameObject a_player) {
      a_player.transform.position = spawnPoint.transform.position;
    }

    public void ModuleCompleted() {
      Debug.Log("ModuleCompleted");
      foreach (RoomModule m in modules) {
        if (!m.isCompleted) {
          Debug.Log(m);
          return;
        }
      }
      // If we've made it here, all buttons are pushed!
      UnlockDoor();
    }
}
