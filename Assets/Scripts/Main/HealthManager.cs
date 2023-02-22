using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float maxHealth = 100f;
    public ParticleSystem ps;
    public SpriteRenderer sp;
    private float health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 0)
        {
            ParticleSystem effect = Instantiate(ps, transform.position, Quaternion.identity);
            var main = effect.GetComponent<ParticleSystem>().main;
            main.startColor = sp.color;
            Destroy(gameObject);
        }
    }

    public void ApplyDamage(float damage)
    {
        health -= damage;
    }
}
