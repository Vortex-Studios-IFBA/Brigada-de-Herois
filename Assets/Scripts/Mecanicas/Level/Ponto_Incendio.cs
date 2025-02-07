using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ponto_Incendio : MonoBehaviour
{
    int nivelPerigo = 1;
    public int salaNum;
    bool eliminado = false;
    [SerializeField] Inimigo inim;
    [SerializeField] GameObject indicador;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    public void AtualizarObjetivo(bool vitoria)
    {
        if(vitoria)
        {
            //desativar indicador de incendio
            eliminado = true;
            indicador.SetActive(false);
            FindObjectOfType<LevelManage>().objetivosFeito += 1;
        }
        else if(!vitoria)
        {
            nivelPerigo += 1;
            //aumenta a dificuldade do inimigo no objetivo
        }
    }
    public void Spawnar()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        //inim.classe = 
        //PEGAR INT DA CENA EM controla jogo Inimigos()
    }
}
