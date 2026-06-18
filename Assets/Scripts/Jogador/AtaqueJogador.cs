using UnityEngine;

public class AtaqueJogador : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private GameObject projetilPrefab;
    [SerializeField] private Transform pontoDisparo; // Um objeto vazio posicionado na ponta da espada

    [Header("Configurações de Ataque")]
    [SerializeField] private float velocidadeProjetil = 15f;
    
    [Header("Sistema de Munição")]
    [SerializeField] private int maxMunicao = 5;
    [SerializeField] private float tempoEsperaInatividade = 10.0f;
    [SerializeField] private float tempoRegeneracaoTotal = 2.0f;

    private int municaoAtual;
    private float cronometroRegeneracao;
    private bool recarregandoTudo = false;

    private Animator anim;
    private bool estaAtacando = false;
    private float bonusDano = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        municaoAtual = maxMunicao;
    }

    // Update is called once per frame
    void Update()
    {
        CalcularRegeneracaoMunicao();

        if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.B)) && !estaAtacando && municaoAtual > 0)
        {
            IniciarAtaque();
        }
    }

    void IniciarAtaque()
    {
        estaAtacando = true;
        municaoAtual--;

        cronometroRegeneracao = 0f;
        recarregandoTudo = false;

        anim.SetTrigger("attack");
        Debug.Log($"Energia Restante: {municaoAtual}/{maxMunicao}");
    }

    private void CalcularRegeneracaoMunicao()
    {
        if (municaoAtual >= maxMunicao) return;

        if (!recarregandoTudo)
        {
            cronometroRegeneracao += Time.deltaTime;

            if (cronometroRegeneracao >= tempoEsperaInatividade)
            {
                recarregandoTudo = true;
                cronometroRegeneracao= 0f;
            }
        }
        else
        {
            cronometroRegeneracao += Time.deltaTime;

            if (cronometroRegeneracao >= tempoRegeneracaoTotal)
            {
                municaoAtual = maxMunicao;
                recarregandoTudo = false;
                cronometroRegeneracao = 0f;
                Debug.Log($"Katana Totalmente Recarregada! Munição: {municaoAtual}/{maxMunicao}");
            }
        }
    }

    private void OnDisable()
    {
        estaAtacando = false;
    }

    void Atacar()
    {
        if (projetilPrefab == null || pontoDisparo == null) return;

        GameObject projetil = Instantiate(projetilPrefab, pontoDisparo.position, Quaternion.identity);

        // Injeção de bônus de dano, caso exista pelos upgrades comprados
        Projetil scriptProjetil = projetil.GetComponent<Projetil>();
        if (scriptProjetil != null)
        {
            scriptProjetil.ConfigurarDanoBonus(bonusDano);
        }

        float direcaoOlhar = Mathf.Sign(transform.localScale.x);
        
        Rigidbody2D rbProjetil = projetil.GetComponent<Rigidbody2D>();
        if (rbProjetil != null)
        {
            rbProjetil.linearVelocity = new Vector2(direcaoOlhar * velocidadeProjetil, 0f);
        }

        if (direcaoOlhar < 0)
        {
            projetil.transform.localScale = new Vector3(-Mathf.Abs(projetil.transform.localScale.x), projetil.transform.localScale.y, projetil.transform.localScale.z);
        }
    }

    public void FinalizarAtaque()
    {
        estaAtacando = false;
    }

    public void AumentarDano(float bonus)
    {
        bonusDano += bonus;
        Debug.Log($"Dano da katana atualizado! Bônus atual: +{bonusDano}");
    }
}
