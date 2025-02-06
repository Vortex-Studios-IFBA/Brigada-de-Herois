using UnityEngine;
using UnityEngine.UI;

public class ConfigAudio : MonoBehaviour
{
    public Slider sliderMusica;
    public Slider sliderEfeitos;

    private void Start()
    {
        sliderMusica.value = ControlaJogo.Instance.volumeMusica;
        sliderEfeitos.value = ControlaJogo.Instance.volumeEfeitos;

        sliderMusica.onValueChanged.AddListener(AjustarMusica);
        sliderEfeitos.onValueChanged.AddListener(AjustarEfeitos);
    }

    private void AjustarMusica(float volume)
    {
        ControlaJogo.Instance.AjustarVolumeMusica(volume);
    }

    private void AjustarEfeitos(float volume)
    {
        ControlaJogo.Instance.AjustarVolumeEfeitos(volume);
    }
}
