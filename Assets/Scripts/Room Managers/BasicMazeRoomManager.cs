using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasicMazeRoomManager : RoomManager
{
    public Transform buttonModule;
    public Transform buttonSpawns;
    private int[] _buttonSpawnPositions;

    public override void RoomStart(int roomNum, int seed, float roomSpeed) {
        base.RoomStart(roomNum, seed, roomSpeed);
        int numButtons = buttonModule.childCount;
        _buttonSpawnPositions = new int[4];
        for (int i = 0; i < numButtons; i++) {
            // check if there's already a button at this spot
            int nextSpawnPoint = _random.Next(buttonSpawns.childCount);
            if (Array.IndexOf(_buttonSpawnPositions, nextSpawnPoint) != -1) {
                i--;
                continue;
            } else {
                // place the button
                buttonModule.GetChild(i).position = buttonSpawns.GetChild(nextSpawnPoint).position;
                _buttonSpawnPositions[i] = nextSpawnPoint;
            }
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
