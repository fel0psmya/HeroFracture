using UnityEngine;

public class ItemCura : MonoBehaviour
{
    [Header("Configurações de Cura")]
    [SerializeField] private float quantidadeCura = 1.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SistemaVida vidaJogador = collision.GetComponent<SistemaVida>();

            if (vidaJogador != null)
            {
                vidaJogador.Curar(quantidadeCura);                
                
                if (AudioManager.Instancia != null) 
                {
                    AudioManager.Instancia.TocarSFX(AudioManager.Instancia.somPegarCura);
                }

                Destroy(gameObject);
            }
        }
    }
}