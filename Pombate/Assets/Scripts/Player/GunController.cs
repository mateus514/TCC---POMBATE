using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public SpriteRenderer characterRenderer, weaponRenderer;
    public Vector2 PointerPosition { get; set; }

    [Header("Crosshair")]
    public GameObject crosshairPrefab; // arrasta o prefab da mira aqui
    private GameObject crosshairInstance;
    private Camera mainCam;

    void Start()
    {
        Cursor.visible = false;    // Esconde o cursor
        Cursor.lockState = CursorLockMode.None; // Pode deixar desbloqueado para mover livremente
        mainCam = Camera.main;

        // Instancia a mira no início
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
        // Se o jogo estiver pausado, não faz nada
        if (Time.timeScale == 0f)
            return;

        // Se estiver em diálogo, também não faz nada
        if (DialogueManager.Instance != null && DialogueManager.Instance.isDialogueActive)
            return;

        // --- Rotaciona a arma em direção ao mouse ---
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

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
            mouseWorldPos.z = 0f; // trava no plano 2D
            crosshairInstance.transform.position = mouseWorldPos;
        }
    }
}
