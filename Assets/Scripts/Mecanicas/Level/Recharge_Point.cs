using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//mover enum para class Ferramenta
public enum ToolType {
    Mangueira, ExtAgua, ExtQuimico
}
public class Recharge_Point : MonoBehaviour
{
    GameObject recharge_infoView;
    //Precisa criar uma classe Ferramenta para carregar seus ataques
    
    // Start is called before the first frame update
    void Awake()
    {
        recharge_infoView = transform.GetChild(0).gameObject;
        recharge_infoView.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MostrarPainel(bool mode)
    {
        recharge_infoView.SetActive(mode);
    }
    public void Recarregar()
    {
        //aumentar cargas de ataques de acordo com o tipo de ferramenta
        Destroy(gameObject);
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
