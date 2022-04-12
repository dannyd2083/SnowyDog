using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{

    PlayerController controller;
    // Start is called before the first frame update
    Vector3 velocity;
    float gravity = -15f;
    float moveSpeed = 6f;

    void Start()
    {
      
        controller = GetComponent<PlayerController>();
    }


    private void Update()
    {
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        velocity.x = playerInput.x * moveSpeed;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity* Time.deltaTime);
    }

}
