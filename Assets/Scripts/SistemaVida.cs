using UnityEngine;
using TMPro;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class SistemaVida : MonoBehaviour
{
    [Header("Configurações de Vida")]
    [SerializeField] private float maxVida = 5.0f;
    public float MaxVida { get { return maxVida; } private set { maxVida = value; } }
    private float vidaAtual;

    public float getVidaMaxima => maxVida;
    public float getVidaAtual => vidaAtual;

    [Header("Interface e Visual")]
    [SerializeField] private TMP_Text textoVida;
    [SerializeField] private SpriteRenderer spriteRenderer; 
    [SerializeField] private Color corDano = Color.red;     
    [SerializeField] private float tempoPiscar = 0.15f;

    [Header("Recompensas (Apenas Inimigos)")]
    [SerializeField] private GameObject prefabDrop;

    private Animator anim;
    private Color corOriginal = Color.white;
    public event Action OnDanoRecebido;
    public event Action OnMorte;
    
    void Awake() 
    {
        vidaAtual = maxVida;
        anim = GetComponentInChildren<Animator>();

        if (spriteRenderer != null)
        {
            corOriginal = spriteRenderer.color;
        }
    }

    void Start()
    {
        if (gameObject.CompareTag("Player") && GerenciadorInterface.Instancia != null)
        {
            GerenciadorInterface.Instancia.AtualizarHUD();
        }
    }

    public void AumentarVidaMaxima(float bonusVida)
    {
        maxVida += bonusVida;
        vidaAtual += bonusVida; 

        if (AudioManager.Instancia != null)
        {
            if (gameObject.CompareTag("Player"))
            {
                AudioManager.Instancia.TocarSFX(AudioManager.Instancia.somDanoRecebido);
            }
        }

        if (gameObject.CompareTag("Player") && GerenciadorInterface.Instancia != null)
        {
            GerenciadorInterface.Instancia.AtualizarHUD();
        }
    }

    public void Curar(float quantidadeCura)
    {
        vidaAtual += quantidadeCura;
        
        if (vidaAtual > maxVida)
        {
            vidaAtual = maxVida;
        }

        if (gameObject.CompareTag("Player") && GerenciadorInterface.Instancia != null)
        {
            GerenciadorInterface.Instancia.AtualizarHUD();
        }
    }

    public void TomarDano(float danoRecebido)
    {
        vidaAtual -= danoRecebido;

        Debug.Log(">>> " + gameObject.name + " tomou " + danoRecebido + " de DANO! Vida atual: " + vidaAtual + " <<<");

        if (OnDanoRecebido != null) OnDanoRecebido.Invoke();

        if (spriteRenderer != null)
        {
            StartCoroutine(EfeitoDano());
        }

        if (gameObject.CompareTag("Player") && GerenciadorInterface.Instancia != null)
        {
            GerenciadorInterface.Instancia.AtualizarHUD();
        }

        if (textoVida != null)
        {
            textoVida.text = vidaAtual.ToString("F2");
        }

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    private IEnumerator EfeitoDano()
    {
        spriteRenderer.color = corDano;
        yield return new WaitForSeconds(tempoPiscar);
        spriteRenderer.color = corOriginal;
    }

    private void Morrer()
    {
        Debug.Log(gameObject.name + " MORREU!");
        if (OnMorte != null) OnMorte.Invoke();
        
        if (AudioManager.Instancia != null)
        {
            AudioManager.Instancia.PararTudo();

            if (gameObject.CompareTag("Player"))
            {
                AudioManager.Instancia.TocarSFX(AudioManager.Instancia.somMorteJogador, 0.7f);
            }
            else if (gameObject.CompareTag("Inimigo")) 
            {
                AudioManager.Instancia.TocarSFX(AudioManager.Instancia.somDroneExplosao, 0.7f);
            }
        }

        if (spriteRenderer != null) spriteRenderer.color = corOriginal;

        // Desativa os outros scripts para o inimigo/boss parar de atacar
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this) script.enabled = false;
        }

        if (anim != null)
        {
            anim.SetBool("isDead", true);
            StartCoroutine(RotinaDestruicao());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator RotinaDestruicao()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic; 
        }

        if (gameObject.CompareTag("Player"))
        {
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            if (prefabDrop != null)
            {
                Instantiate(prefabDrop, transform.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(0.6f);
            Destroy(gameObject); // Chefe/Inimigo some aqui
        }
    }

    private bool TemParametro(Animator animator, string nomeParametro)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == nomeParametro) return true;
        }
        return false;
    }
}