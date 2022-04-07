using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{

    BoxCollider2D playerCollider;
    const float playerSkinLayer = 0.015f;
    OriginRays originRays;
    public int horizentalRayCounts = 4;
    public int verticalRayCounts = 4;
    public float horizentalRaySpacing;
    public float verticalRaySpacing;


    // Start is called before the first frame update
    void Start()
    {
        playerCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        UpdateOriginRayPosition();
        CalculateRaySpacing();
        for (int i = 0; i < verticalRayCounts; i++)
        {
            Debug.DrawRay(originRays.bottomLeft + (Vector2.right * verticalRaySpacing * i), Vector2.up * -2, Color.red);
        }
    }


    struct OriginRays
    {
        public Vector2 bottomLeft, bottomRight, topLeft, topRight;
    }


    void CalculateRaySpacing()
    {
        Bounds playerBound = playerCollider.bounds;

        //Clamp if horizental is smaller then 2, get 2, larger than 4 then, 4, otherwise use it's original number
        horizentalRayCounts = Mathf.Clamp(horizentalRayCounts, 2, 4);
        verticalRayCounts = Mathf.Clamp(verticalRayCounts, 2, 4);

       
       
        horizentalRaySpacing = playerBound.size.y / (horizentalRayCounts - 1);
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


   
