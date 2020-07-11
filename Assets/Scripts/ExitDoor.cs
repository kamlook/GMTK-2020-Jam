using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    public RoomManager roomManager;
    public bool isLocked = false;


    void OnTriggerEnter() {
      if (!isLocked)
        roomManager.RoomCompleted();
    }

    public void Lock() {
      isLocked = true;

      GetComponent<Renderer>().material.SetColor("_Color", Color.black);
    }

    public void Unlock() {
      isLocked = false;

      GetComponent<Renderer>().material.SetColor("_Color", Color.white);
    }
}
