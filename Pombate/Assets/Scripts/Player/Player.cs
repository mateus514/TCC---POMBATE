using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private bool bloqueado = false;
    public ParticleSystem dust;

    private float horizontalInput;
    private Rigidbody2D rb;

    [SerializeField] public int velocidade = 5;
    [SerializeField] public float forcaPulo = 600f;

    [SerializeField] private Transform peDoPersonagem;
    [SerializeField] private LayerMask chaoLayer;

    private bool estaNoChao;

    private Animator animator;
    private WeaponParent weaponParent;

    private enum State { idle, running, jump }
    private State state = State.idle;

    private bool olhandoParaDireita = true;

    [Header("Sprite Renderer (do objeto visual)")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Sons do Jogador")]
    public SomDoJogador somDoJogador; // arraste aqui o objeto SomDoJogador

    private Vector3 posicaoInicial;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weaponParent = GetComponentInChildren<WeaponParent>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        posicaoInicial = transform.position; // salva spawn inicial
        animator.SetInteger("state", (int)state);
        Respawn();
    }

    void Update()
    {
        if (bloqueado) return;

        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Atualiza detecção de chão
        estaNoChao = Physics2D.OverlapCircle(peDoPersonagem.position, 0.2f, chaoLayer);

        // Pulo
        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            rb.AddForce(Vector2.up * forcaPulo);

            // 🔊 Som do pulo
            if (somDoJogador != null)
                somDoJogador.TocarSomPulo();
        }

        // Atualiza posição do mouse para arma
        if (weaponParent != null)
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            weaponParent.PointerPosition = worldPos;
        }

        // 🔹 Controle do som de passos
        ControlarSomPassos();

        StateChange();

        // Flip sprite
        if (horizontalInput > 0 && !olhandoParaDireita) Virar();
        else if (horizontalInput < 0 && olhandoParaDireita) Virar();
    }

    void FixedUpdate()
    {
        if (bloqueado)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        rb.velocity = new Vector2(horizontalInput * velocidade, rb.velocity.y);
    }

    private void StateChange()
    {
        if (!estaNoChao)
        {
            SetState(State.jump);
            dust.Stop();
        }
        else if (Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            SetState(State.running);
            if (!dust.isPlaying) dust.Play();
        }
        else
        {
            SetState(State.idle);
            dust.Stop();
        }
    }

    private void SetState(State novoEstado)
    {
        state = novoEstado;
        animator.SetInteger("state", (int)state);
    }

    private void Virar()
    {
        olhandoParaDireita = !olhandoParaDireita;
        spriteRenderer.flipX = !olhandoParaDireita;
    }

    public void ResetarEstado()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    public void Respawn()
    {
        ResetarEstado();
        bloqueado = true;
        animator.SetTrigger("Respawn");
    }
    public void BloquearMovimento()
    {
        bloqueado = true;
        rb.velocity = Vector2.zero;

        // força idle
        state = State.idle;
        animator.SetInteger("state", (int)State.idle);

        // pausa o animador
        animator.speed = 0f;

        // PARA O SOM DE PASSOS IMEDIATAMENTE
        if (somDoJogador != null && somDoJogador.audioSourcePassos.isPlaying)
            somDoJogador.audioSourcePassos.Stop();
    }

    public void DesbloquearMovimento()
    {
        bloqueado = false;

        // destrava o Animator
        animator.speed = 1f;
    }

    public void VoltarParaSpawn()
    {
        if (somDoJogador != null)
            somDoJogador.TocarSomMorte();

        ResetarEstado();
        transform.position = posicaoInicial;
    }

    // =========================
    // 🔹 Novo método: passos
    // =========================
    private void ControlarSomPassos()
    {
        if (somDoJogador == null) return;

        bool andando = estaNoChao && Mathf.Abs(horizontalInput) > 0.1f;

        if (andando)
        {
            if (!somDoJogador.audioSourcePassos.isPlaying)
                somDoJogador.audioSourcePassos.Play();
        }
        else
        {
            if (somDoJogador.audioSourcePassos.isPlaying)
                somDoJogador.audioSourcePassos.Stop();
        }
    }
}
