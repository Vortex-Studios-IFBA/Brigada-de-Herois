using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public int classe;
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
    void OnCollisionEnter(Collision col)
    {
        FindObjectOfType<ControlaJogo>().CarregarCena(4);
    }
}
