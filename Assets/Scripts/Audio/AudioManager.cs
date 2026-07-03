using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instancia { get; private set; }

    [Header("Fontes de Áudio")]
    [Tooltip("Arraste o AudioSource destinado à música de fundo aqui")]
    [SerializeField] private AudioSource musicaSource;
    
    [Tooltip("Arraste o AudioSource destinado aos efeitos sonoros aqui")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Músicas")]
    public AudioClip musicaMenu;
    public AudioClip musicaFase;
    public AudioClip musicaBoss;

    [Header("Efeitos Sonoros - Jogador")]
    public AudioClip somAtaque;
    public AudioClip somDash;
    public AudioClip somDanoRecebido;

    [Header("Efeitos Sonoros - Inimigos e Ambiente")]
    public AudioClip somDroneExplosao;
    public AudioClip somDisparoProjetil;
    public AudioClip somCaixaQuebrando;
    
    [Header("Efeitos Sonoros - Itens e UI")]
    public AudioClip somPegarCura;
    public AudioClip somPegarDataNode;
    public AudioClip somBotao;

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;
        
        DontDestroyOnLoad(gameObject); 
    }

    public void TocarMusica(AudioClip musica)
    {
        if (musicaSource.clip == musica) return;

        musicaSource.clip = musica;
        musicaSource.loop = true;
        musicaSource.Play();
    }

    public void PararMusica()
    {
        musicaSource.Stop();
    }
    public void TocarSFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
    public void TocarSFX(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }
}