using UnityEngine;

public class AtaqueJogador : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private GameObject projetilPrefab;
    [SerializeField] private Transform pontoDisparo; // Um objeto vazio posicionado na ponta da espada

    [Header("Configurações de Ataque")]
    [SerializeField] private float velocidadeProjetil = 15f;
    
    private Animator anim;
    private bool estaAtacando = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.B)) && !estaAtacando)
        {
            IniciarAtaque();
        }
    }

    void IniciarAtaque()
    {
        estaAtacando = true;
        anim.SetTrigger("attack");
    }
    void Atacar()
    {
        if (projetilPrefab == null || pontoDisparo == null) return;

        GameObject projetil = Instantiate(projetilPrefab, pontoDisparo.position, Quaternion.identity);

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
}
