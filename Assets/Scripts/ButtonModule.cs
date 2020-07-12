using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonModule : RoomModule
{
    public int buttonCount;
    public GameObject buttonPrefab;

    public float timeLimit = 0;

    public Transform[] spawnPositions;


    private Button[] _buttons;
    private bool _allButtonsArePushed = false;
    private bool _timerStarted = false;
    private float _timer = 0;

    void Awake()
    {

    }

    public override void Load() {
      if (buttonCount > spawnPositions.Length) {
        buttonCount = spawnPositions.Length;
      }
      _buttons = new Button[buttonCount];

      List<int> selectedIndices = new List<int>();

      for (int i=0; i<buttonCount; i++) {
        // Spawn buttons at button spawn points
        int ind = roomManager.GetRandom(spawnPositions.Length);
        Debug.Log(selectedIndices);
        while (selectedIndices.Contains(ind)) {
          ind = roomManager.GetRandom(spawnPositions.Length);
        }
        selectedIndices.Add(ind);

        Debug.Log(ind);

        GameObject go = Instantiate(buttonPrefab, spawnPositions[ind]);

        go.transform.SetParent(transform);
        _buttons[i] = go.GetComponent<Button>();
      }
    }

    private IEnumerator Countdown() {
      _timer = 0;
      while ((_timer <= timeLimit) && !_allButtonsArePushed) {
        _timer += Time.deltaTime / timeLimit;

        yield return null;
      }
      if (!_allButtonsArePushed) {
        foreach (Button b in _buttons) {
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
      foreach (Button b in _buttons) {
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
