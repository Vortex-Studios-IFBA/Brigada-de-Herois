using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    public GameObject uiVertical;
    public GameObject uiHorizontal;

    ControlaJogo jogoConfig;
    void Start()
    {
        jogoConfig = FindObjectOfType<ControlaJogo>();

        if (jogoConfig != null)
        {
            Debug.Log("Pegou o config");
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        TrocarUI();
    }

    public void TrocarUI()
    {
        if (jogoConfig.jogoVertical)
        {
            uiHorizontal.SetActive(false);
            uiVertical.SetActive(true);
        }
        if (!jogoConfig.jogoVertical)
        {
            uiHorizontal.SetActive(true);
            uiVertical.SetActive(false);
        }
    }
}
