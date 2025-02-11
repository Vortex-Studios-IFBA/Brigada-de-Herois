using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManage : MonoBehaviour
{
    [SerializeField] TMP_Text timer, objetivos;
    //[SerializeField] List<GameObject> salasIncendio;
    private int nivel;
    public float tempo = 0;
    public int objetivosTT = 0, objetivosFeito = 0, turnos;
    [SerializeField] GameObject[] estrelas;
    [SerializeField] GameObject ui_fase, telaResultado, cameraMain;

    private List<Inimigo> inimigosAtivos = new List<Inimigo>();
    private Inimigo inimigoAtual;

    bool concluiu;
    // Start is called before the first frame update
    void Start()
    {
        nivel = FindObjectOfType<ControlaJogo>().saveLevel;
        foreach(Ponto_Incendio ptInc in FindObjectsOfType<Ponto_Incendio>())
        {
            int salaIndex = FindObjectOfType<ControlaJogo>().Salas().FindIndex(sala => sala == ptInc.salaNum);
            EncontrarInimigos();

            if (salaIndex != -1) 
            {
                print("okfoi" + ptInc.salaNum.ToString());
                ptInc.Spawnar(FindObjectOfType<ControlaJogo>().Inimigos()[salaIndex]);
                objetivosTT += 1; 
            }
            
        }
        AtualizarContador();
        
        telaResultado = GameObject.Find("Resultados");
        telaResultado.SetActive(false);
        SceneManager.LoadSceneAsync("Manual",LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        if(!concluiu)
        {
            //e se não estiver em batalha, ou confere logo após o ultimo inimigo morrer
            if(objetivosFeito == objetivosTT)
            {
                concluiu = true;
                StartCoroutine(TerminarMissao());
                return;
            }
            tempo += Time.deltaTime;
            timer.text = ConverterTempo(tempo); 
        }
        
    }
    string ConverterTempo(float tempo)
    {
        int minutes = Mathf.FloorToInt(tempo / 60);
        int seconds = Mathf.FloorToInt(tempo % 60);

        string clockDisplay = string.Format("{0:D2}:{1:D2}", minutes, seconds);

        return clockDisplay;
    }
    public void AtualizarContador()
    {
        objetivos.text = "Objetivos: "+ objetivosFeito.ToString() +"/"+ objetivosTT.ToString();
    }
    public void EntrarBatalha(int classse, Inimigo inimigo)
    {
        inimigoAtual = inimigo;
        //tem que ver isso
        SceneManager.LoadSceneAsync(4, LoadSceneMode.Additive).completed += (asyncOp) =>
        {
            BattleManager battleManager = FindObjectOfType<BattleManager>();
            if (battleManager != null)
            {
                battleManager.PegaPonto(inimigo.ponto.GetComponent<Ponto_Incendio>());
                print("Ponto passado para BattleManager");
            }
            else
            {
                Debug.LogError("BattleManager não encontrado!");
            }
        };
        print("ok1");
        ui_fase.SetActive(false);
        cameraMain.SetActive(false);
        
    }
    public void Entrou(Ponto_Incendio pt)
    {
        FindObjectOfType<BattleManager>().pontoIncend = pt;
    }
    public void SairBatalha(int classse)
    {
        ui_fase.SetActive(true);
        cameraMain.SetActive(true);
        SceneManager.UnloadSceneAsync(4);
    }
    IEnumerator TerminarMissao()
    {
        //aqui evento fim de missao
        telaResultado.SetActive(true);
        
        int score = 1;
            if(turnos <= FindObjectOfType<ControlaJogo>().TurnosMax())
                score += 1;
            if(tempo <= FindObjectOfType<ControlaJogo>().TempoMax())
                score += 1;
            for(int i = 0; i < score; i++)
            {
                estrelas[i].SetActive(true);
                yield return new WaitForSeconds(1f);
            }  
        
        FindObjectOfType<ControlaJogo>().AtualizarInfo(nivel,tempo,turnos);
        //aqui salvar

        SaveData dados = FindObjectOfType<ControlaJogo>().save.CarregarJogo()??new SaveData(10);
        
        dados.fases[nivel].completou = true;
        dados.fases[nivel].tempo = tempo;
        dados.fases[nivel].turnos = turnos;
        
        FindObjectOfType<ControlaJogo>().save.SalvarJogo(dados);
        
        //no save tem que pegar FindObjectOfType<ControlaJogo>().TempoMax() e TurnosMax() 
        //                      salvando dentro das variaveis do respectivo nivel
        
        yield return new WaitForSeconds(5f);
        FindObjectOfType<ControlaJogo>().CarregarCena(1);
    }

    public void EncontrarInimigos()
    {
        inimigosAtivos.Clear();
        inimigosAtivos.AddRange(FindObjectsOfType<Inimigo>());
        Debug.LogWarning("Inimigos Encontrados: " +  inimigosAtivos.Count);
    }

    public void EliminarInimigo(Inimigo inimigo)
    {
        if (inimigosAtivos.Contains(inimigo))
        {
            inimigosAtivos.Remove(inimigo);
            inimigo.gameObject.SetActive(false);
            print("Inimigo derrotado e removido da lista.");
        }
    }

    public Inimigo GetInimigoAtual()
    {
        return inimigoAtual;
    }
}
