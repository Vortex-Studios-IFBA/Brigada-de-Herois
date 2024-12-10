using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlaJogo : MonoBehaviour
{
<<<<<<< Updated upstream
    bool pausado = false;
    public int missao;
    // Start is called before the first frame update
    void Awake()
=======
    public static ControlaJogo Instance { get; private set; }

    private bool pausado = false;

    public int missao;

    public bool jogoVertical = false;

    public Text verticalTexto;

    private void Awake()
>>>>>>> Stashed changes
    {
        //corrigir depois pra quando voltar ao menu
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        AplicarRotacaoTela();
    }

    // Update is called once per frame
    public void CarregarCena(int cenaID)
    {
        SceneManager.LoadScene(cenaID);
    }
    public void EntrarFase(int index)
    {
        missao = index;
        SceneManager.LoadScene(3);
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

            verticalTexto.text = jogoVertical.ToString();
        }
        else
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;  
            verticalTexto.text = jogoVertical.ToString();
        }
    }

    public void TrocarRotacao()
    {
        jogoVertical = !jogoVertical;
        AplicarRotacaoTela();
        Debug.Log("Trocou a rotacao: " + jogoVertical);
    }
}
