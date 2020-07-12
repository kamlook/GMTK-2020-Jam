using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomModule : MonoBehaviour
{
    public RoomManager roomManager;

    public bool isCompleted = false;

    public virtual void Load() {

    }

    public void ModuleCompleted() {
      isCompleted = true;
      Debug.Log("COMPLETE");

      roomManager.ModuleCompleted();
    }
}
