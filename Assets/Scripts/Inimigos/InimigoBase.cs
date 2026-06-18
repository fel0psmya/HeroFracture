using UnityEngine;

public abstract class InimigoBase : MonoBehaviour
{
    [Header("Referências Base")]
    [SerializeField] protected string tagJogador = "Player";
    protected Transform alvoJogador;
    protected Rigidbody2D rb;
    protected Animator anim;
    protected SistemaVida minhaVida;

    [Header("Configurações Base")]
    [SerializeField] protected float raioDetecao = 7f;
    [SerializeField] protected float danoContato = 0.15f; // ao esbarrar 
    [SerializeField] protected float cooldownDanoContato = 1f;
    protected float ultimoTempoContato = 0f;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        minhaVida = GetComponent<SistemaVida>();

        GameObject jogadorObj = GameObject.FindWithTag(tagJogador);
        if (jogadorObj != null)
        {
            alvoJogador = jogadorObj.transform;
        }

        if (minhaVida != null)
        {
            minhaVida.OnDanoRecebido += AcordarInimigo; 
        }
    }

    protected virtual void OnDestroy() 
    {
        if (minhaVida != null)
        {
            minhaVida.OnDanoRecebido -= AcordarInimigo;
        }
    }

    protected virtual void AcordarInimigo()
    {
        raioDetecao = 1000f; // Foca no jogador permanentemente depois de tomar dano
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        VerificarDanoContato(collision);
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        VerificarDanoContato(collision);
    }

    // virtual para que inimigos que não causam dano de contato possam sobrescrever e ignorar isso
    protected virtual void VerificarDanoContato(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tagJogador))
        {
            SistemaVida vidaSamurai = collision.gameObject.GetComponent<SistemaVida>();
            if (vidaSamurai != null && Time.time >= ultimoTempoContato)
            {
                vidaSamurai.TomarDano(danoContato);
                ultimoTempoContato = Time.time + cooldownDanoContato;
            }
        }
    }
}