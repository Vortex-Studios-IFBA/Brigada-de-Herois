using Terresquall;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    public Rigidbody rig;
    public float vel;
    public bool movimentacaoLivre = true; 
    private NavMeshAgent navMeshAgent;
    VirtualJoystick joystick;

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
            Vector3 forward = transform.forward * movementJoystick.y * vel;
            navMeshAgent.Move(forward * Time.deltaTime);


            if (movementJoystick.x != 0)
            {

                HandleBifurcation(movementJoystick.x);
            }
        }

        movimentacaoText.text = movimentacaoLivre.ToString();
    }

    void HandleBifurcation(float direction)
    {

        if (direction > 0)
        {

        }
        else if (direction < 0)
        {

        }
    }

    public void TrocarMovimentacao()
    {
        
            movimentacaoLivre = !movimentacaoLivre; 
            Debug.Log("Trocou a movimentação: " + movimentacaoLivre);
        
    }
}
