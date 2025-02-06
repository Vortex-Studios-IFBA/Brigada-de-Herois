using Terresquall;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    VirtualJoystick joystick;
    ControlaJogo jogoConfig;
    NavMeshAgent navMeshAgent;
    Rigidbody rig;

    public float vel;
    public float velRotation;
    public bool movimentacaoLivre;

    public Text movimentacaoText;

    public Transform transformCamera; 
    public float sensibilidadeCamera ; 
    private Vector2 ultimoToqueTela;


    void Start()
    {
        rig = GetComponent<Rigidbody>();
       
        navMeshAgent = GetComponent<NavMeshAgent>();
        jogoConfig = FindObjectOfType<ControlaJogo>();

        if (jogoConfig != null)
        {
            Debug.Log("Pegou o config");            
        }        

        navMeshAgent.enabled = false;
        movimentacaoLivre = false;
    }

    void Update()
    {
        ControlarMovimentacao(); 
        ControlarCamera(); 
    }

    void ControlarCamera()
    {
        if (!jogoConfig.jogoVertical)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.position.x > Screen.width / 2)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        ultimoToqueTela = touch.position;
                    }
                    else if (touch.phase == TouchPhase.Moved)
                    {
                        Vector2 delta = touch.position - ultimoToqueTela;
                        ultimoToqueTela = touch.position;

                        transformCamera.Rotate(Vector3.up, delta.x * sensibilidadeCamera * Time.deltaTime, Space.World);
                        transformCamera.Rotate(Vector3.right, -delta.y * sensibilidadeCamera * Time.deltaTime, Space.Self);
                    }
                }
            }
        }
    }

    void ControlarMovimentacao()
    {
        if (!jogoConfig.jogoVertical)
        {
            joystick = FindObjectOfType<VirtualJoystick>();
            if (movimentacaoLivre)
            {
                navMeshAgent.enabled = false;

                Vector2 movementJoystick = joystick.GetAxis();
                Vector3 movement = new Vector3(movementJoystick.x, 0, movementJoystick.y);


                Vector3 cameraForward = transformCamera.forward;
                cameraForward.y = 0;
                cameraForward.Normalize();

                Vector3 cameraRight = transformCamera.right;
                cameraRight.y = 0;
                cameraRight.Normalize();


                Vector3 ajusteMovimento = (cameraForward * movement.z + cameraRight * movement.x).normalized;

                transform.position += ajusteMovimento * Time.deltaTime * vel;

                if (ajusteMovimento != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(ajusteMovimento);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 0);
                }
            }
            else
            {
                navMeshAgent.enabled = true;

                Vector3 cameraForward = transformCamera.forward;
                cameraForward.y = 0;
                cameraForward.Normalize();
                Vector2 movementJoystick = joystick.GetAxis();

                Vector3 cameraRight = transformCamera.right;
                cameraRight.y = 0;
                cameraRight.Normalize();


                Vector3 ajusteMovimento = (cameraForward * movementJoystick.y + cameraRight * movementJoystick.x).normalized;

                navMeshAgent.Move(ajusteMovimento * Time.deltaTime * vel);

                if (ajusteMovimento != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(ajusteMovimento);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 0);
                }
            }

            movimentacaoText.text = movimentacaoLivre.ToString();
        }

        if (jogoConfig.jogoVertical)
        {
            joystick = FindObjectOfType<VirtualJoystick>();

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

                // Movimento com base na direção do personagem
                Vector3 forwardMovement = transform.forward * movementJoystick.y * vel;
                Vector3 sideMovement = transform.right * movementJoystick.x * vel;
                Vector3 movement = forwardMovement + sideMovement;

                // Move o personagem usando NavMesh
                navMeshAgent.Move(movement * Time.deltaTime);

                // Rotaciona suavemente para a direção do movimento
                if (movement != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(movement);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * vel);
                }
            }

            movimentacaoText.text = movimentacaoLivre.ToString();
        }
    }

    public void TrocarMovimentacao()
    {
        movimentacaoLivre = !movimentacaoLivre;
        Debug.Log("Trocou a movimentação: " + movimentacaoLivre);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TriggerSala"))
        {
            TrocarMovimentacao();
            Trigger trigger = other.gameObject.GetComponent<Trigger>();
            trigger.Teste();
        }
    }
}