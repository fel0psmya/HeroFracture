using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PainelUpgradesUI : MonoBehaviour
{
    public static PainelUpgradesUI Instancia { get; private set; }

    [Header("Referências Visuais (Lado Esquerdo)")]
    [SerializeField] private TextMeshProUGUI textoDataNodesMenu;

    [Header("Referências Visuais (Lado Direito)")]
    [SerializeField] private TextMeshProUGUI textoTitulo;
    [SerializeField] private TextMeshProUGUI textoDescricao;
    [SerializeField] private Button botaoComprar;
    [SerializeField] private TextMeshProUGUI textoBotaoComprar;
    [SerializeField] private Image fundoBotaoComprar;

    [Header("Cores do Botão de Compra")]
    [SerializeField] private Color corBotaoAtivo = new Color(0.2f, 0.5f, 0.8f); 
    [SerializeField] private Color corBotaoInativo = new Color(0.4f, 0.4f, 0.4f); 

    private NodeUpgradeUI nodeSelecionadoAtual;

    void Awake()
    {
        Instancia = this;
    }

    void OnEnable()
    {
        textoTitulo.text = "Selecione uma Habilidade";
        textoDescricao.text = "";
        DesativarBotaoCompra("---");

        if (nodeSelecionadoAtual != null) nodeSelecionadoAtual.SetSelecionado(false);
        nodeSelecionadoAtual = null;

        AtualizarTextoDataNodesMenu();
        AtualizarTodosOsNodes();
    }

    private void AtualizarTextoDataNodesMenu()
    {
        if (GerenciadorEvolucao.Instancia != null && textoDataNodesMenu != null)
        {
            textoDataNodesMenu.text = $"Data-Nodes: {GerenciadorEvolucao.Instancia.dataNodesColetados:D3}";
        }
    }

    public void SelecionarUpgrade(NodeUpgradeUI node)
    {
        if (nodeSelecionadoAtual != null) nodeSelecionadoAtual.SetSelecionado(false);

        nodeSelecionadoAtual = node;
        nodeSelecionadoAtual.SetSelecionado(true); 

        UpgradeData dados = node.upgradeData;
        textoTitulo.text = dados.nomeExibicao;

        string sinal = "+";
        string unidade = "";

        switch (dados.tipoEfeito)
        {
            case TipoUpgrade.PotenciaLamina:
                sinal = "+";
                unidade = " de Dano Base";
                break;
            case TipoUpgrade.EficienciaDash:
                sinal = "";
                unidade = "s no Tempo de Recarga";
                break;
            case TipoUpgrade.IntegridadeMemoria:
                sinal = "+";
                unidade = " de Vida Máxima";
                break;
        }

        string textoFormatado = $"{dados.descricao}\n\n" +
                                $"<color=#00FF66><b>MUDANÇA DE STATUS:</b> {sinal}{dados.valorMelhoria:F1}{unidade}</color>";

        textoDescricao.text = textoFormatado;

        AtualizarRegrasDoBotao(dados);
    }

    private void AtualizarRegrasDoBotao(UpgradeData dados)
    {
        if (GerenciadorEvolucao.Instancia == null) return;

        bool jaComprado = GerenciadorEvolucao.Instancia.upgradesComprados.Contains(dados.idUnico);
        bool temPreRequisito = dados.upgradePreRequisito == null || GerenciadorEvolucao.Instancia.upgradesComprados.Contains(dados.upgradePreRequisito.idUnico);
        bool temDinheiro = GerenciadorEvolucao.Instancia.dataNodesColetados >= dados.custoDataNodes;

        if (jaComprado)
        {
            DesativarBotaoCompra("Já Possui");
        }
        else if (!temPreRequisito)
        {
            DesativarBotaoCompra("Requer Habilidade Anterior");
        }
        else if (!temDinheiro)
        {
            DesativarBotaoCompra($"Data-Nodes Insuficientes ({dados.custoDataNodes})");
        }
        else
        {
            botaoComprar.interactable = true;
            fundoBotaoComprar.color = corBotaoAtivo;
            textoBotaoComprar.text = $"Comprar ({dados.custoDataNodes})";
        }
    }

    private void DesativarBotaoCompra(string mensagem)
    {
        botaoComprar.interactable = false;
        fundoBotaoComprar.color = corBotaoInativo;
        textoBotaoComprar.text = mensagem;
    }

    public void EfetuarCompra()
    {
        if (nodeSelecionadoAtual != null && GerenciadorEvolucao.Instancia != null)
        {
            bool temDinheiro = GerenciadorEvolucao.Instancia.dataNodesColetados >= nodeSelecionadoAtual.upgradeData.custoDataNodes;
            bool jaComprado = GerenciadorEvolucao.Instancia.upgradesComprados.Contains(nodeSelecionadoAtual.upgradeData.idUnico);
            
            if (temDinheiro && !jaComprado && AudioManager.Instancia != null)
            {
                switch (nodeSelecionadoAtual.upgradeData.tipoEfeito)
                {
                    case TipoUpgrade.PotenciaLamina:
                        AudioManager.Instancia.TocarSFX(AudioManager.Instancia.somUpgradeArma);
                        break;
                    case TipoUpgrade.EficienciaDash:
                        AudioManager.Instancia.TocarSFX(AudioManager.Instancia.somUpgradeDash);
                        break;
                    case TipoUpgrade.IntegridadeMemoria:
                        AudioManager.Instancia.TocarSFX(AudioManager.Instancia.somUpgradeVida);
                        break;
                }
            }

            GerenciadorEvolucao.Instancia.TentarComprarUpgrade(nodeSelecionadoAtual.upgradeData);
            
            AtualizarTextoDataNodesMenu();
            AtualizarRegrasDoBotao(nodeSelecionadoAtual.upgradeData);
            AtualizarTodosOsNodes();
            
            if (GerenciadorInterface.Instancia != null) 
            {
                GerenciadorInterface.Instancia.AtualizarHUD();
            }
        }
    }

    public void AtualizarTodosOsNodes()
    {
        NodeUpgradeUI[] todosOsNodes = GetComponentsInChildren<NodeUpgradeUI>();
        foreach (var node in todosOsNodes)
        {
            if (node == nodeSelecionadoAtual)
                node.SetSelecionado(true);
            else
                node.AtualizarVisual();
        }
    }
}