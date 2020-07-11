using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMazeRoomManager : RoomManager
{
    public Transform buttonSpawns;
    public int maxNumberOfButtonsSpawned = 4;
     public override void RoomStart(int roomNum, int seed, float roomSpeed) {
         base.RoomStart(roomNum, seed, roomSpeed);
         for (int i = 0; i < maxNumberOfButtonsSpawned; i++) {
            int nextSpawnPoint = _random.Next(buttonSpawns.childCount);
            // @TODO make all of the spawns active by default
            // @TODO and have them actually spawn buttons
            buttonSpawns.GetChild(nextSpawnPoint).gameObject.SetActive(true);
         }
     }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
