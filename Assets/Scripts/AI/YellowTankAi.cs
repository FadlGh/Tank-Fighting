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
        if (timeBetweenCounter > 0)
        {
            timeBetweenCounter -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        FollowPlayerLogic();
    }

    protected override void Shoot()
    {
        base.Shoot();
        if(bullet1 != null)
            bullet1.GetComponent<Bullet>().FollowPlayer(playerRef.transform.position);
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
