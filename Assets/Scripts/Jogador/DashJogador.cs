using UnityEngine;
using System.Collections;

public class DashJogador : MonoBehaviour
{
    [Header("Efeitos Visuais")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color corDoDash = Color.cyan; // Escolha a cor que você usava no Animator
    private Color corOriginal = Color.white;

    [Header("Configurações do Dash")]
    [SerializeField] private float forcaDash = 20f;
    [SerializeField] private float duracaoDash = 0.2f;
    [SerializeField] private float tempoRecarga = 1f;

    private Rigidbody2D rb;
    private Animator anim;
    
    private int playerLayer;
    private int inimigoLayer;
    
    private bool podeDarDash = true;
    private bool estaDandoDash = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        playerLayer = LayerMask.NameToLayer("Player");
        inimigoLayer = LayerMask.NameToLayer("Inimigo");

        if (spriteRenderer != null) corOriginal = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0f) return; // Se o jogo estiver pausado

        if (Input.GetKeyDown(KeyCode.LeftShift) && podeDarDash)
        {
            StartCoroutine(RotinaDash());
        }
    }

    private IEnumerator RotinaDash()
    {
        podeDarDash = false;
        estaDandoDash = true;

        AtaqueJogador scriptAtaque = GetComponent<AtaqueJogador>();
        if (scriptAtaque != null)
        {
            scriptAtaque.enabled = false; 
        }

        anim.SetTrigger("dash");

        if (AudioManager.Instancia != null) {
            AudioManager.Instancia.TocarSFX(AudioManager.Instancia.somDash);
        }

        if (spriteRenderer != null) spriteRenderer.color = corDoDash;

        float gravidadeOriginal = rb.gravityScale;
        rb.gravityScale = 0f; // Trava a gravidade para ele não cair durante o dash se estiver no ar

        Physics2D.IgnoreLayerCollision(playerLayer, inimigoLayer, true); // Atravessar inimigos

        float direcaoOlhar = Mathf.Sign(transform.localScale.x);
        rb.linearVelocity = new Vector2(direcaoOlhar * forcaDash, 0f);

        yield return new WaitForSeconds(duracaoDash);
        
        rb.gravityScale = gravidadeOriginal;
        rb.linearVelocity = Vector2.zero;
        Physics2D.IgnoreLayerCollision(playerLayer, inimigoLayer, false);
        estaDandoDash = false;

        if (spriteRenderer != null) spriteRenderer.color = corOriginal;

        if (scriptAtaque != null)
        {
            scriptAtaque.enabled = true;
        }

        yield return new WaitForSeconds(tempoRecarga);
        
        podeDarDash = true;
    }

    public void ReduzirTempoRecarga(float reducao)
    {
        tempoRecarga += reducao; 
        
        if (tempoRecarga < 0.1f) tempoRecarga = 0.1f; 
        
        Debug.Log($"Novo tempo de recarga do Dash: {tempoRecarga}s");
    }

    public bool EstaDandoDash()
    {
        return estaDandoDash;
    }
}
