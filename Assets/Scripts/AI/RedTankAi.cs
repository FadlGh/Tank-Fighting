using UnityEngine;

public class RedTankAi : TankAi
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
            if (Vector2.Distance(transform.position, playerRef.transform.position) < maxDistance)
            {
                returnFactor = -1f;
            }
            else
            {
                returnFactor = 1f;
            }

            if (playerRef != null)
                MoveToward(playerRef.transform.position);
            Shoot();
        }
    }
}