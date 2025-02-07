using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DadosFase {
    public int[] salas, inimigos;
    public float tempo;
    public int turnos;
}
public class ControlaJogo : MonoBehaviour
{
    public static ControlaJogo Instance { get; private set; }

    //private bool pausado = false;
    public int missao;
    public bool jogoVertical;

    public Text verticalTexto;

    public delegate void CenaCarregadaHandler();
    public static event CenaCarregadaHandler OnCenaCarregada;

    public AudioSource musicaSource;
    public AudioSource efeitosSource;
    public float volumeMusica = 1.0f;
    public float volumeEfeitos = 1.0f;


    DadosFase faseInfo = new DadosFase();

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
        AplicarRotacaoTela(this);
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

    public void AtualizarInfo(float time, int turns, int[] rm = null, int[] enmy = null)
    {
        if(rm != null)
            faseInfo.salas = rm;
        if(enmy != null)
            faseInfo.inimigos = enmy;
        faseInfo.tempo = time;
        faseInfo.turnos = turns;
        //guarda a condiçao das estrelas da fase
    }
    public List<int> Salas()
    {
        List<int> salass = new List<int>();
        foreach(int num in faseInfo.salas)
        {
            salass.Add(num);
        }
        return salass;
    }
    public List<int> Inimigos()
    {
        List<int> inimigoss = new List<int>();
        foreach(int num in faseInfo.salas)
        {
            inimigoss.Add(num);
        }
        return inimigoss;
    }
    public float TempoMax()
    {
        return faseInfo.tempo;
    }
    public int TurnosMax()
    {
        return faseInfo.turnos;
    }

    public void AplicarRotacaoTela(ControlaJogo instJogo)
    {
        if (instJogo.jogoVertical)
        {
            Screen.orientation = ScreenOrientation.AutoRotation;
            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = true;
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;

            //verticalTexto.text = jogoVertical.ToString();
        }
        else
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;

        }

       
    }

    public void TrocarRotacao()
    {
        ControlaJogo control = FindObjectOfType<ControlaJogo>();
        control.jogoVertical = !control.jogoVertical;
        AplicarRotacaoTela(control);
        Debug.Log("Trocou a rotacao: " + control.jogoVertical);
    }
    public void Pausar()
    {
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
    public void AjustarVolumeMusica(float volume)
    {
        volumeMusica = volume;
        if (musicaSource != null)
        {
            musicaSource.volume = volume;
        }
    }

    public void AjustarVolumeEfeitos(float volume)
    {
        volumeEfeitos = volume;
        if (efeitosSource != null)
        {
            efeitosSource.volume = volume;
        }
    }

    public void AplicarVolumes()
    {
        if (musicaSource != null)
            musicaSource.volume = volumeMusica;

        if (efeitosSource != null)
            efeitosSource.volume = volumeEfeitos;
    }

}

