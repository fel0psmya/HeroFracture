using UnityEngine;
using System.Collections;

public class ObjetoQuebravel : MonoBehaviour
{
    [Header("Configurações de Loot")]
    [SerializeField] private GameObject[] prefabsLoot;
    [Range(0f, 100f)] [SerializeField] private float chanceDeDrop = 70f;

    private Animator anim;
    private Collider2D colisor;
    private bool jaQuebrou = false;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        colisor = GetComponent<Collider2D>();
    }

    public void TomarGolpe()
    {
        if (jaQuebrou) return;
        jaQuebrou = true;

        if (colisor != null) colisor.enabled = false;

        if (anim != null) anim.SetTrigger("quebrar");

        CalcularDrop();
        Destroy(gameObject, 0.5f);
    }

    private void CalcularDrop()
    {
        if (prefabsLoot == null || prefabsLoot.Length == 0) return;

        if (Random.Range(0f, 100f) <= chanceDeDrop)
        {
            int indexAleatorio = Random.Range(0, prefabsLoot.Length);
            Debug.Log($"Caixa Quebrada! Total de itens na lista: {prefabsLoot.Length} | Índice sorteado: {indexAleatorio}");

            GameObject itemSorteado = prefabsLoot[indexAleatorio];

            if (itemSorteado != null)
            {
                Instantiate(itemSorteado, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning($"O slot {indexAleatorio} da lista de loots está VAZIO (Null) no Inspector!");
            }
        }
    }
}