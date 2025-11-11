using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public SpriteRenderer characterRenderer, weaponRenderer;
    public Vector2 PointerPosition { get; set; }

    [Header("Crosshair")]
    public GameObject crosshairPrefab;
    private GameObject crosshairInstance;
    private Camera mainCam;

    [Header("Collision Check")]
    [SerializeField] private LayerMask wallLayer; // Layer dos blocos roxos
    [SerializeField] private float weaponLength = 1f; // Tamanho da arma
    
    private Collider2D weaponCollider;
    private Quaternion lastValidRotation;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        mainCam = Camera.main;

        weaponCollider = GetComponent<Collider2D>();
        lastValidRotation = transform.rotation;

        if (crosshairPrefab != null)
        {
            crosshairInstance = Instantiate(crosshairPrefab);
        }
        else
        {
            Debug.LogWarning("Crosshair prefab não foi atribuído no inspector!");
        }
    }

    void Update()
    {
        if (Time.timeScale == 0f)
            return;

        if (DialogueManager.Instance != null && DialogueManager.Instance.isDialogueActive)
            return;

        // --- Rotaciona a arma com verificação de colisão ---
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.right, direction);
        
        // Salva rotação atual
        Quaternion previousRotation = transform.rotation;
        
        // Aplica nova rotação temporariamente
        transform.rotation = targetRotation;
        
        // Verifica se a arma vai colidir com parede
        if (VerificarColisaoArma())
        {
            // Se vai colidir, volta para última rotação válida
            transform.rotation = lastValidRotation;
        }
        else
        {
            // Se não colide, salva como rotação válida
            lastValidRotation = transform.rotation;
        }

        // Flip Y da arma
        Vector2 scale = transform.localScale;
        if (direction.x < 0)
        {
            scale.y = -1;
        }
        else if (direction.x > 0)
        {
            scale.y = 1;
        }
        transform.localScale = scale;

        // Sorting order
        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }
        else
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        }

        // --- Atualiza posição da mira ---
        if (crosshairInstance != null && mainCam != null)
        {
            Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            crosshairInstance.transform.position = mouseWorldPos;
        }
    }

    private bool VerificarColisaoArma()
    {
        // Checa se a ponta da arma vai colidir com parede
        Vector2 weaponTip = (Vector2)transform.position + (Vector2)transform.right * weaponLength;
        
        RaycastHit2D hit = Physics2D.Linecast(transform.position, weaponTip, wallLayer);
        
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        // Visualiza a linha de checagem no editor
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Vector2 weaponTip = (Vector2)transform.position + (Vector2)transform.right * weaponLength;
            Gizmos.DrawLine(transform.position, weaponTip);
        }
    }
}