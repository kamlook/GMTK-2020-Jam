using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonModule : RoomModule
{
    public Button[] buttons;

    public float timeLimit = 0;

    public Transform[] spawnPositions;

    private int _buttonCount;
    private bool _allButtonsArePushed = false;
    private bool _timerStarted = false;
    private float _timer = 0;

    // Start is called before the first frame update
    void Start()
    {
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

    private IEnumerator Countdown() {
      _timer = 0;
      while ((_timer <= timeLimit) && !_allButtonsArePushed) {
        _timer += Time.deltaTime / timeLimit;

        yield return null;
      }
      if (!_allButtonsArePushed) {
        foreach (Button b in buttons) {
          b.UnPush();
        }
      }
      _timerStarted = false;
    }

    public void RegisterPush(int id) {
      if (timeLimit > 0 && !_timerStarted) {
        StartCoroutine(Countdown());
        _timerStarted = true;
      }
      foreach (Button b in buttons) {
        if (!b.isPushed) {
          return;
        }
      }
      // If we've made it here, all buttons are pushed!
      _allButtonsArePushed = true;

      ModuleCompleted();
    }

    public void OnGUI() {
      if (_timerStarted)
        GUI.Box(new Rect(10, 10, 150, 100), "Timer: " + (timeLimit - System.Math.Round(_timer, 2)) + " / " + timeLimit);
    }
}
