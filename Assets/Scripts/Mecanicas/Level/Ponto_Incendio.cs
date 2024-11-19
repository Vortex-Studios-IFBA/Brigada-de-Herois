using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ponto_Incendio : MonoBehaviour
{
    int nivelPerigo = 1;
    bool eliminado = false;
    //TMPro contador de objetivos

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AtualizarObjetivo(bool vitoria)
    {
        if(vitoria)
        {
            //desativar indicador de incendio
            transform.GetChild(0).gameObject.SetActive(false);
            eliminado = true;
        }
        else if(!vitoria)
        {
            nivelPerigo += 1;
            //aumenta a dificuldade do inimigo no objetivo
        }
    }
}
