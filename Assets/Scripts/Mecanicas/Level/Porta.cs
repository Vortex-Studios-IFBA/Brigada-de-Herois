using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porta : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip[] sfx;
    AudioSource open;
    public bool locked = true;
    Animator door;
    public bool isOpening = false;
    void Start()
    {
        open = GetComponent<AudioSource>();
        door = GetComponent<Animator>();
        open.clip = sfx[0];
        //condiçao pra destrancar
    }

    IEnumerator Abrir()
    {
        if(isOpening)
        yield break;

        print("aaaaaaaaaa");
        isOpening = true;
        open.clip = sfx[1];
        open.volume = 1;
        open.Play();
        yield return new WaitForSeconds(0.5f);
        door.Play("DoorOpen");
        yield return new WaitForSeconds(5f);
        //door.Play("New State");
        //isOpening = false;
    }
    void OnTriggerEnter(Collider col)
    {
        if(!locked)
        {
            StartCoroutine(Abrir());
        }
        else
        {
            open.volume = 0.5f;
            open.Play();
            print("Trancado");
        }
    }
}
