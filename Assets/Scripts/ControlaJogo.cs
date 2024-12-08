using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlaJogo : MonoBehaviour
{
    public static ControlaJogo Instance { get; private set; }

    private bool pausado = false;
    public int missao;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
