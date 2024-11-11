using Terresquall;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    VirtualJoystick joystick;
    NavMeshAgent navMeshAgent;
    Rigidbody rig;


    public float vel;
    public float velRotation;
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

            if (movement != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * velRotation);
            }
        }
        else
        {
            navMeshAgent.enabled = true;
            Vector2 movementJoystick = joystick.GetAxis();

            Vector3 forwardMovement = transform.forward * movementJoystick.y * vel;
            Vector3 sideMovement = transform.right * movementJoystick.x * vel;
            Vector3 movement = forwardMovement + sideMovement;

            navMeshAgent.Move(movement * Time.deltaTime);

            if (movement != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * velRotation);
            }
        }

        movimentacaoText.text = movimentacaoLivre.ToString();
    }

    public void TrocarMovimentacao()
    {
        movimentacaoLivre = !movimentacaoLivre;
        Debug.Log("Trocou a movimentação: " + movimentacaoLivre);
    }
}

