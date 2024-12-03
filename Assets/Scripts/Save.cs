using System.Collections;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string saveFilePath; 
    private const string SaveFileName = "saveData.json";

    void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, SaveFileName);
        Debug.Log("Save file path: " + saveFilePath);
    }


    public void SaveGame(SaveData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true); 
            File.WriteAllText(saveFilePath, json);
            Debug.Log("Sucesso");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Falha ao salvar o jogo: " + e.Message);
        }
    }


    public SaveData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(saveFilePath);
                return JsonUtility.FromJson<SaveData>(json);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Falha ao recuperar o save: " + e.Message);
            }
        }
        else
        {
            Debug.LogWarning("Save não encontrado");
        }
        return null; 
    }

    
    public bool SaveExists()
    {
        return File.Exists(saveFilePath);
    }
}
