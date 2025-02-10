using UnityEngine;
using UnityEngine.AI;


public class NPCController : MonoBehaviour
{
    public Transform pontoDeEntrada; 
    public Transform entradaIfba;
    private NavMeshAgent navMeshAgent;
    public bool jogadorNaSala;

    private bool noNavMesh; 
    public float vel = 2.0f; 

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = false; 
        noNavMesh = false;
        entradaIfba = GameObject.Find("Entrada IFBA").transform;
    }

    private void Update()
    {
        if (jogadorNaSala)
        {
            if (!noNavMesh)
            {
                MoverManualParaEntrada();
            }
            else
            {
                MoverComNavMesh();
            }
        }
    }

    void MoverManualParaEntrada()
    {
        
        transform.position = Vector3.MoveTowards(transform.position, pontoDeEntrada.position, vel * Time.deltaTime);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 0.5f, NavMesh.AllAreas))
        {
            noNavMesh = true;
            navMeshAgent.enabled = true;
            navMeshAgent.Warp(hit.position); 
        }
    }

    void MoverComNavMesh()
    {
        if (navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.SetDestination(entradaIfba.position);
        }
        else
        {
            Debug.LogError("NavMeshAgent n�o est� ativo ou n�o est� em uma �rea v�lida do NavMesh.");
        }
    }
}
