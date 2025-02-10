using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//mover enum para class Ferramenta
public enum ToolType {
    Mangueira, ExtEspuma, ExtQuimico
}
public class Recharge_Point : MonoBehaviour
{
    [SerializeField] ToolType tipoRecarga;
    [SerializeField] TMP_Text nome;
    GameObject recharge_infoView;
    //Precisa criar uma classe Ferramenta para carregar seus ataques
    
    // Start is called before the first frame update
    void Awake()
    {
        recharge_infoView = transform.GetChild(0).gameObject;
        switch(tipoRecarga)
        {
            case ToolType.Mangueira:
            nome.text = "Água";
            break;
            case ToolType.ExtEspuma:
            nome.text = "Esp.";
            break;
            case ToolType.ExtQuimico:
            nome.text = "CO2";
            break;
        }
        
        recharge_infoView.SetActive(false);
    }

    // Update is called once per frame
    public void MostrarPainel(bool mode)
    {
        recharge_infoView.SetActive(mode);
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
            //setTrigger e animação de UI
            MostrarPainel(false);
        }
    }
}
