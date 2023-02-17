using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float shootSpeed;
    public ParticleSystem ps;

    void Start()
    {
        StartCoroutine(timer());
    }
    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * shootSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tank AI"))
        {
            collision.GetComponent<FieldOfView>().onCollisionFollowCall();
        }
        die();
    }

    void die()
    {
        Instantiate(ps, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(2f);
        die();
    }
}
