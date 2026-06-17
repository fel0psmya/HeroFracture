using UnityEngine;
using TMPro;
using System;
using System.Collections;
using UnityEngine.SceneManagement;


public class SistemaVida : MonoBehaviour
{
    [Header("Configurações de Vida")]
    [SerializeField] private float maxVida = 5.0f;
    private float vidaAtual;

    [Header("Interface e Visual")]
    [SerializeField] private TMP_Text textoVida;
    [SerializeField] private SpriteRenderer spriteRenderer; // Referência para a imagem do personagem
    [SerializeField] private Color corDano = Color.red;     // Cor que ele pisca ao apanhar
    [SerializeField] private float tempoPiscar = 0.15f;

    [Header("Recompensas (Apenas Inimigos)")]
    [SerializeField] private GameObject prefabDrop;

    private Animator anim;
    private Color corOriginal = Color.white;
    public event Action OnDanoRecebido;
    
    void Start()
    {
        vidaAtual = maxVida;
        anim = GetComponentInChildren<Animator>();

        if (spriteRenderer != null)
        {
            corOriginal = spriteRenderer.color;
        }

        AtualizarTexto();
    }

    public void TomarDano(float dano)
    {
        vidaAtual -= dano;
        vidaAtual = Mathf.Max(vidaAtual, 0); // Impede a vida de mostrar valores negativos
        AtualizarTexto();
        Debug.Log($"{gameObject.name} tomou {dano} de dano. Vida restante: {vidaAtual}");

        OnDanoRecebido?.Invoke();

        if (vidaAtual <= 0)
        {
            vidaAtual = 0;
            AtualizarTexto();
            Morrer();
        }
        else
        {
            if (spriteRenderer != null)
            {
                StartCoroutine(PiscarEfeitoDano());
            }
        }
    }

    private IEnumerator PiscarEfeitoDano()
    {
        spriteRenderer.color = corDano;
        yield return new WaitForSeconds(tempoPiscar);
        spriteRenderer.color = corOriginal;
    }

    private void AtualizarTexto()
    {
        if (textoVida != null)
        {
            // "F2" faz mostrar apenas 2 casas decimais (ex: 5.09, 4.52, 0.01)
            textoVida.text = vidaAtual.ToString("F2"); 
        }
    }
    private void Morrer()
    {
        if (spriteRenderer != null) spriteRenderer.color = corOriginal;

        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this) script.enabled = false;
        }

        if (anim != null)
        {
            anim.SetBool("isDead", true);
            anim.Play("Death");
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
            Destroy(gameObject);
        }
    }
}
