using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public static AudioSource audiosource;  
    public float volumeMaximo = 1;
    public Slider volume;
    void Start()
    {
       
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


}
