using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_MissionSelect : MonoBehaviour
{
    [SerializeField] TMP_Text des_objetivos, des_turnos, des_tempo; //caixa de texto de desempenhos
    [SerializeField] GameObject select;
    GameObject missao_selecionada;
    //int objetivosTT = 0, turnosTT = 0;
    //float tempo = 0;
    // Start is called before the first frame update
    void Start()
    {
        int prevId = -1;
        for(int i = 0; i < transform.childCount; i++)
        {
            int num = 1 + i;
            TMP_Text levelId = transform.GetChild(i).GetChild(0).GetComponent<TMP_Text>();
            levelId.text = num.ToString();
            if(num > 1 && prevId >= 0)
            {
                if(!transform.GetChild(prevId).GetComponent<Missao>().concluida)
                {
                    transform.GetChild(i).GetComponent<Button>().interactable = false;
                }
            }
            prevId = i;
        }
        des_objetivos.text = "";
        des_turnos.text = "";
        des_tempo.text = "";
    }
    public void SelecionarMissao(GameObject missao)
    {
        if(missao == missao_selecionada && missao_selecionada != null)
        {
            FindObjectOfType<ControlaJogo>().AtualizarInfo(0,0,missao.GetComponent<Missao>().rooms);
            FindObjectOfType<ControlaJogo>().EntrarFase(missao.transform.GetSiblingIndex());
        }
        else if(missao != missao_selecionada)
        {
            missao_selecionada = missao;
            select.transform.position = missao_selecionada.transform.position;
            select.SetActive(true);
        }
        AtualizarResultados();
    }
    void AtualizarResultados()
    {
        Missao mission = missao_selecionada.GetComponent<Missao>();
        des_objetivos.text = "Pontos de Incêndio:\n" + mission.objetivos.ToString() + "/" + mission.eliminados.ToString();
        des_turnos.text = "Turnos:\n" + mission.totalTurnos.ToString();

        float timer = mission.tempoFinal;
        string tempoTxt = ConverterTempo(timer);
        if(timer == 0)
            tempoTxt = "--:--";
        des_tempo.text = "Tempo:\n" + tempoTxt;
    }
    string ConverterTempo(float tempo)
    {
        int minutes = Mathf.FloorToInt(tempo / 60);
        int seconds = Mathf.FloorToInt(tempo % 60);

        // Format the time as MM:SS
        string clockDisplay = string.Format("{0:D2}:{1:D2}", minutes, seconds);

        return clockDisplay;
    }
}
