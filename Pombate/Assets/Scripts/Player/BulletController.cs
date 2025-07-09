using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletForce = 20f;

    [SerializeField] private int maxBullets = 6;  // limite ajustável no Inspector
    private int currentBullets = 0;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && currentBullets < maxBullets)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
        currentBullets++;

        Destroy(bullet, 2f);
        StartCoroutine(DecreaseBulletCountAfterDelay(2f));
    }

    private IEnumerator DecreaseBulletCountAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentBullets--;
    }
}