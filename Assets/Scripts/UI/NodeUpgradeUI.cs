using UnityEngine;
using UnityEngine.UI;

public class NodeUpgradeUI : MonoBehaviour
{
    [Header("Dados da Habilidade")]
    public UpgradeData upgradeData; 

    [Header("Cores de Estado")]
    [SerializeField] private Color corBloqueado = new Color(0.3f, 0.3f, 0.3f); 
    [SerializeField] private Color corDisponivel = new Color(0.8f, 0.8f, 0.8f); 
    [SerializeField] private Color corSelecionado = new Color(1f, 0.84f, 0f);
    [SerializeField] private Color corComprado = new Color(0.2f, 0.6f, 1f); 

    private Image imagemFundo;
    private Button botaoPrincipal;
    private bool isSelecionado = false; 

    void Awake()
    {
        imagemFundo = GetComponent<Image>();
        botaoPrincipal = GetComponent<Button>();
        botaoPrincipal.onClick.AddListener(AoClicar);
    }

    public void SetSelecionado(bool selecionado)
    {
        isSelecionado = selecionado;
        AtualizarVisual();
    }

    public void AtualizarVisual()
    {
        if (upgradeData == null || GerenciadorEvolucao.Instancia == null) return;

        bool jaComprado = GerenciadorEvolucao.Instancia.upgradesComprados.Contains(upgradeData.idUnico);
        
        bool temPreRequisito = upgradeData.upgradePreRequisito == null || 
                               GerenciadorEvolucao.Instancia.upgradesComprados.Contains(upgradeData.upgradePreRequisito.idUnico);

        if (jaComprado)
        {
            imagemFundo.color = corComprado;
        }
        else if (isSelecionado)
        {
            imagemFundo.color = corSelecionado;
        }
        else if (temPreRequisito)
        {
            imagemFundo.color = corDisponivel;
        }
        else
        {
            imagemFundo.color = corBloqueado;
        }
    }

    private void AoClicar()
    {
        if (PainelUpgradesUI.Instancia != null)
        {
            PainelUpgradesUI.Instancia.SelecionarUpgrade(this);
        }
    }
}