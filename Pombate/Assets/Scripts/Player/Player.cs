using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private float horizontalInput;
    private Rigidbody2D rb;

    [SerializeField] private int velocidade = 5;
    [SerializeField] private float forcaPulo = 600f;

    [SerializeField] private Transform peDoPersonagem;
    [SerializeField] private LayerMask chaoLayer;

    [SerializeField] private InputActionReference pointerPosition;

    private Vector2 pointerInput;
    private bool estaNoChao;

    private Animator animator;
    private WeaponParent weaponParent;

    private enum State { idle, running, jump }
    private State state = State.idle;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weaponParent = GetComponentInChildren<WeaponParent>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
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

        estaNoChao = Physics2D.OverlapCircle(peDoPersonagem.position, 0.2f, chaoLayer);

        StateChange();
    }

    void FixedUpdate()
    {
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

    private void SetState(State novoEstado)
    {
        if (state == novoEstado) return;

        state = novoEstado;
        animator.SetInteger("state", (int)state);
    }
}
