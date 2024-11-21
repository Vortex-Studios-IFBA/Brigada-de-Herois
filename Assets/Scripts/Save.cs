using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{
    public Missao[] save_missoes = new Missao[10];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SalvarResultado(bool completou, int turnoss, float tempo)
    {
        //salvar quando concluir a fase
        int id = FindObjectOfType<ControlaJogo>().missao;
        save_missoes[id] = new Missao(completou,turnoss,tempo);
    }
    public void CarregarSave()
    {

    }
}
