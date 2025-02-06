using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Tipo{
    Agua, Espuma, Quimico
}
public class Ferramenta : MonoBehaviour
{
    [SerializeField] GameObject mangueiraObj, extintorEsObj, extintorQuimObj;
    GameObject ferramentaAtiva;
    [SerializeField] Image mangueiraImg, extintorEsImg, extintorQuimImg;
    Tipo tipoAtivo = Tipo.Agua;

    // Start is called before the first frame update
    void Start()
    {
        ferramentaAtiva = mangueiraObj;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TrocarFerramenta()
    {
        mangueiraImg.gameObject.SetActive(false);
        extintorEsImg.gameObject.SetActive(false);
        extintorQuimImg.gameObject.SetActive(false);
        switch(tipoAtivo)
        {
            case Tipo.Agua:
                tipoAtivo = Tipo.Espuma;
                extintorEsImg.gameObject.SetActive(true);
                ferramentaAtiva = extintorEsObj;
            break;
            case Tipo.Espuma:
                tipoAtivo = Tipo.Quimico;
                extintorQuimImg.gameObject.SetActive(true);
                ferramentaAtiva = extintorQuimObj;
            break;
            case Tipo.Quimico:
                tipoAtivo = Tipo.Agua;
                mangueiraImg.gameObject.SetActive(true);
                ferramentaAtiva = mangueiraObj;
            break;
        }
    }
}
