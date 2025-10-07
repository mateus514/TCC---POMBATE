using UnityEngine;
using UnityEngine.UI;

public class BulletController : MonoBehaviour
{
    [Header("HUD do Tambor")]
    public Image tamborImage;        // Arraste aqui o Image do Canvas
    public Sprite[] estadosTambor;  // Sprites do tambor (cheio -> vazio)
    
    
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
    
    public void ResetBalas()
    {
        // Volta a quantidade de balas para o máximo
        currentBullets = maxBullets;
        AtualizarHUD();
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
        AtualizarHUD();
        Destroy(bullet, 2f);
    }
    void AtualizarHUD()
    {
        if (tamborImage == null || estadosTambor.Length == 0)
            return;

        // Inverte o índice para que currentBullets = max → sprite cheio
        int index = estadosTambor.Length - 1 - Mathf.Clamp(currentBullets, 0, estadosTambor.Length - 1);
        tamborImage.sprite = estadosTambor[index];
    }
}