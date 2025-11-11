using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private bool bloqueado = false;
    public bool jogoPausado = false;
    public bool gameOver = false;

    private float horizontalInput;
    private Rigidbody2D rb;

    [SerializeField] public int velocidade = 5;
    [SerializeField] public float forcaPulo = 600f;

    [SerializeField] private Transform peDoPersonagem;
    [SerializeField] private LayerMask chaoLayer; // Agora pode ter múltiplas layers!

    [SerializeField] private InputActionReference pointerPosition;

    private Vector2 pointerInput;
    private bool estaNoChao;

    private Animator animator;
    private WeaponParent weaponParent;

    private enum State { idle, running, jump }
    private State state = State.idle;

    private bool olhandoParaDireita = true;

    [Header("Sprite Renderer (do objeto visual)")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weaponParent = GetComponentInChildren<WeaponParent>();
        animator = GetComponentInChildren<Animator>();
        
        // Garante detecção contínua para evitar atravessar blocos
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Start()
    {
        animator.SetInteger("state", (int)state);
        Respawn();
    }

    void Update()
    {
        if (bloqueado) return;
        bool emDialogo = DialogueManager.Instance != null && DialogueManager.Instance.isDialogueActive;

        if (emDialogo)
        {
            horizontalInput = 0;
            weaponParent.PointerPosition = transform.position;
            SetState(State.idle);
            return;
        }

        pointerInput = GetPointerInput();
        weaponParent.PointerPosition = pointerInput;

        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            rb.AddForce(Vector2.up * forcaPulo);
        }

        // Verifica se está no chão (agora inclui múltiplas layers)
        estaNoChao = Physics2D.OverlapCircle(peDoPersonagem.position, 0.2f, chaoLayer);

        StateChange();

        // Flip apenas do sprite visual
        if (horizontalInput > 0 && !olhandoParaDireita)
        {
            Virar();
        }
        else if (horizontalInput < 0 && olhandoParaDireita)
        {
            Virar();
        }
    }

    void FixedUpdate()
    {
        if (bloqueado)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        
        bool emDialogo = DialogueManager.Instance != null && DialogueManager.Instance.isDialogueActive;

        if (emDialogo)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(horizontalInput * velocidade, rb.linearVelocity.y);
    }

    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = pointerPosition != null
            ? pointerPosition.action.ReadValue<Vector2>()
            : Mouse.current.position.ReadValue();

        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void StateChange()
    {
        if (!estaNoChao)
        {
            SetState(State.jump);
        }
        else if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
        {
            SetState(State.running);
        }
        else
        {
            SetState(State.idle);
        }
    }

    public void ResetarEstado()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    private void SetState(State novoEstado)
    {
        state = novoEstado;
        animator.SetInteger("state", (int)state);
    }

    public void Morrer()
    {
        Scene sceneAtual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(sceneAtual.name);
    }

    private void Virar()
    {
        olhandoParaDireita = !olhandoParaDireita;
        spriteRenderer.flipX = !olhandoParaDireita;
    }

    public void Respawn()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        bloqueado = true;
        animator.SetTrigger("Respawn");
    }
    
    public void DesbloquearMovimento()
    {
        bloqueado = false;
    }
}