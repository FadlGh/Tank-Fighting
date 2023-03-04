using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowTankAi : TankAi
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
    }

    void FixedUpdate()
    {
        FollowPlayerLogic();
    }

    protected override void Shoot()
    {
        base.Shoot();
        if(bullet1 != null)
            bullet1.GetComponent<bullet>().FollowPlayer(playerRef.transform.position);
    }

    protected override void FollowPlayerLogic()
    {
        base.FollowPlayerLogic();
        if (canSeePlayer)
        {
            RotateToward(playerRef.transform.position);
            Shoot();
        }
    }
}
