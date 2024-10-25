using Terresquall;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    VirtualJoystick joystick;
    NavMeshAgent navMeshAgent;

    public Rigidbody rig;
    public float vel;
    public bool movimentacaoLivre = true; 

    public Text movimentacaoText;
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        joystick = FindObjectOfType<VirtualJoystick>();
        navMeshAgent = GetComponent<NavMeshAgent>();


        navMeshAgent.enabled = false;
    }

    void Update()
    {
        if (movimentacaoLivre)
        {
            navMeshAgent.enabled = false; 
            Vector2 movementJoystick = joystick.GetAxis();
            Vector3 movement = new Vector3(movementJoystick.x, 0, movementJoystick.y);
            transform.position += movement * Time.deltaTime * vel;
        }
        else
        {

            navMeshAgent.enabled = true; 
            Vector2 movementJoystick = joystick.GetAxis();
            Vector3 movement = new Vector3(movementJoystick.x, 0, movementJoystick.y);
            navMeshAgent.Move(movement * Time.deltaTime * vel);


        }

        movimentacaoText.text = movimentacaoLivre.ToString();
    }


    public void TrocarMovimentacao()
    {
        
            movimentacaoLivre = !movimentacaoLivre; 
            Debug.Log("Trocou a movimentação: " + movimentacaoLivre);
        
    }
}
