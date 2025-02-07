using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Electricity_Box : MonoBehaviour
{
    GameObject electricity_infoView;
    [SerializeField] TMP_Text estado;
    public bool ligado;
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
    public void Ativaçao()
    {
        ligado = !ligado;
        if(ligado)
        {
            estado.text = "Desligar";
        }
        else
            estado.text = "Ligar";
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
