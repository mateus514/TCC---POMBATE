using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject laserPrefab; // Prefab do feixe (Line Renderer)
    public Transform firePoint;
    public float bulletForce = 20f;

    [SerializeField] private int maxBullets = 6; // limite ajustável no Inspector
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
        // Cria a bala e aplica força
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
        currentBullets++;

        Destroy(bullet, 2f);
        StartCoroutine(DecreaseBulletCountAfterDelay(2f));

        // Cria o feixe laser
        StartCoroutine(ShootLaser());
    }

    private IEnumerator DecreaseBulletCountAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentBullets--;
    }

    private IEnumerator ShootLaser()
    {
        // Instancia o feixe na posição do firePoint
        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
        LineRenderer line = laser.GetComponent<LineRenderer>();

        // Define os pontos do feixe (começo e fim)
        line.SetPosition(0, firePoint.position);
        line.SetPosition(1, firePoint.position + firePoint.right * 2f); // Ajuste o comprimento aqui

        // Espera um pouco para o feixe aparecer (duração curta)
        yield return new WaitForSeconds(0.1f);

        // Destrói o feixe
        Destroy(laser);
    }
}