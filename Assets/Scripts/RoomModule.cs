using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomModule : MonoBehaviour
{
    public RoomManager roomManager;

    public bool isCompleted = false;

    public void ModuleCompleted() {
      isCompleted = true;

      roomManager.ModuleCompleted();
    }
}
