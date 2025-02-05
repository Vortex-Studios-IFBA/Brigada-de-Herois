using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manual_Control : MonoBehaviour
{
    [SerializeField] GameObject back, next, fechar;
    [SerializeField] Image imagem;
    [SerializeField] TMP_Text texto, titulo;
    [SerializeField] string[] txts, titulos;
    [SerializeField] Sprite[] imgs;
    int pagina = 0;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<ControlaJogo>().Pausar();
        print(pagina);
        MudarPagina(pagina);
    }
    public void MudarPagina(int x)
    {
        pagina += x;
        if(pagina < 0 || pagina > txts.Length -1)
        print(pagina);
        imagem.sprite = imgs[pagina];
        texto.text = txts[pagina];
        titulo.text = "> " + titulos[pagina];

        if(pagina > 0)
            back.SetActive(true);
        else
            back.SetActive(false);

        if(pagina < txts.Length -1)
            next.SetActive(true);
        else
            next.SetActive(false);
        if(pagina == txts.Length -1)
            fechar.SetActive(true);
        //som aqui
    }
    public void Fechar()
    {
        SceneManager.UnloadSceneAsync("Manual");
    }

    public void FecharAlt()
    {
        gameObject.SetActive(false);
    }
}
