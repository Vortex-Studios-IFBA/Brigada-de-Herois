using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ponto_Incendio : MonoBehaviour
{
    int nivelPerigo = 1;
    public int salaNum;
    [SerializeField] Inimigo inim;
    [SerializeField] GameObject indicador;

    // Start is called before the first frame update
    void Awake()
    {
        inim.ponto = this.gameObject;
    }

    // Update is called once per frame
    public void AtualizarObjetivo(bool vitoria)
    {
        FindObjectOfType<LevelManage>().SairBatalha(inim.classe);
        Debug.Log("Atualizou objetivo");
        Debug.Log("Objetivo antes do if: " + FindObjectOfType<LevelManage>().objetivosFeito);
        if(vitoria)
        {
            //desativar indicador de incendio
            indicador.SetActive(false);
            FindObjectOfType<LevelManage>().objetivosFeito += 1;
            Debug.Log("Objetivos feitos é:" + FindObjectOfType<LevelManage>().objetivosFeito);
        }
        else if(!vitoria)
        {
            nivelPerigo += 1;
            //aumenta a dificuldade do inimigo no objetivo
        }
    }
    public void Spawnar(int claasse)
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        inim.classe = claasse;
        print("inimigoclasse" + inim.classe.ToString());
    }
}
