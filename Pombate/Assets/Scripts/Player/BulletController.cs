using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletForce = 20f;
    
    [Header("Balas")]
    [SerializeField] private int maxBullets = 6;
    private int currentBullets;

    [Header("Recuo do Player")]
    public Rigidbody2D playerRb;     // arraste o Rigidbody2D do player no Inspector
    public float recoilForce = 5f;   // intensidade do recuo

    void Start()
    {
        currentBullets = maxBullets;
    }

    void Update()
    {
        bool emDialogo = DialogueManager.Instance != null && DialogueManager.Instance.isDialogueActive;

        if (emDialogo)
            return;

        if (Input.GetButtonDown("Fire1") && currentBullets > 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (Time.timeScale == 0f)
    return;

        // Calcula a direção baseada no firePoint
        Vector2 shootDirection = firePoint.right.normalized;

        // Instancia a bala
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(shootDirection * bulletForce, ForceMode2D.Impulse);

        // Aplica recuo no player (oposto ao tiro)
        if (playerRb != null)
        {
            playerRb.AddForce(-shootDirection * recoilForce, ForceMode2D.Impulse);
        }

        currentBullets--;
        Destroy(bullet, 2f);
    }
}