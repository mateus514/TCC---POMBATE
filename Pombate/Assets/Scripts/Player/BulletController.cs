using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject bulletPrefab;   // Arraste o prefab aqui no inspetor
    public Transform firePoint;       // Ponto de origem do tiro
    public float bulletForce = 20f;   // Velocidade da bala

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Botão esquerdo do mouse
        {
            Shoot();
        }
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
        Destroy(bullet, 2f);
    }
}