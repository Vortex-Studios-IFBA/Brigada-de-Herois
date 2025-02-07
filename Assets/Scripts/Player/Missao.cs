using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Missao : MonoBehaviour
{
    public bool concluida;
    public int turnosMax, totalTurnos, objetivos, eliminados;
    public float tempoMax, tempoFinal;
    public int[] rooms, inimigos;

    // Start is called before the first frame update
    void Start()
    {
        //antes disso receber resultados da fase
        VerificarPontuacao();
    }
    public void IniciarMissao()
    {
        //scenemanager.loadScene
    }
    public void SairMissao()
    {
        //scenemanager.loadScene(Selecao Fase)
    }
    void VerificarPontuacao()
    {
        int score = 0;
        if(concluida == true)
            score += 1;
        if(score > 0)
        {
            if(totalTurnos <= turnosMax)
                score += 1;
            if(tempoFinal <= tempoMax)
                score += 1;
            for(int i = 0; i < score; i++)
            {
                transform.GetChild(1).GetChild(i).gameObject.GetComponent<Image>().color = new Color32(255,153,16,255);
            }  
        }
    }
}
