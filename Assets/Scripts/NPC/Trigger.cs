using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public GameObject[] npcs;  

    public void Teste()
    {
        foreach (GameObject npc in npcs) 
        {
            NPCController controller = npc.GetComponent<NPCController>(); 
            if (controller != null)
            {
                controller.MoverParaEntrada();
                controller.transform.position = gameObject.transform.position;
                Debug.Log("Jogador entrou na sala");
            }
        }
        Debug.Log("Jogador entrou na sala");
    }
}
