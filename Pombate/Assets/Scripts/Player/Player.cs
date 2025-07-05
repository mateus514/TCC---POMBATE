using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    private float horizontalInput;
    private Rigidbody2D rb;

   [SerializeField] private int velocidade = 5;
   

   [SerializeField] private  Transform  peDoPersonagem;
   [SerializeField] private  LayerMask  chaoLayer;

   private bool estaNoChao;

   private Animator animator;
   
   private int MovendoHash = Animator.StringToHash("Movendo");
   private int PulandoHash = Animator.StringToHash("Pulando");
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            rb.AddForce(Vector2.up * 600);
        }
        
        estaNoChao = Physics2D.OverlapCircle(peDoPersonagem.position, 0.2f, chaoLayer);
        animator.SetBool("Movendo", horizontalInput != 0);
        animator.SetBool("Pulando", !estaNoChao);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * 5, rb.linearVelocity.y);
    }
}
