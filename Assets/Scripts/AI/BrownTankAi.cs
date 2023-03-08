using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrownTankAi : TankAi
{
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
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
        if (timeBetweenCounter > 0)
            return;

        print('s');
        bullet1 = Instantiate(bullet, shootPoint.position, transform.rotation * bulletOffset);
        StartCoroutine(shootMultipleTimes());
        timeBetweenCounter = timeBetween;
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
    }
}
