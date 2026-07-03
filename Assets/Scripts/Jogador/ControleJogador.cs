using UnityEngine;

public class ControleJogador : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidade = 8f;
    public float forcaPulo = 12f;

    [Header("Verificação de Chão")]
    public Transform checadorChao;
    public Vector2 tamanhoChecador = new Vector2(0.5f, 0.1f);
    public LayerMask camadaChao;
    private bool estaNoChao;
    private float timerPassos;
    private float intervaloPassos = 0.4f;

    private Rigidbody2D rb;
    private Animator anim;
    private float movimentoHorizontal;
    private DashJogador dash;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dash = GetComponent<DashJogador>();

        if (AudioManager.Instancia != null && AudioManager.Instancia.musicaFase != null) {
            AudioManager.Instancia.TocarMusica(AudioManager.Instancia.musicaFase);
        }
    }

    void Update()
    {
        if (EstaBloqueadoPeloDash()) return;

        movimentoHorizontal = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump") && estaNoChao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
            
            if (AudioManager.Instancia != null)
                AudioManager.Instancia.TocarSFX(AudioManager.Instancia.somPulo);
        }

        if (movimentoHorizontal != 0 && estaNoChao)
        {
            timerPassos -= Time.deltaTime;
            if (timerPassos <= 0)
            {
                if (AudioManager.Instancia != null)
                    AudioManager.Instancia.TocarSFX(AudioManager.Instancia.somPassos);
                
                timerPassos = intervaloPassos;
            }
        }
        else
        {
            timerPassos = 0;
        }

        AjustarDirecaoSprite();
        AtualizarAnimacoes();
    }

    void FixedUpdate()
    {
        if (EstaBloqueadoPeloDash()) return;

        // Aplica a velocidade horizontal na física
        if (movimentoHorizontal != 0)
        {
            rb.linearVelocity = new Vector2(movimentoHorizontal * velocidade, rb.linearVelocity.y);
        }
        else
        {
            // Se soltou a tecla, para
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        Collider2D meuColisor = GetComponent<Collider2D>();
        if (meuColisor != null)
        {
            Vector2 centroBaseColisor = new Vector2(meuColisor.bounds.center.x, meuColisor.bounds.min.y);
            estaNoChao = Physics2D.OverlapBox(centroBaseColisor, tamanhoChecador, 0f, camadaChao);
        }
        else
        {
            estaNoChao = Physics2D.OverlapBox(checadorChao.position, tamanhoChecador, 0f, camadaChao);
        }
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
        Collider2D meuColisor = GetComponent<Collider2D>();
        if (meuColisor != null)
        {
            Gizmos.color = Color.red;
            Vector2 centroBaseColisor = new Vector2(meuColisor.bounds.center.x, meuColisor.bounds.min.y);
            Gizmos.DrawWireCube(centroBaseColisor, tamanhoChecador);
        }
    }

    private bool EstaBloqueadoPeloDash()
    {
        return dash != null && dash.EstaDandoDash();
    }
}
