using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    void OnTriggerEnter(Collider c) {
        this.transform.parent.GetComponent<Player>().PunchCollision(c);
    }
}
