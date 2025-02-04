using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    private GameObject uiVertical;
    private GameObject uiHorizontal;
    private GameObject botaoRotacao;
    private GameObject botaoJogar;
    private GameObject botaoConfig;


    private ControlaJogo jogoConfig;

    private bool estadoAtual;


    void Start()
    {
        uiVertical = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(obj => obj.name == "UI_Vertical");
        uiHorizontal = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(obj => obj.name == "UI_Horizontal");


        if (uiVertical == null || uiHorizontal == null)
        {
            Debug.LogError("UI_Vertical ou UI_Horizontal não foram encontrados na cena!");
            return;
        }
        else
        {
            Debug.Log("UI Associada com sucesso");
        }

        

        jogoConfig = ControlaJogo.Instance;

        if (jogoConfig != null)
        {
            Debug.Log("Configuração do jogo carregada com sucesso.");
            estadoAtual = jogoConfig.jogoVertical;
            TrocarUI(); 
        }
        else
        {
            Debug.LogError("ControlaJogo não encontrado!");
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            botaoJogar = GameObject.Find("BtnJogar");
            if (botaoJogar == null)
            {
                Debug.LogError("Botao de jogar nao encontrado");
            }
            else
            {
                botaoJogar.GetComponent<Button>().onClick.RemoveAllListeners();
                botaoJogar.GetComponent<Button>().onClick.AddListener(() => ControlaJogo.Instance.CarregarCena(1));

            }
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            botaoConfig = GameObject.Find("BtnConfig");
            if (botaoConfig == null)
            {
                Debug.LogError("Botao de config nao encontrado");
            }
            else
            {
                botaoConfig.GetComponent<Button>().onClick.RemoveAllListeners();
                botaoConfig.GetComponent<Button>().onClick.AddListener(() => ControlaJogo.Instance.CarregarCena(2));

            }
        }
    }

    void Update()
    {
        if (jogoConfig != null && jogoConfig.jogoVertical != estadoAtual)
        {
            estadoAtual = jogoConfig.jogoVertical;
            TrocarUI();
        }
    }

    public void TrocarUI()
    {
        if (jogoConfig == null) return;

        if (jogoConfig.jogoVertical)
        {
            uiHorizontal.SetActive(false);
            uiVertical.SetActive(true);
        }
        else
        {
            uiHorizontal.SetActive(true);
            uiVertical.SetActive(false);
        }

        botaoRotacao = GameObject.Find("BtdTrocarRotacao");
        if (botaoRotacao == null)
        {
            Debug.LogError("Botao de rotação nao encontrado");
        }
        else
        {
            botaoRotacao.GetComponent<Button>().onClick.RemoveAllListeners();
            botaoRotacao.GetComponent<Button>().onClick.AddListener(() => ControlaJogo.Instance.TrocarRotacao());

        }
    }
}
