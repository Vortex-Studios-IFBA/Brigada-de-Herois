using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public int classe;
    private bool entrouBatalha = false;
    public GameObject ponto;
    //componentes de animação
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Atacar()
    {
        //play animação de ataque
    }
    public void Derrotado()
    {
        FindObjectOfType<LevelManage>().EliminarInimigo(this);
    }
    void OnCollisionEnter(Collision col)
    {
        if (!entrouBatalha) 
        {
            entrouBatalha = true; 
            FindObjectOfType<LevelManage>().EntrarBatalha(classe, this);
            print("Entrou em batalha: " + ponto.ToString());
        }
    }
}
