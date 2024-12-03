using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disjuntor 
{
    string nome;
    bool ligado = false;

    public void Ativaçao()
    {
        ligado = !ligado;
    }
}
public class Electricity_Box : MonoBehaviour
{
    GameObject electricity_infoView;
    Disjuntor[] disjuntores;
    // Start is called before the first frame update
    void Awake()
    {
        electricity_infoView = transform.GetChild(0).gameObject;
        electricity_infoView.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MostrarPainel(bool mode)
    {
        electricity_infoView.SetActive(mode);
    }
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            MostrarPainel(true);
        }
    }
    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            //setTrigger de animaçao de UI
            MostrarPainel(false);
        }
    }
}
