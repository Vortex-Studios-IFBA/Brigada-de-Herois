using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyGet : MonoBehaviour
{
    public string doorName;
    public Text showName;
    // Start is called before the first frame update
    void Start()
    {
        showName = GetComponentInChildren<Text>();
        showName.text = "Chave "+doorName;
        showName.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
    void OnTriggerEnter(Collider col)
    {
        print("Chave "+doorName);
        //showName.gameObject.SetActive(true);
    }
    void OnTriggerExit(Collider col)
    {
        //showName.gameObject.SetActive(false);
    }
}
