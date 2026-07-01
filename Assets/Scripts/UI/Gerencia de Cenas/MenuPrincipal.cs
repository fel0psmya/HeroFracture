using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void IniciarJogo(string nomeDaCenaPrimeiraFase)
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(nomeDaCenaPrimeiraFase);
    }

    public void SairDoJogo()
    {
        Debug.Log("Fechando o jogo...");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}