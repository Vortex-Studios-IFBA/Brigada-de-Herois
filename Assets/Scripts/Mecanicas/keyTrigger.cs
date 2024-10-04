using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class keyTrigger : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D key) 
    {
        if(key.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            SceneManager.UnloadSceneAsync(1);
        }
    }
}
