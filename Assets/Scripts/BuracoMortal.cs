using UnityEngine;

public class BuracoMortal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se quem entrou no buraco tem a tag "Player"
        if (collision.CompareTag("Player"))
        {
            // Puxa o seu script SistemaVida
            SistemaVida vida = collision.GetComponent<SistemaVida>();
            
            if (vida != null)
            {
                // Aplica um dano absurdo para garantir hit kill (ou usa a MaxVida)
                Debug.Log("Jogador caiu no buraco!");
                vida.TomarDano(vida.MaxVida); 
            }
        }
        // Bônus: Se um inimigo ou drone cair no buraco, ele é destruído para não pesar o jogo
        else if (collision.CompareTag("Inimigo") || collision.CompareTag("InimigoVoador"))
        {
            Destroy(collision.gameObject);
        }
    }
}