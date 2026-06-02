using UnityEngine;
using TMPro;
public class SistemaVida : MonoBehaviour
{
    [Header("Configurações de Vida")]
    [SerializeField] private float maxVida = 5.0f;
    private float vidaAtual;

    [Header("Interface Flutuante")]
    [SerializeField] private TMP_Text textoVida;

    private Animator anim;

    void Start()
    {
        vidaAtual = maxVida;
        anim = GetComponent<Animator>();
        AtualizarTexto();
    }

    public void TomarDano(float dano)
    {
        vidaAtual -= dano;
        vidaAtual = Mathf.Max(vidaAtual, 0); // Impede a vida de mostrar valores negativos

        AtualizarTexto();
        Debug.Log($"{gameObject.name} tomou {dano} de dano. Vida restante: {vidaAtual}");

        // Ativa a animação de impacto se o Animator existir
        if (anim != null)
        {
            anim.SetTrigger("tomouDano");
        }

        if (vidaAtual <= 0)
        {
            Morrer();
        }
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
        Debug.Log($"{gameObject.name} foi destruído");
        Destroy(gameObject);
    }
}
