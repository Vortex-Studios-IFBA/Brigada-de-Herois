using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public Transform pontoDeEntrada; 
    private NavMeshAgent navMeshAgent;
    public bool jogadorNaSala;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        //ReposicionarNoNavMesh();
    }

    private void Update()
    {
        if (jogadorNaSala)
        {
            MoverParaEntrada();
        }
    }
    public void MoverParaEntrada()
    {
        if (navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.SetDestination(pontoDeEntrada.position);
        }
        else
        {
            Debug.LogError("NavMeshAgent n�o est� ativo ou n�o est� em uma �rea v�lida do NavMesh.");
            ReposicionarNoNavMesh(); 
        }
    }

    void ReposicionarNoNavMesh()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position; 
            if (!navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.enabled = true;
            }
        }
        else
        {
            Debug.LogError("N�o foi poss�vel encontrar uma posi��o v�lida no NavMesh.");
        }
    }
}
