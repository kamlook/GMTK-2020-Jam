using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public GameObject[] rooms;

    public float gameSpeed = 1;
    public GameObject smallRoomBasePrefab;
    public GameObject[] modulePrefabs;

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

    GameObject[] GenerateRooms(int a_seed, int roomCount) {
      GameObject[] gs = new GameObject[roomCount];
      System.Random _random = new System.Random(_seed);

      for (int i=0; i<roomCount; i++) {
        GameObject roomBase = Instantiate(smallRoomBasePrefab, new Vector3(0,0,0), Quaternion.identity);
        GameObject module;

        // Choose a random module prefab
        int module_index = (_random.Next(modulePrefabs.Length));

        module = Instantiate(modulePrefabs[module_index], new Vector3(0,0,0), Quaternion.identity);

        module.transform.parent = roomBase.transform;

        RoomManager rManager = roomBase.GetComponentInChildren<RoomManager>();
        RoomModule rModule = module.GetComponent<RoomModule>();
        rManager.modules.Add(rModule);
        rModule.roomManager = rManager;

        roomBase.SetActive(false);

        gs[i] = roomBase;
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
