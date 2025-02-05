using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManage : MonoBehaviour
{
    [SerializeField] TMP_Text timer, objetivos;
    //[SerializeField] List<GameObject> salasIncendio;
    public float tempo = 0;
    public int objetivosTT = 0, objetivosFeito = 0, turnos;

    bool concluiu;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Ponto_Incendio ptInc in FindObjectsOfType<Ponto_Incendio>())
        {
            objetivosTT += 1;
        }
        AtualizarContador();
        
        SceneManager.LoadSceneAsync("Manual",LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        if(!concluiu)
        {
            if(objetivosFeito == objetivosTT)
            {
                concluiu = true;
                StartCoroutine(TerminarMissao());
                return;
            }
            tempo += Time.deltaTime;
            timer.text = ConverterTempo(tempo); 
        }
        
    }
    string ConverterTempo(float tempo)
    {
        int minutes = Mathf.FloorToInt(tempo / 60);
        int seconds = Mathf.FloorToInt(tempo % 60);

        string clockDisplay = string.Format("{0:D2}:{1:D2}", minutes, seconds);

        return clockDisplay;
    }
    public void AtualizarContador()
    {
        objetivos.text = "Objetivos: "+ objetivosTT.ToString() +"/"+ objetivosFeito.ToString();
    }
    IEnumerator TerminarMissao()
    {
        FindObjectOfType<ControlaJogo>().AtualizarInfo(tempo,turnos);
        //aqui evento fim de missao
        
        //aqui salvar
        
        yield return new WaitForSeconds(5f);
        FindObjectOfType<ControlaJogo>().CarregarCena(1);
    }
}
