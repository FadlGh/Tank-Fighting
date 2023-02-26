using System.Collections;
using UnityEngine;

public class TankAi : MonoBehaviour
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
    protected bool canShoot;
    protected float timeBetweenCounter;
    protected float timeBetween = 1f;

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

    protected Rigidbody2D rb;
    protected bool canSeePlayer;
    protected float returnFactor = 1f;
    protected bool collided;

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

    protected void FieldOfViewCheck()
    {
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.up, directionToTarget) < angle / 2)
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

    protected void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);

        rb.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    protected void MoveToward(Vector3 target)
    {
        Vector2 directionToTarget = (target - transform.position).normalized;
        float rotateAmount = Vector3.Cross(directionToTarget, transform.up).z;
        float minSpeedBeforeAllowTurningFactor = (rb.velocity.magnitude / 3);

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

    protected void FollowPlayerLogic()
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
        }
    }

    public virtual void Shoot()
    {
        if (timeBetweenCounter > 0)
        {
            timeBetweenCounter -= Time.deltaTime;
            return;
        }

        Instantiate(bullet, shootPoint.position, transform.rotation * bulletOffset);
        timeBetweenCounter = timeBetween;
    }
    protected void OnCollisionFollowCall()
    {
        StartCoroutine(OnCollisionFollow());
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != 3)
        {
            StartCoroutine(OnCollisionFollow());
        }
        else
            print("ai");
    }

    IEnumerator OnCollisionFollow()
    {
        collided = true;

        yield return new WaitForSeconds(2f);

        collided = false;
    }
}