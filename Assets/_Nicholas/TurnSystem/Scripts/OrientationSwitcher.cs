using UnityEngine;

public class OrientationSwitcher : MonoBehaviour
{
    public Canvas canvasPortrait;
    public Canvas canvasLandscape;
    public GameObject gameObjectPortrait;
    public GameObject gameObjectLandscape;

    private void Start()
    {
        ApplyOrientation(PlayerPrefs.GetInt("LastOrientation", 1) == 1);
    }

    public void SwitchOrientation()
    {
        bool isPortrait = Screen.orientation == ScreenOrientation.Portrait;

        if (isPortrait)
        {
            ApplyOrientation(false);
            PlayerPrefs.SetInt("LastOrientation", 0); // Salvar Landscape
        }
        else
        {
            ApplyOrientation(true);
            PlayerPrefs.SetInt("LastOrientation", 1); // Salvar Portrait
        }

        PlayerPrefs.Save();
    }

    private void ApplyOrientation(bool isPortrait)
    {
        canvasPortrait.gameObject.SetActive(isPortrait);
        gameObjectPortrait.SetActive(isPortrait);

        canvasLandscape.gameObject.SetActive(!isPortrait);
        gameObjectLandscape.SetActive(!isPortrait);

        Screen.orientation = isPortrait ? ScreenOrientation.Portrait : ScreenOrientation.LandscapeLeft;
    }
}
