using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletForce = 20f;

    [SerializeField] private int maxBullets = 6; // Ajustável no Inspector
    private int currentBullets;

    void Start()
    {
        currentBullets = maxBullets; // Começa com todas as balas
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && currentBullets > 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);

        currentBullets--;
        Destroy(bullet, 2f);
    }
}