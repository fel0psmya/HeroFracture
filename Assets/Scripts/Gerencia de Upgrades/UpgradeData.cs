using UnityEngine;

public enum TipoUpgrade
{
    PotenciaLamina,
    EficienciaDash, 
    IntegridadeMemoria 
}

[CreateAssetMenu(fileName = "NovoUpgrade", menuName = "HeroFracture/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    [Header("Identificação")]
    [Tooltip("Um ID único em letras minúsculas (ex: memoria_01, lamina_02) para o sistema de Save")]
    public string idUnico;
    public string nomeExibicao;
    
    [TextArea(2, 4)]
    public string descricao;

    [Header("Custos e Pré-requisitos")]
    public int custoDataNodes;
    
    [Tooltip("Se vazio, o upgrade está liberado desde o nível 1. Se preenchido, exige que o jogador compre o anterior primeiro.")]
    public UpgradeData upgradePreRequisito;

    [Header("Efeito no Jogo")]
    public TipoUpgrade tipoEfeito;
    
    [Tooltip("O valor do buff. Ex: 1 (para +1 de Dano/Vida), -0.5 (para -0.5s no tempo do Dash)")]
    public float valorMelhoria;
}