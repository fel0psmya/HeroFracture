using UnityEngine;

public class BossAI : InimigoBase
{
    [Header("Movimentação do Chefe")]
    public float speed = 2f;

    [Header("Ataque Físico Pesado")]
    public float attackRange = 1.5f;
    public float attackDamage = 10f; // Reduzido para o jogador não morrer com 1 hit
    public float attackCooldown = 2f;
    private float lastAttackTime;

    protected override void Start()
    {
        base.Start(); 
    }

    void Update()
    {
        if (alvoJogador == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, alvoJogador.position);

        if (distanceToPlayer > attackRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
            }
            else
            {
                if (anim != null) anim.SetBool("isWalking", false);
            }
        }
    }

    void MoveTowardsPlayer()
    {
        // CORREÇÃO DO TAMANHO: Pega o tamanho atual (Scale Y) e o mantém!
        float tamanho = Mathf.Abs(transform.localScale.y);

        if (alvoJogador.position.x > transform.position.x)
            transform.localScale = new Vector3(tamanho, tamanho, 1);
        else if (alvoJogador.position.x < transform.position.x)
            transform.localScale = new Vector3(-tamanho, tamanho, 1);

        Vector2 targetPosition = new Vector2(alvoJogador.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        
        if (anim != null) anim.SetBool("isWalking", true);
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        if (anim != null) anim.SetBool("isWalking", false);
        if (anim != null) anim.SetTrigger("Attack");

        Debug.Log("Chefe: Receba meu golpe físico pesado!");

        SistemaVida vidaJogador = alvoJogador.GetComponent<SistemaVida>();
        if (vidaJogador != null)
        {
            vidaJogador.TomarDano(attackDamage);
        }
    }

    void OnDisable()
    {
        Debug.Log("BossAI foi desativado - Chefe Morreu!");
    }
}