using System.Collections;
using UnityEngine;

public class RedTankAi : MonoBehaviour
{
    [Header("Detection Settings")]
    [Range(0, 360)]
    public float angle;
    public float radius;
    public float maxDistance;

    [Header("Shoot Settings")]
    public GameObject bullet;
    public Transform shootPoint;
    public Quaternion bulletOffset;
    private bool canShoot;
    private float timeBetweenCounter;
    private float timeBetween = 1f;

    [Header("Wandering Settings")]
    public float range;
    public float maxPosition;
    private Vector3 wayPoint;

    [Header("Layers")]
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    [Header("Speeds")]
    public float accelerationFactor;
    public float rotationFactor;
    public float driftFactor;
    public float maxSpeed;

    [Header("Player")]
    public GameObject playerRef;

    private Rigidbody2D rb;
    private bool canSeePlayer;
    private float returnFactor = 1f;
    private bool collided;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        timeBetweenCounter = timeBetween;
        SetNewDestination();
    }

    void Update()
    {
        FieldOfViewCheck();
    }

    void FixedUpdate()
    {
        FollowPlayerLogic();
    }

    private void FieldOfViewCheck()
    {
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if (Mathf.Abs(Vector2.Angle(transform.up, directionToTarget)) < angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    if (!canSeePlayer)
                        timeBetweenCounter = timeBetween;
                    canSeePlayer = true;
                }
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }

    void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);

        rb.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    void MoveToward(Vector3 target)
    {
        Vector2 directionToTarget = (target - transform.position).normalized;
        float rotateAmount = Vector3.Cross(directionToTarget, transform.up).z;
        float minSpeedBeforeAllowTurningFactor = (rb.velocity.magnitude / 8);

        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        //Calculate how much "forward" we are going in terms of direction of our velocity
        float velocityVsUp = Vector2.Dot(transform.up, rb.velocity);

        if (velocityVsUp > maxSpeed)
            return;

        rb.angularVelocity = -rotateAmount * rotationFactor * minSpeedBeforeAllowTurningFactor;
        rb.AddForce(transform.up * accelerationFactor * returnFactor, ForceMode2D.Force);
        rb.drag = 0;

        KillOrthogonalVelocity();
    }

    void FollowPlayerLogic()
    {
        if (!canSeePlayer && collided)
        {
            canSeePlayer = true;
        }

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

            MoveToward(playerRef.transform.position);
            Shoot();
        }
        else
        {
            rb.drag = Mathf.Lerp(rb.drag, 5.0f, Time.fixedDeltaTime * 3f);
            Wandering();
        }
    }

    void Shoot()
    {
        if (timeBetweenCounter < 0)
        {
            canShoot = true;
        }
        else
        {
            canShoot = false;
            timeBetweenCounter -= Time.deltaTime;
        }

        float direction()
        {
            if (playerRef.GetComponent<PlayerMovement>().inputVector.x > 0)
                return 1;
            else
                return -1;
        }
        if (canShoot)
        {
            Instantiate(bullet, shootPoint.position, transform.rotation * new Quaternion(bulletOffset.x,
                                                                                         bulletOffset.y,
                                                                                         bulletOffset.z * direction(),
                                                                                         bulletOffset.w));
            timeBetweenCounter = timeBetween;
        }
    }

    void Wandering()
    {
        if(Vector2.Distance(transform.position, wayPoint) < range)
            SetNewDestination();
        MoveToward(wayPoint);
    }

    void SetNewDestination()
    {
        wayPoint = new Vector3(Random.Range(-maxPosition, maxPosition), Random.Range(-maxPosition, maxPosition));
    }
    
    public void OnCollisionFollowCall()
    {
        StartCoroutine(OnCollisionFollow());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SetNewDestination();
        if (collision.gameObject.layer != 3)
        {
            StartCoroutine(OnCollisionFollow());
        }
        else
        {
            SetNewDestination();
        }
    }

    IEnumerator OnCollisionFollow()
    {
        collided = true;

        yield return new WaitForSeconds(1f);

        collided = false;
    }
}