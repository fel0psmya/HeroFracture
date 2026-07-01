using UnityEngine;

public class BossAI : InimigoBase
{
    [Header("Movimentação do Chefe")]
    public float speed = 2f;

    [Header("Ataque Físico Pesado")]
    public float attackRange = 1.5f;
    public float attackDamage = 30f;
    public float attackCooldown = 2f;
    private float lastAttackTime;

    protected override void Start()
    {
        // O base.Start() já encontra o alvoJogador, pega o Animator (anim), Rigidbody (rb) e SistemaVida (minhaVida)
        base.Start(); 
    }

    void Update()
    {
        if (alvoJogador == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, alvoJogador.position);

        // Verifica a distância. Se for maior que o alcance, ele anda.
        if (distanceToPlayer > attackRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            // Se estiver perto, verifica se o cooldown permite atacar
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
        // Espelha o sprite dependendo da posição do jogador
        if (alvoJogador.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else if (alvoJogador.position.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);

        // Move na direção do jogador usando o MoveTowards
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

        // Causa o dano no jogador, aproveitando o SistemaVida que você já criou
        SistemaVida vidaJogador = alvoJogador.GetComponent<SistemaVida>();
        if (vidaJogador != null)
        {
            vidaJogador.TomarDano(attackDamage);
        }
    }

    void OnDisable()
    {
        // O SistemaVida.cs desativa todos os scripts no método Morrer().
        // Logo, quando este script for desativado, sabemos que o Chefe morreu!
        Debug.Log("BossAI foi desativado - Chefe Morreu! Vitória!!");
        
        // A lógica final para chamar a tela de vitória pode ser feita aqui 
        // chamando um GameManager, por exemplo: GameManager.Instance.VencerJogo();
    }
}
