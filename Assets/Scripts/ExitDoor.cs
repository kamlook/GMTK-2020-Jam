using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    public RoomManager roomManager;
    // Start is called before the first frame update
    void OnTriggerEnter() {
      roomManager.RoomCompleted();
    }
}
