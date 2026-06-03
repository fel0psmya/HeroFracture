using UnityEngine;
using System.Collections;

public class DroneAtaqueProximidade : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private string tagJogador = "Player";
    private Transform alvoJogador;
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Configurações de Distância")]
    [SerializeField] private float raioDetecao = 7f;
    [SerializeField] private float raioAtaque = 1.5f;

    [Header("Movimentação")]
    [SerializeField] private float velocidadePerseguicao = 4f;

    [Header("Configurações de Ataque")]
    [SerializeField] private float danoAtaque = 0.3f;
    [SerializeField] private float duracaoAnimacaoAtaque = 0.4f;
    [SerializeField] private float cooldownAtaque = 1.5f;

    [Header("Configurações de Visão")]
    [SerializeField] private LayerMask mascaraCenario;    private bool bateuEmObstaculo = false;

    private bool estaAtacando = false;
    private bool emCooldown = false;
    private bool causouDano = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        GameObject jogadorObj = GameObject.FindWithTag(tagJogador);
        if (jogadorObj != null)
        {
            alvoJogador = jogadorObj.transform;
        }
    }

    void FixedUpdate()
    {
        if (alvoJogador == null || estaAtacando) return;
        
        float distancia = Vector2.Distance(transform.position, alvoJogador.position);

        if (distancia > raioDetecao)
        {
            rb.linearVelocity = Vector2.zero;
            if (anim != null) anim.SetBool("perseguindo", false);
            return;
        }

        GirarEmDirecaoAoAlvo();

        if (distancia <= raioAtaque)
        {
            Vector2 direcao = (alvoJogador.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direcao, distancia, mascaraCenario);

            if (hit.collider == null) // Nenhuma parede detectada
            {
                rb.linearVelocity = Vector2.zero;
                if (anim != null) anim.SetBool("perseguindo", false);

                if (!emCooldown)
                {
                    StartCoroutine(RotinaAtaque());
                }
            }
            else
            {
                PerseguirJogador(); 
            }
        }
        else
        {
            PerseguirJogador();
        }
    }

    private void PerseguirJogador()
    {
        if (anim != null) anim.SetBool("perseguindo", true);

        Vector2 direcao = (alvoJogador.position - transform.position).normalized;
        rb.linearVelocity = direcao * velocidadePerseguicao;
    }

    private void GirarEmDirecaoAoAlvo()
    {
        float direcaoParaAlvo = alvoJogador.position.x - transform.position.x;

        if (direcaoParaAlvo > 0.1f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direcaoParaAlvo < -0.1f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private IEnumerator RotinaAtaque()
    {
        estaAtacando = true;
        emCooldown = true;
        causouDano = false;
        bateuEmObstaculo = false;

        rb.linearVelocity = Vector2.zero; 
        rb.angularVelocity = 0f;

        if (anim != null) anim.SetTrigger("atacar");

        Vector2 centroJogador = new Vector2(alvoJogador.position.x, alvoJogador.position.y + 1f);
        Vector2 direcaoAtaque = (centroJogador - (Vector2)transform.position).normalized;

        float tempoPassado = 0f;

        float tempoIda = duracaoAnimacaoAtaque * 0.5f;     
        float tempoEspera = duracaoAnimacaoAtaque * 0.5f;

        float velocidadeIda = raioAtaque / tempoIda;

        // Indo ao jogador
        while (tempoPassado < tempoIda)
        {
            if (causouDano || bateuEmObstaculo) break;

            rb.linearVelocity = direcaoAtaque * velocidadeIda;

            tempoPassado += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(tempoEspera);

        Collider2D meuColisor = GetComponent<Collider2D>();
        if (meuColisor != null && alvoJogador != null)
        {
            Collider2D colisorJogador = alvoJogador.GetComponent<Collider2D>();
            if (colisorJogador != null)
            {
                Physics2D.IgnoreCollision(meuColisor, colisorJogador, false);
            }
        }

        estaAtacando = false;

        yield return new WaitForSeconds(cooldownAtaque);
        emCooldown = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        VerificarEAplicarImpacto(collision);

        if (estaAtacando && ((1 << collision.gameObject.layer) & mascaraCenario) != 0) // Se atacar e bater em uma plataforma, para
        {
            bateuEmObstaculo = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Mesmo se o jogador estiver andando, o dano é adicionado
        VerificarEAplicarImpacto(collision);
    }

    private void VerificarEAplicarImpacto(Collision2D collision)
    {
        if (estaAtacando && !causouDano && collision.gameObject.CompareTag(tagJogador))
        {
            SistemaVida vidaSamurai = collision.gameObject.GetComponent<SistemaVida>();
            if (vidaSamurai != null)
            {
                vidaSamurai.TomarDano(danoAtaque);
                causouDano = true;
                Physics2D.IgnoreCollision(collision.collider, collision.otherCollider, true); // Ignorar a colisão física entre o jogador e o drone temporariamente para não causar um empurrão
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioDetecao);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioAtaque);
    }
}
