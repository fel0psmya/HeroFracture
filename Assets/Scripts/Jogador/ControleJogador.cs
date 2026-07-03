using UnityEngine;

public class ControleJogador : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidade = 8f;
    public float forcaPulo = 12f;

    [Header("Configurações da Escada")]
    public float velocidadeEscalada = 5f;
    private float movimentoVertical;
    private bool pertoDaEscada;
    private bool escalando;
    private float gravidadeOriginal;

    [Header("Verificação de Chão")]
    public Transform checadorChao;
    public Vector2 tamanhoChecador = new Vector2(0.5f, 0.1f);
    public LayerMask camadaChao;
    private bool estaNoChao;

    private Rigidbody2D rb;
    private Animator anim;
    private float movimentoHorizontal;
    private DashJogador dash;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dash = GetComponent<DashJogador>();
        
        // Salva a gravidade padrão (geralmente 1 ou algo parecido) para restaurar depois da escada
        gravidadeOriginal = rb.gravityScale; 
    }

    void Update()
    {
        if (EstaBloqueadoPeloDash()) return;

        movimentoHorizontal = Input.GetAxisRaw("Horizontal");
        movimentoVertical = Input.GetAxisRaw("Vertical"); // Lê as setas Cima/Baixo ou W/S

        // Lógica para começar a escalar: tem que estar na escada e apertar para cima ou para baixo
        if (pertoDaEscada && Mathf.Abs(movimentoVertical) > 0f)
        {
            escalando = true;
        }

        if(Input.GetButtonDown("Jump") && estaNoChao && !escalando)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
        }

        AjustarDirecaoSprite();
        AtualizarAnimacoes();
    }

    void FixedUpdate()
    {
        if (EstaBloqueadoPeloDash()) return;

        // 1. Movimento Horizontal
        if (movimentoHorizontal != 0)
        {
            rb.linearVelocity = new Vector2(movimentoHorizontal * velocidade, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        // 2. Lógica de Escalada
        if (escalando)
        {
            rb.gravityScale = 0f; // Desliga a gravidade para ele não cair
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, movimentoVertical * velocidadeEscalada);
        }
        else
        {
            rb.gravityScale = gravidadeOriginal; // Devolve a gravidade quando sai da escada
        }

        // 3. Verificação do Chão
        if (checadorChao != null)
        {
            estaNoChao = Physics2D.OverlapBox(checadorChao.position, tamanhoChecador, 0f, camadaChao);
        }
    }

    // --- MÉTODOS DE DETECÇÃO (TRIGGERS) ---

    private void OnTriggerEnter2D(Collider2D colisao)
    {
        if (colisao.CompareTag("Escada"))
        {
            pertoDaEscada = true;
        }
    }

    private void OnTriggerExit2D(Collider2D colisao)
    {
        if (colisao.CompareTag("Escada"))
        {
            pertoDaEscada = false;
            escalando = false;
        }
    }

    // --- MÉTODOS VISUAIS E DE ESTADO ---

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
        
        // Se você tiver uma animação de escalar no Animator, pode ativá-la assim:
        // anim.SetBool("isClimbing", escalando);
    }

    private void OnDrawGizmosSelected()
    {
        if (checadorChao != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(checadorChao.position, tamanhoChecador);
        }
    }

    private bool EstaBloqueadoPeloDash()
    {
        return dash != null && dash.EstaDandoDash();
    }
}