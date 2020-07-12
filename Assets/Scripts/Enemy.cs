using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public int id;
    public Transform[] waypoints;
    public float stoppingDistance = 1;
    public GameObject player;
    public float maxChaseDistance = 0;

    private int _currentWaypoint = 0;
    private Vector3 _spawnPosition;

    public enum AIBehaviors {
      Patrol,
      Chase,
      Flee
    }

    public AIBehaviors behavior = AIBehaviors.Patrol;

    // Start is called before the first frame update
    void Start()
    {
        if (waypoints.Length > 0 && behavior == AIBehaviors.Patrol) {
          GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().SetTarget(waypoints[_currentWaypoint]);
        }
        _spawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // PATROL
        if (waypoints.Length > 0 && behavior == AIBehaviors.Patrol) {
          if (Vector3.Distance(transform.position, waypoints[_currentWaypoint].position) < stoppingDistance) {
            _currentWaypoint++;
            if (_currentWaypoint >= waypoints.Length) {
              _currentWaypoint = 0;
            }

            GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().SetTarget(waypoints[_currentWaypoint]);
          }
        }

        // CHASE
        else if (behavior == AIBehaviors.Chase) {
          if (Vector3.Distance(transform.position, _spawnPosition) < maxChaseDistance &&
              Vector3.Distance(_spawnPosition, player.transform.position) < maxChaseDistance) {
            GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().SetTarget(player.transform);
          }
          else {
            GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().SetDestination(_spawnPosition);
          }

        }

        // FLEE
        else if (behavior == AIBehaviors.Flee) {
          Vector3 newPos;
          if (Vector3.Distance(transform.position, player.transform.position) > maxChaseDistance) {
            float farthestDist = 0;
            Transform dest = waypoints[0];
            foreach (Transform w in waypoints) {
              float dist = Vector3.Distance(w.position, player.transform.position);
              if (dist > farthestDist) {
                dest = w;
                farthestDist = dist;
              }
            }
            newPos = dest.position;
          }
          else
          {
            Vector3 dirToPlayer = transform.position - player.transform.position;
            newPos = transform.position + dirToPlayer;
          }

          GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().SetDestination(newPos);
        }

        base.OutOfBoundsCheck();
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
