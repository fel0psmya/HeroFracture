using UnityEngine;

public class ControleJogador : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidade = 8f;
    public float forcaPulo = 12f;

    [Header("Verificação de Chão")]
    public Transform checadorChao;
    public LayerMask camadaChao;
    private bool estaNoChao;

    private Rigidbody2D rb;
    private Animator anim;
    private float movimentoHorizontal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movimentoHorizontal = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump") && estaNoChao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
        }

        AjustarDirecaoSprite();
        AtualizarAnimacoes();
    }

    void FixedUpdate()
    {
        // Aplica a velocidade horizontal na física
        rb.linearVelocity = new Vector2(movimentoHorizontal * velocidade, rb.linearVelocity.y);

        // Verifica se o jogador está tocando o chão usando um pequeno círculo nos pés
        estaNoChao = Physics2D.OverlapCircle(checadorChao.position, 0.2f, camadaChao);
    }

    void AjustarDirecaoSprite()
    {
        float escalaXOriginal = Mathf.Abs(transform.localScale.x);

        if (movimentoHorizontal > 0)
            transform.localScale = new Vector3(escalaXOriginal, transform.localScale.y, transform.localScale.z);
        else if (movimentoHorizontal < 0)
            transform.localScale = new Vector3(-escalaXOriginal, transform.localScale.y, transform.localScale.z);
    }

    void AtualizarAnimacoes()
    {
        anim.SetBool("isRunning", movimentoHorizontal != 0);
        anim.SetBool("isGrounded", estaNoChao);
    }

    private void OnDrawGizmosSelected()
    {
        if (checadorChao != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(checadorChao.position, 0.2f);
        }
    }
}
