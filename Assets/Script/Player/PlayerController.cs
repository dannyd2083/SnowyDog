using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{

    BoxCollider2D playerCollider;

    public LayerMask collisionMask;

    const float playerSkinLayer = 0.015f;
    OriginRays originRays;
    public int horizontalRayCounts = 4;
    public int verticalRayCounts = 4;
    float horizontalRaySpacing;
    float verticalRaySpacing;


    // Start is called before the first frame update
    void Start()
    {
        playerCollider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }


    struct OriginRays
    {
        public Vector2 bottomLeft, bottomRight, topLeft, topRight;
    }




    // ref will make you change the original object, not just the copy of it.
    void DetectVerticalCollisions(ref Vector3 velocity)
    {

        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + playerSkinLayer;

        for (int i = 0; i < verticalRayCounts; i++)
        {
            // see which direction we moving to decide which initial rayorigin we are shooting the rays
            Vector2 rayOrigin = (directionY == -1) ? originRays.bottomLeft : originRays.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            // Origin, shooting direction, distance, layerMask (Filter to detect Colliders only on certain layers)
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.Log(hit);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - playerSkinLayer)*directionY;
                rayLength = hit.distance;
            }


         
        }
    }


    void DetectHorizontalCollisions(ref Vector3 velocity)
    {

        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + playerSkinLayer;

        for (int i = 0; i < horizontalRayCounts; i++)
        {
            // see which direction we moving to decide which initial rayorigin we are shooting the rays
            Vector2 rayOrigin = (directionX == -1) ? originRays.bottomLeft : originRays.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            // Origin, shooting direction, distance, layerMask (Filter to detect Colliders only on certain layers)
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);


            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);
            if (hit)
            {
                velocity.x = (hit.distance - playerSkinLayer) * directionX;
                rayLength = hit.distance;
            }


         
        }
    }



    public void Move(Vector3 velocity)
    {
        UpdateOriginRayPosition();

        if (velocity.x != 0) {
            DetectHorizontalCollisions(ref velocity);
        }

        if (velocity.y != 0) {
            DetectVerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }

    void CalculateRaySpacing()
    {
        Bounds playerBound = playerCollider.bounds;

        //Clamp if horizental is smaller then 2, get 2, larger than 4 then, 4, otherwise use it's original number
        horizontalRayCounts = Mathf.Clamp(horizontalRayCounts, 2, 4);
        verticalRayCounts = Mathf.Clamp(verticalRayCounts, 2, 4);

       
       
        horizontalRaySpacing = playerBound.size.y / (horizontalRayCounts - 1);
        verticalRaySpacing = playerBound.size.x / (verticalRayCounts - 1);
       
    }


    void UpdateOriginRayPosition()
    {
        Bounds playerBound = playerCollider.bounds;
        // We shrink the player collider's size (let the ray not directly shot from the palyer 
        // so when stands still the rays can be shot properly. (Expand use the extents of the box so we need -2)
        playerBound.Expand(playerSkinLayer * -2);
        originRays.bottomLeft = new Vector2(playerBound.min.x, playerBound.min.y);
        originRays.bottomRight = new Vector2(playerBound.max.x, playerBound.min.y);
        originRays.topLeft = new Vector2(playerBound.min.x, playerBound.max.y);
        originRays.topRight = new Vector2(playerBound.max.x, playerBound.max.y);

    }


}


   
