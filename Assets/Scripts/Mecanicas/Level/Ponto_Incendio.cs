using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ponto_Incendio : MonoBehaviour
{
    int nivelPerigo = 1;
    [SerializeField] int salaNum;
    bool eliminado = false;
    [SerializeField] Inimigo inim;
    [SerializeField] GameObject indicador;
    ControlaJogo missao;

    // Start is called before the first frame update
    void Awake()
    {
        missao = FindObjectOfType<ControlaJogo>();
        if(missao.Salas().Contains(salaNum))
        {
            //PEGAR INT DA CENA DO INIMIGO
            gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    public void AtualizarObjetivo(bool vitoria)
    {
        if(vitoria)
        {
            //desativar indicador de incendio
            eliminado = true;
            indicador.SetActive(false);
        }
        else if(!vitoria)
        {
            nivelPerigo += 1;
            //aumenta a dificuldade do inimigo no objetivo
        }
    }
}
