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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<HealthManager>() != null)
            collision.gameObject.GetComponent<HealthManager>().ApplyDamage(25f);
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
