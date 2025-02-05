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
    GridLayoutGroup gridMissions;
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

        gridMissions = GetComponent<GridLayoutGroup>();
        if(FindObjectOfType<ControlaJogo>().jogoVertical == true)
        {
            gridMissions.constraintCount = 2;
            gridMissions.cellSize = new Vector2(45,45);
            gridMissions.spacing = new Vector2(30,15 - Camera.main.pixelHeight/1000);
            select.GetComponent<RectTransform>().sizeDelta = new Vector2(50,50);
        }
        else
        {
            gridMissions.constraintCount = 5;
            gridMissions.cellSize = new Vector2(80,80);
        }
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
        des_objetivos.text = "Pontos de Incêndio:\n" + mission.eliminados.ToString() + "/" + mission.objetivos.ToString();
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

        string clockDisplay = string.Format("{0:D2}:{1:D2}", minutes, seconds);

        return clockDisplay;
    }
}
