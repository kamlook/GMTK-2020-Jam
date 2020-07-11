using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public int id;
    public Transform[] waypoints;
    public float stoppingDistance = 1;

    private int _currentWaypoint = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (waypoints.Length > 0) {
          GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().SetTarget(waypoints[_currentWaypoint]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (waypoints.Length > 0) {
          if (Vector3.Distance(transform.position, waypoints[_currentWaypoint].position) < stoppingDistance) {
            _currentWaypoint++;
            if (_currentWaypoint >= waypoints.Length) {
              _currentWaypoint = 0;
            }

            GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().SetTarget(waypoints[_currentWaypoint]);
          }
        }
    }

    public override void HandleCollision(Collider c) {
      if (c.tag == "Player")
        c.gameObject.GetComponent<Player>().Die();
    }

    public override void Die() {
      try {
        GetComponentInParent<EnemiesModule>().EnemyKilled(id);
        Destroy(this.gameObject);
      }
      catch {
        Debug.Log("This enemy is invincible, because it isn't attached to an EnemiesModule.");
      }
    }
}
