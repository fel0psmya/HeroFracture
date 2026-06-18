using UnityEngine;
using System.Collections.Generic;

public class GerenciadorEvolucao : MonoBehaviour
{
    [Header("Recursos")]
    public int dataNodesColetados { get; private set; }

    [Header("Sistema de Upgrades")]
    [Tooltip("Lista dos IDs únicos dos upgrades que o jogador já comprou")]
    public List<string> upgradesComprados = new List<string>();

    [Header("Apenas para Testes (T09)")]
    public UpgradeData upgradeParaTestar;

    public static GerenciadorEvolucao Instancia { get; private set; } // Singleton para facilitar o acesso 

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
            Destroy(gameObject);
        else
            Instancia = this;
    }

    private void Update()
    {
        // Teste
        if (Input.GetKeyDown(KeyCode.I))
        {
            AdicionarDataNode(1);
        }

        // SIMULADOR DE BOTÃO UI: Pressione 'U' para tentar comprar
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (upgradeParaTestar != null)
            {
                TentarComprarUpgrade(upgradeParaTestar);
            }
            else
            {
                Debug.LogWarning("Arraste um UpgradeData no Inspector para testar!");
            }
        }
    }

    public void AdicionarDataNode(int quantidade = 1)
    {
        dataNodesColetados += quantidade;
        Debug.Log($"Data-Node coletado! Saldo: {dataNodesColetados}");
    }

    public void TentarComprarUpgrade(UpgradeData upgrade)
    {
        if (upgradesComprados.Contains(upgrade.idUnico))
        {
            Debug.LogWarning($"[BLOQUEADO] Você já possui o upgrade: {upgrade.nomeExibicao}");
            return;
        }

        if (dataNodesColetados < upgrade.custoDataNodes)
        {
            Debug.LogWarning($"[RECUSADO] Faltam Data-Nodes! Custa {upgrade.custoDataNodes}, você tem {dataNodesColetados}.");
            return;
        }

        if (upgrade.upgradePreRequisito != null)
        {
            if (!upgradesComprados.Contains(upgrade.upgradePreRequisito.idUnico))
            {
                Debug.LogWarning($"[BLOQUEADO] Pré-requisito faltando! Você precisa de: {upgrade.upgradePreRequisito.nomeExibicao}.");
                return;
            }
        }

        dataNodesColetados -= upgrade.custoDataNodes;
        upgradesComprados.Add(upgrade.idUnico);
        
        Debug.Log($"[COMPRA SUCESSO] {upgrade.nomeExibicao} adquirido! Saldo restante: {dataNodesColetados}");

        AplicarMelhoriaNoJogador(upgrade);
    }

    private void AplicarMelhoriaNoJogador(UpgradeData upgrade)
    {
        GameObject jogador = GameObject.FindGameObjectWithTag("Player");

        if (jogador == null)
        {
            Debug.LogError("ERRO: Jogador não encontrado na cena (Verifique a tag)");
            return;
        }

        switch (upgrade.tipoEfeito)
        {
            case TipoUpgrade.PotenciaLamina:
                AtaqueJogador ataque = jogador.GetComponent<AtaqueJogador>();
                if (ataque != null) ataque.AumentarDano(upgrade.valorMelhoria);
                break;
                
            case TipoUpgrade.EficienciaDash:
                DashJogador dash = jogador.GetComponent<DashJogador>();
                if (dash != null) dash.ReduzirTempoRecarga(upgrade.valorMelhoria);
                break;
                
            case TipoUpgrade.IntegridadeMemoria:
                SistemaVida vida = jogador.GetComponent<SistemaVida>();
                if (vida != null) vida.AumentarVidaMaxima(upgrade.valorMelhoria);
                break;
        }
    }
}