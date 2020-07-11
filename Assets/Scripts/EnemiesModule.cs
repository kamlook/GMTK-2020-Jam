using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesModule : RoomModule
{
    public Enemy[] enemies;

    private int _enemyCount;

    // Start is called before the first frame update
    void Start()
    {
      _enemyCount = this.transform.childCount;
      enemies = new Enemy[_enemyCount];

      int i = 0;
      foreach (Transform child in transform)
      {
        //child is your child transform
        enemies[i] = child.GetComponent<Enemy>();
        enemies[i].id = i;
        enemies[i].player = roomManager.player;
        i++;
      }
    }

    public void EnemyKilled(int id) {
      _enemyCount--;
      // If we've made it here, all buttons are pushed!
      if (_enemyCount <= 0) {
        ModuleCompleted();
      }
    }

    public void OnGUI() {
      if (_enemyCount > 0)
        GUI.Box(new Rect(10, 10, 150, 100), "Enemy Count: " + _enemyCount);
    }
}
