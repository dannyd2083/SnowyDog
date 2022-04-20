using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{

    PlayerController controller;
    // Start is called before the first frame update
    Vector3 velocity;
   
    float moveSpeed = 6f;

    
  
   public float jumpHeight = 4f;
   public float timeToJumpApex =0.4f;
    // s = v_i * t + 1/2 * a * t^2
    // jumpHeight = (gravity * timeToJumpApex^2)/2
    float gravity;
    // v_f = v_i + a *t
    float jumpVelocity;
    float velocityXSmoothing;

    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;


    void Start()
    {
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
        controller = GetComponent<PlayerController>();
    }


    private void Update()
    {

        if (controller.collisionsMessage.top || controller.collisionsMessage.bottom)
        {
            velocity.y = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && controller.collisionsMessage.bottom)
        {
            velocity.y = jumpVelocity;
        }
      
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        velocity.x = Mathf.SmoothDamp(velocity.x, playerInput.x * moveSpeed, ref velocityXSmoothing, (controller.collisionsMessage.bottom) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity* Time.deltaTime);
    }

}
