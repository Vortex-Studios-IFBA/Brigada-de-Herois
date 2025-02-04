using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlaJogo : MonoBehaviour
{
    public static ControlaJogo Instance { get; private set; }

    private bool pausado = false;


    public int missao;
    public bool jogoVertical;

    public Text verticalTexto;

    public delegate void CenaCarregadaHandler();
    public static event CenaCarregadaHandler OnCenaCarregada;

    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        AplicarRotacaoTela();
    }

    public void EntrarFase(int index)
    {
        missao = index;
        SceneManager.LoadScene(3);
        SceneManager.sceneLoaded += CenaCarregada;
    }
    public void CarregarCena(int cenaID)
    {
        SceneManager.LoadScene(cenaID);
        SceneManager.sceneLoaded += CenaCarregada;
    }


    private void CenaCarregada(Scene cena, LoadSceneMode modo)
    {
        SceneManager.sceneLoaded -= CenaCarregada;
        OnCenaCarregada?.Invoke();
    }

    public void AplicarRotacaoTela()
    {
        if (jogoVertical)
        {
            Screen.orientation = ScreenOrientation.AutoRotation;
            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = true;
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
        }
        else
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;

        }

       
    }

    public void TrocarRotacao()
    {
        jogoVertical = !jogoVertical;
        AplicarRotacaoTela();
        Debug.Log("Trocou a rotacao: " + jogoVertical);
    }
}

