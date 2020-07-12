﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public Animator anim;
    public GameObject punchHitbox;
    public RoomManager roomManager;
    public Camera sceneCamera;

    public float speed = 8;

    private CharacterController _characterController;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = sceneCamera.transform.right * x + sceneCamera.transform.forward * z;
        move.y = 0;
        move = move * speed * Time.deltaTime * _gameManager.gameSpeed;

        _characterController.Move(move);

        if (Input.GetKeyDown(KeyCode.E)) {
            Attack();
        }

        base.OutOfBoundsCheck();
    }

    void Attack() {
        anim.SetTrigger("Attack");

        // Spawn punch hitbox
        punchHitbox.SetActive(true);
    }

    // This function is triggered at the "hit" portion of the anim
    public void Hit() {
      // Deactivate punch hitbox
      punchHitbox.SetActive(false);
    }

    public override void HandleCollision(Collider a_coll) {
      if (a_coll.gameObject.TryGetComponent(out Attackable atk)) {
        atk.ProcessAttack();
      }
    }

    public override void Die() {
        Debug.Log("Dead!");

        roomManager.Respawn(this.gameObject);
    }
}
