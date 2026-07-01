using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GerenciadorInterface : MonoBehaviour
{
    [Header("HUD de Gameplay")]
    [SerializeField] private Slider sliderVida;
    [SerializeField] private TextMeshProUGUI textoDataNodes;

    [Tooltip("Quantos pixels a barra deve ter para cada 1 ponto de vida? (Ex: 100)")]
    [SerializeField] private float multiplicadorLarguraVida = 100f;

    [Header("Munição da Katana")]
    [SerializeField] private Image[] slotsMunicao;

    [Header("Menus Ocultos")]
    [SerializeField] private GameObject painelPause;
    [SerializeField] private GameObject painelUpgrades;

    public static GerenciadorInterface Instancia { get; private set; }

    private bool jogoPausado = false;
    private bool painelUpgradesAberto = false;
    public bool IsPausado => jogoPausado || painelUpgradesAberto;

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
            Destroy(gameObject);
        else
            Instancia = this;
    }

    private void Start()
    {
        if (painelPause != null) painelPause.SetActive(false);
        Time.timeScale = 1f;

        AtualizarHUD();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (painelUpgradesAberto)
            {
                FecharPainelUpgrades();
            }
            else
            {
                if (jogoPausado)
                    ContinuarJogo();
                else
                    PausarJogo();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!jogoPausado)
            {
                if (painelUpgradesAberto)
                    FecharPainelUpgrades();
                else
                    AbrirPainelUpgrades();
            }
        }
    }

    public void PausarJogo()
    {
        if (painelUpgradesAberto) return;

        jogoPausado = true;
        if (painelPause != null) painelPause.SetActive(true);
        
        Time.timeScale = 0f; 
        Debug.Log("Jogo Pausado");
    }

    public void ContinuarJogo()
    {
        jogoPausado = false;
        if (painelPause != null) painelPause.SetActive(false);
        
        Time.timeScale = 1f; 
        Debug.Log("Jogo Retomado");
    }

    public void AbrirPainelUpgrades()
    {
        painelUpgradesAberto = true;
        if (painelUpgrades != null) painelUpgrades.SetActive(true);
        
        Time.timeScale = 0f;
        Debug.Log("Menu de Upgrades Aberto");
    }

    public void FecharPainelUpgrades()
    {
        painelUpgradesAberto = false;
        if (painelUpgrades != null) painelUpgrades.SetActive(false);

        Time.timeScale = 1f; 
        Debug.Log("Menu de Upgrades Fechado");
    }

    public void VoltarParaMenuPrincipal()
    {
        Time.timeScale = 1f; // Descongela o tempo antes de sair
        jogoPausado = false;
        
        SceneManager.LoadScene("MenuInicial"); 
    }

    public void SairDoJogo()
    {
        Debug.Log("Saindo do jogo...");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void AtualizarMunicaoHUD(int municaoAtual, float progressoRecarga)
    {
        for (int i = 0; i < slotsMunicao.Length; i++)
        {
            if (slotsMunicao[i] == null) continue;

            Color corOriginal = slotsMunicao[i].color;
            if (i < municaoAtual) 
            {
                slotsMunicao[i].fillAmount = 1f;
                slotsMunicao[i].color = new Color(corOriginal.r, corOriginal.g, corOriginal.b, 1f);
            }
            else if (i == municaoAtual) 
            {
                slotsMunicao[i].fillAmount = progressoRecarga;
                slotsMunicao[i].color = new Color(corOriginal.r, corOriginal.g, corOriginal.b, 0.7f); 
            }
            else 
            {
                slotsMunicao[i].fillAmount = 0f; 
                slotsMunicao[i].color = new Color(corOriginal.r, corOriginal.g, corOriginal.b, 0.2f);
            }
        }
    }
    public void AtualizarHUD()
    {
        if (GerenciadorEvolucao.Instancia != null && textoDataNodes != null)
        {
            textoDataNodes.text = $"Data-Nodes: {GerenciadorEvolucao.Instancia.dataNodesColetados:D3}"; 
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && sliderVida != null)
        {
            SistemaVida vida = player.GetComponent<SistemaVida>();
            if (vida != null)
            {
                sliderVida.maxValue = vida.getVidaMaxima;
                sliderVida.value = vida.getVidaAtual;

                RectTransform rectBarra = sliderVida.GetComponent<RectTransform>();
                if (rectBarra != null)
                {
                    rectBarra.sizeDelta = new Vector2(vida.getVidaMaxima * multiplicadorLarguraVida, rectBarra.sizeDelta.y);
                }
            }
        }
    }
}