using UnityEngine;
using UnityEngine.SceneManagement;

public class TransicaoFase : MonoBehaviour
{
    [Tooltip("Digite o nome exato da cena da próxima fase. Se for zerar o jogo, coloque o nome do Menu Inicial.")]
    [SerializeField] private string nomeProximaCena;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"Transição de fase: Carregando {nomeProximaCena}...");
            SceneManager.LoadScene(nomeProximaCena);
        }
    }
}