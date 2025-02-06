using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    private GameObject uiVertical;
    private GameObject uiHorizontal;
    private GameObject butaoRotacao;


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

        butaoRotacao = GameObject.Find("BtdTrocarRotacao");
        if (butaoRotacao == null)
        {
            Debug.LogError("Botao de rotação nao encontrado");
        }
        else
        {
            butaoRotacao.GetComponent<Button>().onClick.RemoveAllListeners();
            butaoRotacao.GetComponent<Button>().onClick.AddListener(() => ControlaJogo.Instance.TrocarRotacao());

        }
    }
}
