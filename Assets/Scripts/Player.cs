﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public Animator anim;
    public GameObject punchHitbox;
    public RoomManager roomManager;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Attack() {
        anim.SetTrigger("Attack");

        // Spawn punch hitbox
        punchHitbox.SetActive(true);
    }

    // This function is triggered at the "hit" portion of the anim
    public void Hit() {
      // Debug.Log("YEA");

      // Check overlap of punch hitbox

      // Deactivate punch hitbox
      punchHitbox.SetActive(false);
    }

    public override void HandleCollision(Collider a_coll) {
      // Debug.Log(a_coll);
      a_coll.gameObject.GetComponent<Button>().Push();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            Attack();
        }
    }

    public void Die() {
        Debug.Log("Dead!");

        roomManager.Respawn(this.gameObject);
    }


}
