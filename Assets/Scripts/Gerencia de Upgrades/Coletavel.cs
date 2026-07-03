using UnityEngine;

public class Coletavel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GerenciadorEvolucao status = collision.GetComponentInParent<GerenciadorEvolucao>();
            if (status != null)
            {
                status.AdicionarDataNode();
                
                if (AudioManager.Instancia != null)
                {
                    AudioManager.Instancia.TocarSFX(AudioManager.Instancia.somPegarDataNode);
                }
                
                Destroy(gameObject);
            } 
            else 
            {
                Debug.LogWarning("Bateu no Player, mas o script 'GerenciadorEvolucao' não foi encontrado no objeto ou nos pais!");
            }
        }
    }
}