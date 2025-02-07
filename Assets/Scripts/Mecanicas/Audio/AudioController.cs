using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip[] musicas; 
    public AudioClip[] efeitosSonoros;

    private ControlaJogo jogo;
    void Start()
    {
        jogo = GetComponent<ControlaJogo>();
    }

    void Update()
    {
        
    }

    public void TocarMusica(int index)
    {
        if (jogo.musicaSource != null && musicas.Length > index)
        {
            jogo.musicaSource.clip = musicas[index];
            jogo.musicaSource.loop = true;
            jogo.musicaSource.Play();
        }
    }

    public void TocarEfeito(int index)
    {
        if (jogo.efeitosSource != null && efeitosSonoros.Length > index)
        {
            jogo.efeitosSource.PlayOneShot(efeitosSonoros[index]);
        }
    }

    public void AjustarVolumeMusica(float volume)
    {
        if (jogo.musicaSource != null)
            jogo.musicaSource.volume = volume;
    }

    public void AjustarVolumeEfeitos(float volume)
    {
        if (jogo.efeitosSource != null)
            jogo.efeitosSource.volume = volume;
    }
}
