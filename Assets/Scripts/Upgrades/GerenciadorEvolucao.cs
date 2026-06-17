using UnityEngine;

public class GerenciadorEvolucao : MonoBehaviour
{
    [Header("Recursos")]
    public int dataNodesColetados { get; private set; }

    public void AdicionarDataNode(int quantidade = 1)
    {
        dataNodesColetados += quantidade;
        Debug.Log($"Data-Node coletado! Total: {dataNodesColetados}");
    }
}