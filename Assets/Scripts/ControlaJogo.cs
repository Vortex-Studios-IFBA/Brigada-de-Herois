using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlaJogo : MonoBehaviour
{
    bool pausado = false;
    public int missao;
    // Start is called before the first frame update
    void Awake()
    {
        //corrigir depois pra quando voltar ao menu
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    public void CarregarCena(int cenaID)
    {
        SceneManager.LoadScene(cenaID);
    }
    public void EntrarFase(int index)
    {
        missao = index;
        SceneManager.LoadScene(3);
    }
}