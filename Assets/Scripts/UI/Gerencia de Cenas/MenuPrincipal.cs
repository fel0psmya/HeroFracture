using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    private void Start()
    {
        if (AudioManager.Instancia != null && AudioManager.Instancia.musicaMenu != null) {
            AudioManager.Instancia.TocarMusica(AudioManager.Instancia.musicaMenu);
        }
    }

    public void IniciarJogo(string nomeDaCenaPrimeiraFase)
    {
        if (AudioManager.Instancia != null){
            AudioManager.Instancia.TocarSFX(AudioManager.Instancia.somBotao);
        }

        Time.timeScale = 1f; 
        SceneManager.LoadScene(nomeDaCenaPrimeiraFase);
    }

    public void SairDoJogo()
    {
        if (AudioManager.Instancia != null) {
            AudioManager.Instancia.TocarSFX(AudioManager.Instancia.somBotao);
        }
        
        Debug.Log("Fechando o jogo...");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}