using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrownTankAi : TankAi
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
        if (timeBetweenCounter > 0)
        {
            timeBetweenCounter -= Time.deltaTime;
            return;
        }

        timeBetweenCounter = timeBetween;

        StartCoroutine(shootMultipleTimes());
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

    IEnumerator shootMultipleTimes()
    {
        yield return new WaitForSeconds(1f);

        Instantiate(bullet, shootPoint.position, transform.rotation * bulletOffset);

        yield return new WaitForSeconds(1f);

        Instantiate(bullet, shootPoint.position, transform.rotation * bulletOffset);

        yield return new WaitForSeconds(1f);
    }
}
