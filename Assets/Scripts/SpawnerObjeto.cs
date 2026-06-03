using System.Collections;
using UnityEngine;

public class SpawnerObjeto : MonoBehaviour
{
    [Header("Configurações de Spawn")]
    [SerializeField] private GameObject prefabParaSpawnar;
    [SerializeField] private float tempoRespawn = 5.0f;

    private GameObject objetoInstanciado;
    private bool aguardandoRespawn = false;

    void Start()
    {
        Spawnar();
    }

    void Update()
    {
        if (objetoInstanciado == null && !aguardandoRespawn)
        {
            StartCoroutine(RotinaRespawn());
        }
    }

    private void Spawnar()
    {
        if (prefabParaSpawnar == null) return;
        
        // Instancia o prefab exatamente na posição do Spawner
        objetoInstanciado = Instantiate(prefabParaSpawnar, transform.position, Quaternion.identity);
    }

    private IEnumerator RotinaRespawn()
    {
        aguardandoRespawn = true;
        yield return new WaitForSeconds(tempoRespawn);

        Spawnar();
        aguardandoRespawn = false;
    }
}