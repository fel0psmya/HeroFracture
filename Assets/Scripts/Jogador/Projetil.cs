using UnityEngine;

public class Projetil : MonoBehaviour
{
    [Header("Configurações do Projétil")]
    [SerializeField] private float danoProjetil = 0.5f;
    [SerializeField] private float tempoVidaMaximo = 2.0f;

    private int camadaChao;
    void Start()
    {
        camadaChao = LayerMask.NameToLayer("Ground");
        Destroy(gameObject, tempoVidaMaximo);
    }

    private void OnTriggerEnter2D(Collider2D colisor)
    {
        if (colisor.CompareTag("Player")) return;

        SistemaVida vidaDoAlvo = colisor.GetComponent<SistemaVida>();
        if (vidaDoAlvo != null)
        {
            vidaDoAlvo.TomarDano(danoProjetil);
            Destroy(gameObject);
            return; // Evitar dupla checagem
        }

        ObjetoQuebravel caixa = colisor.GetComponent<ObjetoQuebravel>();
        if (caixa != null)
        {
            caixa.TomarGolpe();
            Destroy(gameObject);
            return;
        }
        
        // Se bater nas plataformas ou chão, também se destrói
        if (colisor.gameObject.layer == camadaChao)
        {
            Destroy(gameObject);
        }
    }
}
