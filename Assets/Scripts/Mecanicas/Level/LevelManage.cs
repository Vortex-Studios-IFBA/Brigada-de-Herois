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

    bool concluiu;
    // Start is called before the first frame update
    void Start()
    {
        nivel = FindObjectOfType<ControlaJogo>().saveLevel;
        foreach(Ponto_Incendio ptInc in FindObjectsOfType<Ponto_Incendio>())
        {
            if(FindObjectOfType<ControlaJogo>().Salas().Contains(ptInc.salaNum))
            {
                print("okfoi");
                ptInc.Spawnar();
                objetivosTT += 1; 
            }
            
        }
        AtualizarContador();
        
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
    public void EntrarBatalha()
    {
        
    }
    IEnumerator TerminarMissao()
    {
        
        //aqui evento fim de missao
        
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

        SaveData dados = FindObjectOfType<ControlaJogo>().save.CarregarJogo()??new SaveData(nivel);
        
        dados.fases[nivel].completou = true;
        dados.fases[nivel].tempo = tempo;
        dados.fases[nivel].turnos = turnos;
        
        FindObjectOfType<ControlaJogo>().save.SalvarJogo(dados);
        
        //no save tem que pegar FindObjectOfType<ControlaJogo>().TempoMax() e TurnosMax() 
        //                      salvando dentro das variaveis do respectivo nivel
        
        yield return new WaitForSeconds(5f);
        FindObjectOfType<ControlaJogo>().CarregarCena(1);
    }
}
