using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_MissionSelect : MonoBehaviour
{
    [SerializeField] TMP_Text des_objetivos, des_turnos, des_tempo; //caixa de texto de desempenhos
    GameObject missao_selecionada;
    //int objetivosTT = 0, turnosTT = 0;
    //float tempo = 0;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text = (1 + i).ToString();
        }
        des_objetivos.text = "";
        des_turnos.text = "";
        des_tempo.text = "";
    }
    public void SelecionarMissao(GameObject missao)
    {
        if(missao == missao_selecionada && missao_selecionada != null)
        {
            //entrar na cena de missao
        }
        else if(missao != missao_selecionada)
        {
            missao_selecionada = missao;
        }
        AtualizarResultados();
    }
    void AtualizarResultados()
    {
        Missao mission = missao_selecionada.GetComponent<Missao>();
        des_objetivos.text = "Pontos de Incêndio:\n" + mission.objetivos.ToString() + "/" + mission.eliminados.ToString();
        des_turnos.text = "Turnos:\n" + mission.totalTurnos.ToString();
        des_tempo.text = "Tempo:\n" + mission.tempoFinal.ToString();
    }
}
