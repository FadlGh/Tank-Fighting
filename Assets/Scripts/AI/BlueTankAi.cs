using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueTankAi : TankAi
{
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        timeBetweenCounter = timeBetween;
        StartCoroutine(SetDestination());
    }

    void Update()
    {
        FieldOfViewCheck();
        if (timeBetweenCounter > 0)
        {
            timeBetweenCounter -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        FollowPlayerLogic();
    }

    protected override void FollowPlayerLogic()
    {
        base.FollowPlayerLogic();
        if (canSeePlayer)
        {
            if(playerRef != null)
                RotateToward(playerRef.transform.position);
            Shoot();
        }
    }
}