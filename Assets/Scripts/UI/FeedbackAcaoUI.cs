using UnityEngine;
using TMPro;

public class FeedbackTeclaUI : MonoBehaviour
{
    [Header("Configurações de Input")]
    [Tooltip("Qual tecla fará este texto piscar?")]
    public KeyCode teclaAlvo;

    [Tooltip("Existe uma tecla secundária? (Ex: Mouse0 para clique esquerdo)")]
    public KeyCode teclaAlternativa = KeyCode.None;

    [Header("Feedback Visual")]
    [SerializeField] private TextMeshProUGUI textoElemento;
    [SerializeField] private Color corNormal = Color.white;
    [SerializeField] private Color corPressionado = new Color(1f, 0.8f, 0f); // Amarelo/Dourado

    private bool isAtivo = false;
    void Start()
    {   
        bool estaPressionando = Input.GetKey(teclaAlvo) || 
                                (teclaAlternativa != KeyCode.None && Input.GetKey(teclaAlternativa));

        if (estaPressionando && !isAtivo)
        {
            AtivarFeedback();
            isAtivo = true;
        }
        else if (!estaPressionando && isAtivo)
        {
            DesativarFeedback();
            isAtivo = false;
        }
    }

    void Update()
    {
        if (GerenciadorInterface.Instancia != null && GerenciadorInterface.Instancia.IsPausado)
        {
            if (isAtivo) DesativarFeedback(); 
            return; 
        }

        bool apertouPrimaria = Input.GetKey(teclaAlvo);
        bool apertouSecundaria = false;
        
        if (teclaAlternativa == KeyCode.Mouse0)
        {
            apertouSecundaria = Input.GetMouseButton(0); 
        }
        else if (teclaAlternativa != KeyCode.None)
        {
            apertouSecundaria = Input.GetKey(teclaAlternativa);
        }

        bool estaPressionando = apertouPrimaria || apertouSecundaria;

        if (estaPressionando && !isAtivo)
        {
            AtivarFeedback();
            isAtivo = true;
        }
        else if (!estaPressionando && isAtivo)
        {
            DesativarFeedback();
            isAtivo = false;
        }
    }

    private void AtivarFeedback()
    {
        if (textoElemento != null) textoElemento.color = corPressionado;
        
        if (AudioManager.Instancia != null)
        {
            AudioManager.Instancia.TocarSFX(AudioManager.Instancia.somBotao);
        }
    }

    private void DesativarFeedback()
    {
        if (textoElemento != null) textoElemento.color = corNormal;
    }
}