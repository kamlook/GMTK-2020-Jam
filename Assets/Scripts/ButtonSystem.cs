using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSystem : MonoBehaviour
{
    public Button[] buttons;
    public RoomManager roomManager;

    private int _buttonCount;
    // Start is called before the first frame update
    void Start()
    {
      roomManager = GameObject.FindWithTag("RoomManager").GetComponent<RoomManager>();

      roomManager.LockDoor();

      _buttonCount = this.transform.childCount;
      buttons = new Button[_buttonCount];

      int i = 0;
      foreach (Transform child in transform)
      {
        //child is your child transform
        buttons[i] = child.GetComponent<Button>();
        buttons[i].id = i;
        i++;
      }
    }

    void SendDoneSignal() {
      // Tell RoomManager that this button system is done
      roomManager.UnlockDoor();
    }

    public void RegisterPush(int id) {
      bool allButtonsArePushed = true;

      foreach (Button b in buttons) {
        if (!b.isPushed) {
          allButtonsArePushed = false;
          break;
        }
      }
      if (allButtonsArePushed) {
        SendDoneSignal();
      }
    }
}
