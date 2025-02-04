using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public static AudioSource audiosource;
    GameObject[] controlador;
    public static float volumeMaximo = 1;
    Slider volume;
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "configs")
        {
            volume = FindObjectOfType<Slider>();
        }


        audiosource = GetComponent<AudioSource>();

        if (audiosource != null)
        {
            Debug.Log("Controlador de audio adicionado com sucesso");
        }
        else
        {
            Debug.Log("Controlador de audio nao adicionado");
        }
    }

    void FixedUpdate()
    {
        if (volume != null)
        {
            volumeMaximo = volume.value;
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name == "Menu_Config")
        {
            volume = FindObjectOfType<Slider>();
        }
        
    }
}
