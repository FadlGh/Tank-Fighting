using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bullet;
    public Transform shootPoint;
    private float timeBetweenCounter;
    private float timeBetween = 1f;

    void Update()
    {
        if (timeBetweenCounter > 0)
        {
            timeBetweenCounter -= Time.deltaTime;
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(bullet, shootPoint.position, transform.rotation);
            timeBetweenCounter = timeBetween;
        }
    }
}
