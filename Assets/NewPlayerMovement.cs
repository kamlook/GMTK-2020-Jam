using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{
    public Camera sceneCamera;
    public float speed = 8;
    private CharacterController _characterController;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
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
    }
}
