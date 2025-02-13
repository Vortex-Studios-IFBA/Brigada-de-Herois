using Terresquall;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    VirtualJoystick joystick;
    ControlaJogo jogoConfig;
    NavMeshAgent navMeshAgent;
    AudioController audioController;
    Rigidbody rig;

    public float vel;
    public float velRotation;
    public bool movimentacaoLivre;
    bool movendoCamera = false;

    public Text movimentacaoText;

    public Transform transformCamera; 
    public float sensibilidadeCamera ; 
    private Vector2 ultimoToqueTela;

    public GameObject rechargeBt;
    public GameObject rechargeObj;

    private bool tocandoSomMovimento = false;
    private bool audioTocando;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
       
        navMeshAgent = GetComponent<NavMeshAgent>();
        jogoConfig = FindObjectOfType<ControlaJogo>();
        audioController = FindObjectOfType<AudioController>();

        if (jogoConfig != null)
        {
            Debug.Log("Pegou o config");            
        }        

        navMeshAgent.enabled = false;
        movimentacaoLivre = false;

        rechargeBt = GameObject.Find("Recharge");
        rechargeBt.SetActive(false);
    }

    void Update()
    {
        ControlarMovimentacao(); 
        ControlarCamera();
        SonsPassos();
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
                        movendoCamera = true;
                        ultimoToqueTela = touch.position;
                    }
                    
                    if (touch.phase == TouchPhase.Ended)
                    {
                        movendoCamera = false;
                    }
                }
                else
                {
                    if (touch.phase == TouchPhase.Ended)
                    {
                        movendoCamera = false;
                    }
                }
                if(movendoCamera)
                {
                    Vector2 delta = touch.position - ultimoToqueTela;
                    ultimoToqueTela = touch.position;

                    transformCamera.Rotate(Vector3.up, delta.x * sensibilidadeCamera * Time.deltaTime, Space.World);
                    transformCamera.Rotate(Vector3.right, -delta.y * sensibilidadeCamera * Time.deltaTime, Space.Self);

                    float xRot = transformCamera.eulerAngles.x - delta.y * sensibilidadeCamera * Time.deltaTime;

                    if (xRot > 180)
                        xRot -= 360;
                    xRot = Mathf.Clamp(xRot, -90f, 90f);
                    transformCamera.rotation = Quaternion.Euler(xRot, transformCamera.eulerAngles.y, transformCamera.eulerAngles.z);
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
                    tocandoSomMovimento = true;
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
                    tocandoSomMovimento = true;
                }
            }

            //movimentacaoText.text = movimentacaoLivre.ToString();
        }
        else
        {

            tocandoSomMovimento = false;
        }

        if (jogoConfig.jogoVertical)
        {
            joystick = FindObjectOfType<VirtualJoystick>();

            if (movimentacaoLivre)
            {
                //navMeshAgent.enabled = false;
                //Vector2 movementJoystick = joystick.GetAxis();
                //Vector3 movement = new Vector3(movementJoystick.x, 0, movementJoystick.y);
                //transform.position += movement * Time.deltaTime * vel;
                navMeshAgent.enabled = false;
                Vector2 movementJoystick = joystick.GetAxis();

                Vector3 forwardMovement = transform.forward * movementJoystick.y * vel;
                Vector3 sideMovement = transform.right * movementJoystick.x * vel;
                Vector3 movement = forwardMovement + sideMovement;





                transform.position += movement * Time.deltaTime;

                if (movement != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(movement);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * vel);
                    tocandoSomMovimento = true;
                }
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
                    tocandoSomMovimento = true;
                }
            }

            //movimentacaoText.text = movimentacaoLivre.ToString();
        }
        else
        {

            tocandoSomMovimento = false;
        }
    }

    public void TrocarMovimentacao()
    {
        movimentacaoLivre = !movimentacaoLivre;
        Debug.Log("Trocou a movimentação: " + movimentacaoLivre);
    }

    public void SonsPassos()
    {
        Debug.Log(tocandoSomMovimento);
        if (tocandoSomMovimento)
        {
            if (jogoConfig.efeitosSource.isPlaying == false)
            {
                jogoConfig.efeitosSource.clip = audioController.efeitosSonoros[3];
                jogoConfig.efeitosSource.loop = false;
                jogoConfig.efeitosSource.Play();
                tocandoSomMovimento = false;
                Debug.Log("Tocando Som passos");
            }
        }
        else 
        {
            if (!tocandoSomMovimento)
            {
                jogoConfig.efeitosSource.Stop();
                tocandoSomMovimento = false;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TriggerSala"))
        {
            TrocarMovimentacao();
            Trigger trigger = other.gameObject.GetComponent<Trigger>();
            trigger.Teste();
        }
        if(other.CompareTag("Recharge"))
        {
            rechargeBt.SetActive(true);
            rechargeObj = other.transform.parent.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Recharge"))
        {
            rechargeBt.SetActive(false);
        }
    }
}