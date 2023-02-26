using System.Collections;
using UnityEngine;

public class RedTankAi : TankAi
{
    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        timeBetweenCounter = timeBetween;
    }

    void Update()
    {
        FieldOfViewCheck();
    }

    void FixedUpdate()
    {
        FollowPlayerLogic();
    }
}