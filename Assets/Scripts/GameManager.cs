using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public GameObject[] rooms;

    public float gameSpeed = 1;
    // public GameObject smallRoomBasePrefab;
    public GameObject[] roomTemplates;

    private int _currentRoom = 0;
    private int _seed;
    public string _seedString = "";

    // Start is called before the first frame update
    void Start()
    {
      // Set up random seed
      if (_seedString == "") {
        _seed = System.DateTime.Now.GetHashCode();
      }
      else {
        _seed = _seedString.GetHashCode();
      }

      // Generate rooms
      rooms = GenerateRooms(_seed, 10);

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

    // Generate an array of rooms
    GameObject[] GenerateRooms(int a_seed, int roomCount) {
      GameObject[] gs = new GameObject[roomCount];
      System.Random _random = new System.Random(_seed);

      for (int i=0; i<roomCount; i++) {
        // Choose a random template
        int rand_index = _random.Next(roomTemplates.Length);

        // Instantiate that template
        GameObject rmTemplate = Instantiate(roomTemplates[rand_index], new Vector3(0,0,0), Quaternion.identity);
        RoomManager rmManager = rmTemplate.GetComponentInChildren<RoomManager>();

        // How many modules should we deactivate (based on difficulty)? (eventually this should be replaced by a config file, specifying allowable combos)
        int numModules = i;
        if (numModules > rmManager.modules.Count) {
          numModules = rmManager.modules.Count;
        }
        int numModulesToDeactivate = rmManager.modules.Count - numModules;

        // Activate room modules
        for (int j=0; j<numModulesToDeactivate; j++) {
          // Choose a random module prefab
          int module_index = _random.Next(rmManager.modules.Count);

          rmManager.modules[module_index].gameObject.SetActive(false);

          rmManager.modules.RemoveAt(module_index);
        }

        rmTemplate.SetActive(false);
        gs[i] = rmTemplate;
      }
      return gs;
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
