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
        if (collision.gameObject.layer == 7)
        {
            print("s");
            collision.gameObject.GetComponent<HealthManager>().ApplyDamage(25f);
            if (collision.tag == "Red")
                collision.GetComponent<RedTankAi>().OnCollisionFollowCall();
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
