using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float shootSpeed;
    public ParticleSystem ps;
    private Rigidbody2D rb;

    void Start()
    {
        StartCoroutine(timer());
        rb = GetComponent<Rigidbody2D>();
        FindObjectOfType<AudioManager>().Play("fire");
    }
    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.up * shootSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        FindObjectOfType<AudioManager>().Play("explosion");
        if (collision.gameObject.GetComponent<HealthManager>() != null)
            collision.gameObject.GetComponent<HealthManager>().ApplyDamage(25f);
        die();
    }

    void die()
    {
        Instantiate(ps, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void FollowPlayer(Vector3 target)
    {
        shootSpeed = 1f;

        Vector2 directionToTarget = (target - transform.position).normalized;
        float rotateAmount = Vector3.Cross(directionToTarget, transform.up).z;
        if (rb != null)
        {
            rb.angularVelocity = -rotateAmount * 500f;
            rb.velocity = transform.up * shootSpeed;
        }
    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(2f);
        die();
    }
}
