using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public Transform pontoEntrada;
    private NavMeshAgent navMeshAgent;
    private bool jogadorNaSala = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (jogadorNaSala)
        {
            MoverParaEntrada();
        }
    }

    private void MoverParaEntrada()
    {
        if (navMeshAgent.destination != pontoEntrada.position)
        {
            navMeshAgent.SetDestination(pontoEntrada.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            jogadorNaSala = true;
            Debug.Log("Jogador entrou na sala");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorNaSala = false;
        }
    }
}
